using System;

namespace Bau.Libraries.LibChessGame.Board
{
	/// <summary>
	///		Variación de una partida
	/// </summary>
	public class VariationModel
	{	
		public VariationModel()
		{
			GameBoard = new GameBoardModel(this);
		}

		/// <summary>
		///		Configuración inicial del tablero
		/// </summary>
		public BoardSetup Setup { get; } = new BoardSetup();

		/// <summary>
		///		Tablero
		/// </summary>
		public GameBoardModel GameBoard { get; }

		/// <summary>
		///		Movimientos
		/// </summary>
		public Movements.MovementModelCollection Movements { get; } = new Movements.MovementModelCollection();

		/// <summary>
		///		Error de interpretación
		/// </summary>
		public string ParseError { get; internal set; }
	}
}
