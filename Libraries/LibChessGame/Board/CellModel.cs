using System;

namespace Bau.Libraries.LibChessGame.Board
{
	/// <summary>
	///		Clase con los datos de una celda
	/// </summary>
	public class CellModel
	{
		public CellModel(int row, int column)
		{
			Row = row;
			Column = column;
		}

		/// <summary>
		///		Fila
		/// </summary>
		public int Row { get; set; }

		/// <summary>
		///		Columna
		/// </summary>
		public int Column { get; set; }

		/// <summary>
		///		Sobrescribe el método ToString
		/// </summary>
		public override string ToString()
		{
			return $"{Row}.{Column}";
		}
	}
}
