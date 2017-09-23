using System;

using Bau.Libraries.LibChessGame.Games;

namespace Bau.Libraries.LibChessGame
{
	/// <summary>
	///		Clase con los datos de un archivo con partidas de ajedrez
	/// </summary>
	public class ChessGameModel
	{
		/// <summary>
		///		Carga los datos de un archivo
		/// </summary>
		public void Load(string fileName)
		{
			Games.Clear();
			Games.AddRange(new Parser.GamesParser().Load(fileName));
		}

		/// <summary>
		///		Juegos
		/// </summary>
		public GameModelCollection Games { get; } = new GameModelCollection();
	}
}
