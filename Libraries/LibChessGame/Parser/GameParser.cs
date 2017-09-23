using System;
using System.Collections.Generic;

using ilf.pgn;
using ilf.pgn.Data;
using Bau.Libraries.LibChessGame.Games;

namespace Bau.Libraries.LibChessGame.Parser
{
	/// <summary>
	///		Intérprete de juegos
	/// </summary>
	internal class GameParser
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
				// Carga la variación
				gameParsed.Variation = new GameVariationParser().Parse(game);
				// Devuelve el juego interpretado
				return gameParsed;
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
