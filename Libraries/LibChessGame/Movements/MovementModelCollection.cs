using System;
using System.Collections.Generic;

using ilf.pgn.Data;
using Bau.Libraries.LibChessGame.Board;
using Bau.Libraries.LibChessGame.Pieces;

namespace Bau.Libraries.LibChessGame.Movements
{
	/// <summary>
	///		Colección de <see cref="MovementFigureModel"/>
	/// </summary>
	public class MovementModelCollection : List<MovementBaseModel>
	{
		/// <summary>
		///		Carga los datos de un juego
		/// </summary>
		internal void Load(Game game)
		{
			PieceBaseModel.PieceColor actualColor = PieceBaseModel.PieceColor.White;
			GameBoardModel board = new GameBoardModel();

				// Inicializa el tablero
				board.Reset();
				if (game.BoardSetup != null)
				{
					// Cambia el color inicial
					if (!game.BoardSetup.IsWhiteMove)
						actualColor = PieceBaseModel.PieceColor.Black;
				}
				// Carga los movimientos
				foreach (MoveTextEntry move in game.MoveText)
					switch (move)
					{
						case RAVEntry rav:
								System.Diagnostics.Debug.WriteLine(move.GetType().ToString());
							break;
						case NAGEntry nag:
								System.Diagnostics.Debug.WriteLine(move.GetType().ToString());
							break;
						case CommentEntry comment:
								Add(new MovementRemarksModel(comment.Comment));
							break;
						case MovePairEntry movement:
								actualColor = AddMovement(board, actualColor, (move as MovePairEntry).White);
								actualColor = AddMovement(board, actualColor, (move as MovePairEntry).Black);
							break;
						case HalfMoveEntry movement:
								actualColor = AddMovement(board, actualColor, (move as HalfMoveEntry).Move);
							break;
						case GameEndEntry movement:
								Add(new MovementGameEndModel(ConvertResult(movement.Result)));
							break;
						default:
								System.Diagnostics.Debug.WriteLine(move.GetType().ToString());
							break;
					}
		}

		/// <summary>
		///		Convierte el resultado de un juego
		/// </summary>
		private GameModel.ResultType ConvertResult(GameResult result)
		{
			switch (result)
			{
				case GameResult.Black:
					return GameModel.ResultType.BlackWins;
				case GameResult.White:
					return GameModel.ResultType.WhiteWins;
				case GameResult.Draw:
					return GameModel.ResultType.Draw;
				default:
					return GameModel.ResultType.Open;
			}
		}

		/// <summary>
		///		Añade un movimiento a la lista
		/// </summary>
		private PieceBaseModel.PieceColor AddMovement(GameBoardModel board, PieceBaseModel.PieceColor actualColor, Move move)
		{
			MovementFigureModel newMovement = new MovementFigureModel();

				// Asigna los datos
				newMovement.Color = actualColor;
				newMovement.Type = ConvertMovement(move.Type);
				newMovement.Text = move.ToString();
				newMovement.IsCheck = move.IsCheck ?? false;
				newMovement.IsDoubleCheck = move.IsDoubleCheck ?? false;
				newMovement.IsCheckMate = move.IsCheckMate ?? false;
				switch (newMovement.Type)
				{
					case MovementFigureModel.MovementType.CastleKingSide:
					case MovementFigureModel.MovementType.CastleQueenSide:
							// Asigna la pieza de origen
							newMovement.OriginPiece = PieceBaseModel.PieceType.Rook;
							// Crea las acciones de enroque
							newMovement.Actions.CreateCastleKingActions(board, newMovement);
						break;
					default:
							PieceBaseModel.PieceType? targetPiece = null;

								// Asigna los datos
								newMovement.OriginPiece = ConvertPiece(move.Piece);
								targetPiece = ConvertPiece(move.TargetPiece);
								// Añade la pieza promocionada
								if (move.PromotedPiece != null)
									newMovement.PromotedPiece = ConvertPiece(move.PromotedPiece);
								// Crea las acciones
								newMovement.Actions.CreateActions(board, newMovement,
																  ConvertCell(move.TargetSquare.Rank, move.TargetSquare.File),
																  ConvertCell(move.OriginRank, move.OriginFile),
																  targetPiece);
						break;
				}
				newMovement.Annotation = ConvertAnnotation(move.Annotation);
				// Ejecuta los movimientos sobre el tablero
				board.Execute(newMovement);
				// Añade el movimiento
				Add(newMovement);
				// Devuelve el color del siguiente movimient
				return actualColor == PieceBaseModel.PieceColor.White ? PieceBaseModel.PieceColor.Black : PieceBaseModel.PieceColor.White;
		}

		/// <summary>
		///		Obtiene el color del siguiente movimiento
		/// </summary>
		private PieceBaseModel.PieceColor GetNextColor(PieceBaseModel.PieceColor actualColor)
		{
			return actualColor == PieceBaseModel.PieceColor.White ? PieceBaseModel.PieceColor.Black : PieceBaseModel.PieceColor.White;
		}

