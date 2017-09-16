using System;

namespace Bau.Libraries.LibChessGame.Pieces
{
	/// <summary>
	///		Rey
	/// </summary>
	public class KingModel : PieceBaseModel
	{
		public KingModel(Board.GameBoardModel board, PieceColor color, Board.CellModel cell) : base(board, color, cell) {}

		/// <summary>
		///		Comprueba si la pieza se puede mover a una fila / columna
		/// </summary>
		public override bool CanMoveTo(Board.GameBoardModel board, Board.CellModel target, Movements.MovementFigureModel.MovementType type)
		{
			// Comprueba si es un movimiento horizontal / diagonal de un solo recuadro
			if (IsVerticalHorizontalMovement(target, true) || IsDiagonalMovement(target, true))
				return IsLegal(board, target, type);
			// Si ha llegado hasta aquí es porque el movimiento no era legal
			return false;
		}

		/// <summary>
		///		Tipo de pieza
		/// </summary>
		public override PieceType Type 
		{ 
			get { return PieceType.King; }
		}
	}
}
