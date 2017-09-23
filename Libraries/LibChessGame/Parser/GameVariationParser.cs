using System;
using System.Collections.Generic;

using ilf.pgn.Data;
using Bau.Libraries.LibChessGame.Games;
using Bau.Libraries.LibChessGame.Board.Pieces;

namespace Bau.Libraries.LibChessGame.Parser
{
	/// <summary>
	///		Intérprete para los datos de una variación
	/// </summary>
	internal class GameVariationParser
	{
		/// <summary>
		///		Interpreta una variación
		/// </summary>
		internal Board.VariationModel Parse(Game game)
		{
			Board.VariationModel variation = new Board.VariationModel();

				// Carga el setup del tablero
				LoadGameBoard(variation, game);
				// Carga los movimientos
				try
				{
					variation.Movements.AddRange(new GameMovementsParser().Parse(variation, game));
				}
				catch (Exception exception)
				{
					variation.ParseError = $"Error en la interpretación del juego: {exception.Message}";
				}
				// Devuelve la variación
				return variation;
		}

		/// <summary>
		///		Carga el tablero de juego
		/// </summary>
		private void LoadGameBoard(Board.VariationModel variation, Game game)
		{
			if (game.BoardSetup != null)
			{
				// Asigna las propiedades
				variation.Setup.CanBlackCastleKingSide = game.BoardSetup.CanBlackCastleKingSide;
				variation.Setup.CanBlackCastleQueenSide = game.BoardSetup.CanBlackCastleQueenSide;
				variation.Setup.CanWhiteCastleKingSide = game.BoardSetup.CanWhiteCastleKingSide;
				variation.Setup.CanWhiteCastleQueenSide = game.BoardSetup.CanWhiteCastleQueenSide;
				if (game.BoardSetup.EnPassantSquare == null)
					variation.Setup.EnPassantCell = null;
				else
					variation.Setup.EnPassantCell = new CellConversor().ConvertCell(game.BoardSetup.EnPassantSquare.Rank, game.BoardSetup.EnPassantSquare.File);
				variation.Setup.FullMoveCount = game.BoardSetup.FullMoveCount;
				variation.Setup.HalfMoveClock = game.BoardSetup.HalfMoveClock;
				variation.Setup.IsWhiteMove = game.BoardSetup.IsWhiteMove;
				// Interpreta la cadena FEN con las piezas iniciales
				if (game.Tags.TryGetValue("FEN", out string boardPieces))
					variation.Setup.Pieces.AddRange(ParseBoardPieces(boardPieces));
			}
		}

		/// <summary>
		///		Interpreta las piezas del tablero
		/// </summary>
		private List<PieceBaseModel> ParseBoardPieces(string boardPieces)
		{
			List<PieceBaseModel> pieces = new List<PieceBaseModel>();
			int row = 7;

				// Carga las piezas
				if (!string.IsNullOrEmpty(boardPieces))
				{
					string [] rowsParts = boardPieces.Split('\n');

						// Recorre las partes
						foreach (string rowPart in rowsParts)	
							if (!string.IsNullOrWhiteSpace(rowPart) && row >= 0)
							{
								string[] cellParts = rowPart.Trim().Split('|');

									// Si se trata de una fila con datos
									if (cellParts.Length == 10)
									{
										// Asigna las piezas
										for (int column = 0; column < 8; column++)
										{
											string piece = cellParts[column + 1].Trim();

												if (!string.IsNullOrWhiteSpace(piece))
													switch (piece.ToUpper())
													{
														case "P":
																pieces.Add(CreatePiece(PieceBaseModel.PieceType.Pawn, GetColor(piece), row, column));
															break;
														case "R":
																pieces.Add(CreatePiece(PieceBaseModel.PieceType.Rook, GetColor(piece), row, column));
															break;
														case "N":
																pieces.Add(CreatePiece(PieceBaseModel.PieceType.Knight, GetColor(piece), row, column));
															break;
														case "B":
																pieces.Add(CreatePiece(PieceBaseModel.PieceType.Bishop, GetColor(piece), row, column));
															break;
														case "K":
																pieces.Add(CreatePiece(PieceBaseModel.PieceType.King, GetColor(piece), row, column));
															break;
														case "Q":
																pieces.Add(CreatePiece(PieceBaseModel.PieceType.Queen, GetColor(piece), row, column));
															break;
													}
										}
										// Decrementa la fila
										row--;
									}
							}
				}
				// Devuelve la colección de piezas
				return pieces;
		}

		/// <summary>
		///		Obtiene el color asociado a una letra
		/// </summary>
		private PieceBaseModel.PieceColor GetColor(string letter)
		{	
			if (letter == letter.ToUpper())
				return PieceBaseModel.PieceColor.White;
			else
				return PieceBaseModel.PieceColor.Black;
		}

		/// <summary>
		///		Crea una pieza
		/// </summary>
		private PieceBaseModel CreatePiece(PieceBaseModel.PieceType type, PieceBaseModel.PieceColor color, int row, int column)
		{
			switch (type)
			{
				case PieceBaseModel.PieceType.Pawn:
					return new PawnModel(null, color, new Board.CellModel(row, column));
				case PieceBaseModel.PieceType.Rook:
					return new RookModel(null, color, new Board.CellModel(row, column));
				case PieceBaseModel.PieceType.Knight:
					return new KnightModel(null, color, new Board.CellModel(row, column));
				case PieceBaseModel.PieceType.Bishop:
					return new BishopModel(null, color, new Board.CellModel(row, column));
				case PieceBaseModel.PieceType.Queen:
					return new QueenModel(null, color, new Board.CellModel(row, column));
				case PieceBaseModel.PieceType.King:
					return new KingModel(null, color, new Board.CellModel(row, column));
				default:
					return null;
			}
		}
	}
}
