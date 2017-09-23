using System;

namespace Bau.Libraries.LibChessGame.Board
{
	/// <summary>
	///		Variación de una partida
	/// </summary>
	public class VariationModel
	{	
		/// <summary>
		///		Configuración del tablero
		/// </summary>
		public BoardSetup Board { get; } = new BoardSetup();
	}
}
