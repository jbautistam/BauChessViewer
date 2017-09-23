using System;
using System.Collections.Generic;

using ilf.pgn;
using ilf.pgn.Data;
using Bau.Libraries.LibChessGame.Movements;
using Bau.Libraries.LibChessGame.Pieces;

namespace Bau.Libraries.LibChessGame.Parser
{
	/// <summary>
	///		Intérprete de juegos
	/// </summary>
	internal class GamesParser
	{
		/// <summary>
		///		Carga los juegos de un archivo
		/// </summary>
		internal GameModelCollection Load(string fileName)
		{
			GameModelCollection games = new GameModelCollection();
			Database gameDb = new PgnReader().ReadFromFile(fileName);

				// Inicializa los juegos
				foreach (Game game in gameDb.Games)
					games.Add(Parse(game));
				// Devuelve los juegos cargados
				return games;
		}

		/// <summary>
		///		Añade los datos del juego
		/// </summary>
		private GameModel Parse(Game game)
		{
			GameModel gameParsed = new GameModel();
			GameMovementsParser movementsParser = new GameMovementsParser();

				// Inicializa las propiedades básicas
				gameParsed.Event = game.Event;
				gameParsed.Round = game.Round;
				gameParsed.Site = game.Site;
				gameParsed.WhitePlayer = game.WhitePlayer;
				gameParsed.BlackPlayer = game.BlackPlayer;
				// newGame.Board = game.BoardSetup;
				gameParsed.Result = ConvertResult(game.Result);
				gameParsed.Year = game.Year;
				gameParsed.Month = game.Month;
				gameParsed.Day = game.Day;
				// Añade la información adicional
				foreach (GameInfo info in game.AdditionalInfo)
					gameParsed.AdditionalInfo.Add(new KeyValuePair<string, string>(info.Name, info.Value));
				// Añade las etiquetas
				foreach (KeyValuePair<string, string> tag in game.Tags)
					gameParsed.Tags.Add(new KeyValuePair<string, string>(tag.Key, tag.Value));
				// Carga el setup del tablero
				LoadGameBoard(gameParsed, game, movementsParser);
				// Carga los movimientos
				try
				{
					gameParsed.Movements.AddRange(movementsParser.Parse(gameParsed, game));
				}
				catch (Exception exception)
				{
					gameParsed.ParseError = $"Error en la interpretación del juego: {exception.Message}";
				}
				// Devuelve el juego interpretado
				return gameParsed;
		}

		/// <summary>
		///		Carga el tablero de juego
		/// </summary>
		private void LoadGameBoard(GameModel gameParsed, Game game, GameMovementsParser movementsParser)
		{
			if (game.BoardSetup != null)
			{
				// Asigna las propiedades
				gameParsed.Board.CanBlackCastleKingSide = game.BoardSetup.CanBlackCastleKingSide;
				gameParsed.Board.CanBlackCastleQueenSide = game.BoardSetup.CanBlackCastleQueenSide;
				gameParsed.Board.CanWhiteCastleKingSide = game.BoardSetup.CanWhiteCastleKingSide;
				gameParsed.Board.CanWhiteCastleQueenSide = game.BoardSetup.CanWhiteCastleQueenSide;
				if (game.BoardSetup.EnPassantSquare == null)
					gameParsed.Board.EnPassantCell = null;
				else
					gameParsed.Board.EnPassantCell = movementsParser.ConvertCell(game.BoardSetup.EnPassantSquare.Rank, game.BoardSetup.EnPassantSquare.File);
				gameParsed.Board.FullMoveCount = game.BoardSetup.FullMoveCount;
				gameParsed.Board.HalfMoveClock = game.BoardSetup.HalfMoveClock;
				gameParsed.Board.IsWhiteMove = game.BoardSetup.IsWhiteMove;
				// Interpreta la cadena FEN con las piezas iniciales
				if (game.Tags.TryGetValue("FEN", out string boardPieces))
					gameParsed.Board.Pieces.AddRange(ParseBoardPieces(boardPieces));
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

		/// <summary>
		///		Convierte el resultado
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

	}
}
