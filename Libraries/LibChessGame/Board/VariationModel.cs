using System;

namespace Bau.Libraries.LibChessGame.Board
{
	/// <summary>
	///		Variación de una partida
	/// </summary>
	public class VariationModel
	{	
		/// <summary>
		///		Configuración inicial del tablero
		/// </summary>
		public BoardSetup Setup { get; } = new BoardSetup();

		/// <summary>
		///		Tablero
		/// </summary>
		public GameBoardModel GameBoard { get; } = new GameBoardModel();

		/// <summary>
		///		Movimientos
		/// </summary>
		public Movements.MovementModelCollection Movements { get; } = new Movements.MovementModelCollection();
	}
}
