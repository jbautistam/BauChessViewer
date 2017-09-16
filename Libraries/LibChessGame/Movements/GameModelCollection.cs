using System;
using System.Collections.Generic;

using ilf.pgn;
using ilf.pgn.Data;

namespace Bau.Libraries.LibChessGame.Movements
{
	/// <summary>
	///		Colección de <see cref="GameModel"/>
	/// </summary>
	public class GameModelCollection : List<GameModel>
	{
		/// <summary>
		///		Carga un archivo
		/// </summary>
		internal void Load(string fileName)
		{
			Database gameDb = new PgnReader().ReadFromFile(fileName);

				// Inicializa los juegos
				foreach (Game game in gameDb.Games)
					Add(game);
		}

		/// <summary>
		///		Añade los datos del juego
		/// </summary>
		private void Add(Game game)
		{
			GameModel newGame = new GameModel();

				// Inicializa las propiedades básicas
				newGame.Event = game.Event;
				newGame.Round = game.Round;
				newGame.Site = game.Site;
				newGame.WhitePlayer = game.WhitePlayer;
				newGame.BlackPlayer = game.BlackPlayer;
				// newGame.Board = game.BoardSetup;
				newGame.Result = ConvertResult(game.Result);
				newGame.Year = game.Year;
				newGame.Month = game.Month;
				newGame.Day = game.Day;
				// Añade la información adicional
				foreach (GameInfo info in game.AdditionalInfo)
					newGame.AdditionalInfo.Add(new KeyValuePair<string, string>(info.Name, info.Value));
				// Añade las etiquetas
				foreach (KeyValuePair<string, string> tag in game.Tags)
					newGame.Tags.Add(new KeyValuePair<string, string>(tag.Key, tag.Value));
				//TODO --> Queda inicializar el tablero con BoardSetup
				// Carga los movimientos
				try
				{
					newGame.Movements.Load(game);
				}
				catch (Exception exception)
				{
					newGame.ParseError = $"Error en la interpretación del juego: {exception.Message}";
				}
				// Añade el juego
				Add(newGame);
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
