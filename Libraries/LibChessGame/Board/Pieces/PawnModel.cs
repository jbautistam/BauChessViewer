using System;

namespace Bau.Libraries.LibChessGame.Board.Pieces
{
	/// <summary>
	///		Peón
	/// </summary>
	public class PawnModel : PieceBaseModel
	{
		public PawnModel(Board.GameBoardModel board, PieceColor color, Board.CellModel cell) : base(board, color, cell) {}

		/// <summary>
		///		Comprueba si la pieza se puede mover a una fila / columna
		/// </summary>
		public override bool CanMoveTo(Board.GameBoardModel board, Board.CellModel target, Movements.MovementFigureModel.MovementType type)
		{
			int rowDiference = Cell.Row - target.Row;
			int columnDiference = Cell.Column - target.Column;
			bool can = false;

				// Comprueba si se puede mover
				if (Math.Abs(columnDiference) <= 1) // sólo se puede mover como máximo una columna a la izquierda
				{
					// Las blancas sólo se pueden mover hacia arriba y las negras sólo se pueden mover hacia abajo
					if ((Math.Sign(rowDiference) == 1 && Color == PieceColor.White) ||
							(Math.Sign(rowDiference) == -1 && Color == PieceColor.Black))
						{
							// Se puede mover dos filas: si no se ha movido y sólo ha ido en la misma columna o es una captura in Passant
							if (Math.Abs(rowDiference) == 2 && !IsMoved)
							{
								if (columnDiference == 0) // ... movimiento normal
									can = board.IsEmpty(Cell, target); // ... si están vacías las celdas entre una y otra
							}
							else if (Math.Abs(rowDiference) == 1)
							{
								if (columnDiference == 0)
									can = board.IsEmpty(target);
								else
									can = type == Movements.MovementFigureModel.MovementType.Capture && board.CanCapture(this, target);
							}
						}
					}
				// Devuelve el valor que indica si se puede mover
				return can;
		}

		/// <summary>
		///		Tipo de pieza
		/// </summary>
		public override PieceType Type 
		{ 
			get { return PieceType.Pawn; }
		}
	}
}
