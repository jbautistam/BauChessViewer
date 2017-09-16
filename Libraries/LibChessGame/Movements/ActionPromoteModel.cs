using System;

namespace Bau.Libraries.LibChessGame.Movements
{
	/// <summary>
	///		Acción de promocionar una pieza
	/// </summary>
	public class ActionPromoteModel : ActionBaseModel
	{
		public ActionPromoteModel(Pieces.PieceBaseModel.PieceType type, Pieces.PieceBaseModel.PieceColor color,
								  Board.CellModel to) : base(type, color)
		{
			To = to;
		}

		/// <summary>
		///		Lugar donde se coloca la pieza promocionada
		/// </summary>
		public Board.CellModel To { get; }
	}
}
