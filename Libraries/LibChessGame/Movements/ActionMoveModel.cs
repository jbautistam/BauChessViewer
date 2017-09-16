using System;

namespace Bau.Libraries.LibChessGame.Movements
{
	/// <summary>
	///		Movimiento de una pieza
	/// </summary>
	public class ActionMoveModel : ActionBaseModel
	{
		public ActionMoveModel(Pieces.PieceBaseModel.PieceType type, Pieces.PieceBaseModel.PieceColor color,
							   Board.CellModel from, Board.CellModel to) : base(type, color)
		{
			From = from;
			To = to;
		}

		/// <summary>
		///		Celda origen
		/// </summary>
		public Board.CellModel From { get; }

		/// <summary>
		///		Celda destino
		/// </summary>
		public Board.CellModel To { get; }
	}
}
