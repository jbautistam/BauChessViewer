using System;

namespace Bau.Libraries.LibChessGame.Board.Pieces
{
	/// <summary>
	///		Reina
	/// </summary>
	public class QueenModel : PieceBaseModel
	{
		public QueenModel(PieceColor color, CellModel cell) : base(color, cell) {}

		/// <summary>
		///		Comprueba si la pieza se puede mover a una fila / columna
		/// </summary>
		public override bool CanMoveTo(GameBoardModel board, CellModel target, Movements.MovementFigureModel.MovementType type)
		{
			// Comprueba si es un movimiento horizontal / diagonal
			if (IsVerticalHorizontalMovement(target, false) || IsDiagonalMovement(target, false))
				return IsLegal(board, target, type);
			// Si ha llegado hasta aquí es porque el movimiento no era legal
			return false;
		}

		/// <summary>
		///		Tipo de pieza
		/// </summary>
		public override PieceType Type 
		{ 
			get { return PieceType.Queen; }
		}
	}
}
