using System;

namespace Bau.Libraries.LibChessGame.Board.Pieces
{
	/// <summary>
	///		Caballo
	/// </summary>
	public class KnightModel : PieceBaseModel
	{
		public KnightModel(PieceColor color, Board.CellModel cell) : base(color, cell) {}

		/// <summary>
		///		Comprueba si la pieza se puede mover a una fila / columna
		/// </summary>
		public override bool CanMoveTo(Board.GameBoardModel board, Board.CellModel target, Movements.MovementFigureModel.MovementType type)
		{
			int rowDifference = Math.Abs(Cell.Row - target.Row);
			int columnDifference = Math.Abs(Cell.Column - target.Column);
				
				// Puede moverse si se desplaza dos celdas en una dirección y una en otra
				if ((rowDifference == 2 && columnDifference == 1) ||
						(rowDifference == 1 && columnDifference == 2))
					return IsLegal(board, target, type);
				// Devuelve el valor que indica que no se puede
				return false;
		}

		/// <summary>
		///		Tipo de pieza
		/// </summary>
		public override PieceType Type 
		{ 
			get { return PieceType.Knight; }
		}
	}
}
