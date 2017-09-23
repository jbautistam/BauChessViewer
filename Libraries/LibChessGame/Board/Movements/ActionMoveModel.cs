using System;

namespace Bau.Libraries.LibChessGame.Board.Movements
{
	/// <summary>
	///		Movimiento de una pieza
	/// </summary>
	public class ActionMoveModel : ActionBaseModel
	{
		public ActionMoveModel(Pieces.PieceBaseModel.PieceType type, Pieces.PieceBaseModel.PieceColor color,
							   CellModel from, CellModel to) : base(type, color)
		{
			From = from;
			To = to;
		}

		/// <summary>
		///		Celda origen
		/// </summary>
		public CellModel From { get; }

		/// <summary>
		///		Celda destino
		/// </summary>
		public CellModel To { get; }
	}
}
