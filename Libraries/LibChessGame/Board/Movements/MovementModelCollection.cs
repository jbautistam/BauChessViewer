using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibChessGame.Board.Movements
{
	/// <summary>
	///		Colección de <see cref="MovementFigureModel"/>
	/// </summary>
	public class MovementModelCollection : List<MovementBaseModel>
	{
		/// <summary>
		///		Busca el último movimiento de una pieza en una colección
		/// </summary>
		internal MovementFigureModel SearchLastPieceMovement()
		{
			// Busca el último movimiento de pieza
			for (int index = Count - 1; index >= 0; index--)
				if (this[index] is MovementFigureModel movement)
					return movement;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return null;
		}
	}
}
