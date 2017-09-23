using System;

using ilf.pgn.Data;
using Bau.Libraries.LibChessGame.Board;

namespace Bau.Libraries.LibChessGame.Parser
{
	/// <summary>
	///		Conversor de datos de celdas
	/// </summary>
	internal class CellConversor
	{
		/// <summary>
		///		Convierte una celda
		/// </summary>
		internal CellModel ConvertCell(int? row, File? file)
		{
			return new CellModel(ConvertRow(row), ConvertColumn(file));
		}

		/// <summary>
		///		Convierte una columna
		/// </summary>
		private int ConvertColumn(File? file)
		{
			if (file == null)
				return -1;
			else
				switch (file)
				{
					case File.A:
						return 0;
					case File.B:
						return 1;
					case File.C:
						return 2;
					case File.D:
						return 3;
					case File.E:
						return 4;
					case File.F:
						return 5;
					case File.G:
						return 6;
					case File.H:
						return 7;
					default:
						throw new NotImplementedException();
				}
		}

		/// <summary>
		///		Convierte una fila
		/// </summary>
		private int ConvertRow(int? row)
		{
			if (row == null)
				return -1;
			else
				return 8 - (row ?? 0);
		}
	}
}
