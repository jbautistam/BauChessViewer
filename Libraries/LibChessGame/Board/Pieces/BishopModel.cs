﻿using System;

namespace Bau.Libraries.LibChessGame.Board.Pieces
{
	/// <summary>
	///		Alfil
	/// </summary>
	public class BishopModel : PieceBaseModel
	{
		public BishopModel(PieceColor color, CellModel cell) : base(color, cell) {}

		/// <summary>
		///		Comprueba si la pieza se puede mover a una fila / columna
		/// </summary>
		public override bool CanMoveTo(GameBoardModel board, CellModel target, Movements.MovementFigureModel.MovementType type)
		{
			// Comprueba si puede mover
			if (IsDiagonalMovement(target, false))
				return board.IsEmpty(Cell, target) && IsLegal(board, target, type);
			// Devuelve el valor que indica si puede mover
			return false;
		}

		/// <summary>
		///		Tipo de pieza
		/// </summary>
		public override PieceType Type 
		{ 
			get { return PieceType.Bishop; }
		}
	}
}