		/// <summary>
		///		Convierte un tipo de movimiento
		/// </summary>
		private MovementFigureModel.MovementType ConvertMovement(MoveType type)
		{
			switch (type)
			{
				case MoveType.Capture:
					return MovementFigureModel.MovementType.Capture;
				case MoveType.CaptureEnPassant:
					return MovementFigureModel.MovementType.CaptureEnPassant;
				case MoveType.CastleKingSide:
					return MovementFigureModel.MovementType.CastleKingSide;
				case MoveType.CastleQueenSide:
					return MovementFigureModel.MovementType.CastleQueenSide;
				case MoveType.Simple:
					return MovementFigureModel.MovementType.Normal;
				default:
					throw new NotImplementedException();
			}
		}

		/// <summary>
		///		Convierte una celda
		/// </summary>
		private CellModel ConvertCell(int? row, File? file)
		{
			return new CellModel(ConvertRow(row), ConvertColumn(file));
		}

		/// <summary>
		///		Convierte una columna
		/// </summary>
		private int ConvertColumn(File? file)
		{
			if (file == null)
				return -1;
			else
				switch (file)
				{
					case File.A:
						return 0;
					case File.B:
						return 1;
					case File.C:
						return 2;
					case File.D:
						return 3;
					case File.E:
						return 4;
					case File.F:
						return 5;
					case File.G:
						return 6;
					case File.H:
						return 7;
					default:
						throw new NotImplementedException();
				}
		}

		/// <summary>
		///		Convierte una fila
		/// </summary>
		private int ConvertRow(int? row)
		{
			if (row == null)
				return -1;
			else
				return 8 - (row ?? 0);
		}

		/// <summary>
		///		Obtiene el tipo de pieza
		/// </summary>
		private PieceBaseModel.PieceType ConvertPiece(PieceType? piece)
		{
			switch (piece)
			{
				case PieceType.Bishop:
					return PieceBaseModel.PieceType.Bishop;
				case PieceType.King:
					return PieceBaseModel.PieceType.King;
				case PieceType.Knight:
					return PieceBaseModel.PieceType.Knight;
				case PieceType.Pawn:
					return PieceBaseModel.PieceType.Pawn;
				case PieceType.Queen:
					return PieceBaseModel.PieceType.Queen;
				case PieceType.Rook:
					return PieceBaseModel.PieceType.Rook;
				default:
					throw new NotImplementedException();
			}
		}

		/// <summary>
		///		Convierte una anotación
		/// </summary>
		private MovementFigureModel.AnnotationType? ConvertAnnotation(MoveAnnotation? annotation)
		{
			if (annotation == null)
				return null;
			else
				switch (annotation)
				{
					case MoveAnnotation.MindBlowing:
						return MovementFigureModel.AnnotationType.MindBlowing;
					case MoveAnnotation.Brilliant:
						return MovementFigureModel.AnnotationType.Brilliant;
					case MoveAnnotation.Good:
						return MovementFigureModel.AnnotationType.Good;
					case MoveAnnotation.Interesting:
						return MovementFigureModel.AnnotationType.Interesting;
					case MoveAnnotation.Dubious:
						return MovementFigureModel.AnnotationType.Dubious;
					case MoveAnnotation.Mistake:
						return MovementFigureModel.AnnotationType.Mistake;
					case MoveAnnotation.Blunder:
						return MovementFigureModel.AnnotationType.Blunder;
					case MoveAnnotation.Abysmal:
						return MovementFigureModel.AnnotationType.Abysmal;
					case MoveAnnotation.FascinatingButUnsound:
						return MovementFigureModel.AnnotationType.FascinatingButUnsound;
					case MoveAnnotation.Unclear:
						return MovementFigureModel.AnnotationType.Unclear;
					case MoveAnnotation.WithCompensation:
						return MovementFigureModel.AnnotationType.WithCompensation;
					case MoveAnnotation.EvenPosition:
						return MovementFigureModel.AnnotationType.EvenPosition;
					case MoveAnnotation.SlightAdvantageWhite:
						return MovementFigureModel.AnnotationType.SlightAdvantageWhite;
					case MoveAnnotation.SlightAdvantageBlack:
						return MovementFigureModel.AnnotationType.SlightAdvantageBlack;
					case MoveAnnotation.AdvantageWhite:
						return MovementFigureModel.AnnotationType.AdvantageWhite;
					case MoveAnnotation.AdvantageBlack:
						return MovementFigureModel.AnnotationType.AdvantageBlack;
					case MoveAnnotation.DecisiveAdvantageWhite:
						return MovementFigureModel.AnnotationType.DecisiveAdvantageWhite;
					case MoveAnnotation.DecisiveAdvantageBlack:
						return MovementFigureModel.AnnotationType.DecisiveAdvantageBlack;
					case MoveAnnotation.Space:
						return MovementFigureModel.AnnotationType.Space;
					case MoveAnnotation.Initiative:
						return MovementFigureModel.AnnotationType.Initiative;
					case MoveAnnotation.Development:
						return MovementFigureModel.AnnotationType.Development;
					case MoveAnnotation.Counterplay:
						return MovementFigureModel.AnnotationType.Counterplay;
					case MoveAnnotation.Countering:
						return MovementFigureModel.AnnotationType.Countering;
					case MoveAnnotation.Idea:
						return MovementFigureModel.AnnotationType.Idea;
					case MoveAnnotation.TheoreticalNovelty:
						return MovementFigureModel.AnnotationType.TheoreticalNovelty;
					default:
						return MovementFigureModel.AnnotationType.UnknownAnnotation;
				}
		}
	}
}
