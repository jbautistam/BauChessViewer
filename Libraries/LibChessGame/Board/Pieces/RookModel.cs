using System;

namespace Bau.Libraries.LibChessGame.Board.Pieces
{
	/// <summary>
	///		Torre
	/// </summary>
	public class RookModel : PieceBaseModel
	{
		public RookModel(Board.GameBoardModel board, PieceColor color, Board.CellModel cell) : base(board, color, cell) {}

		/// <summary>
		///		Comprueba si la pieza se puede mover a una fila / columna
		/// </summary>
		public override bool CanMoveTo(Board.GameBoardModel board, Board.CellModel target, Movements.MovementFigureModel.MovementType type)
		{
			// Comprueba si puede mover
			if (IsVerticalHorizontalMovement(target, false))
				return board.IsEmpty(Cell, target) && IsLegal(board, target, type);
			// Devuelve el valor que indica si puede mover
			return false;
		}

		/// <summary>
		///		Tipo de pieza
		/// </summary>
		public override PieceType Type 
		{ 
			get { return PieceType.Rook; }
		}
	}
}
