using System;

using ilf.pgn.Data;
using Bau.Libraries.LibChessGame.Board;
using Bau.Libraries.LibChessGame.Board.Movements;
using Bau.Libraries.LibChessGame.Board.Pieces;
using Bau.Libraries.LibChessGame.Games;

namespace Bau.Libraries.LibChessGame.Parser
{
	/// <summary>
	///		Intérprete de movimientos de un juego
	/// </summary>
	internal class GameMovementsParser
	{
		/// <summary>
		///		Carga los datos de un juego
		/// </summary>
		internal MovementModelCollection Parse(VariationModel variation, Game game)
		{
			MovementModelCollection movements = new MovementModelCollection();
			PieceBaseModel.PieceColor actualColor = PieceBaseModel.PieceColor.White;

				// Inicializa el tablero
				variation.GameBoard.Reset();
				// Cambia el color inicial
				if (variation.Setup.HasSetup && !variation.Setup.IsWhiteMove)
					actualColor = PieceBaseModel.PieceColor.Black;
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
								movements.Add(new MovementRemarksModel(comment.Comment));
							break;
						case MovePairEntry movement:
								// Añade el movimiento de blancas
								movements.Add(ParseMovement(variation.GameBoard, actualColor, (move as MovePairEntry).White));
								actualColor = GetNextColor(actualColor);
								// Añade el movimiento de negras
								movements.Add(ParseMovement(variation.GameBoard, actualColor, (move as MovePairEntry).Black));
								actualColor = GetNextColor(actualColor);
							break;
						case HalfMoveEntry movement:
								movements.Add(ParseMovement(variation.GameBoard, actualColor, (move as HalfMoveEntry).Move));
								actualColor = GetNextColor(actualColor);
							break;
						case GameEndEntry movement:
								movements.Add(new MovementGameEndModel(ConvertResult(movement.Result)));
							break;
						default:
								System.Diagnostics.Debug.WriteLine(move.GetType().ToString());
							break;
					}
				// Devuelve la colección de movimientos
				return movements;
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
		private MovementFigureModel ParseMovement(GameBoardModel board, PieceBaseModel.PieceColor actualColor, Move move)
		{
			MovementFigureModel newMovement = new MovementFigureModel();
			CellConversor cellConversor = new CellConversor();

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
																  cellConversor.ConvertCell(move.TargetSquare.Rank, move.TargetSquare.File),
																  cellConversor.ConvertCell(move.OriginRank, move.OriginFile),
																  targetPiece);
						break;
				}
				newMovement.Annotation = ConvertAnnotation(move.Annotation);
				// Ejecuta los movimientos sobre el tablero
				board.Execute(newMovement);
				// Devuelve el movimiento
				return newMovement;
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
