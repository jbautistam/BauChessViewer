using System;

namespace Bau.Libraries.LibChessGame.Movements
{
	/// <summary>
	///		Acción para eliminar una pieza
	/// </summary>
	public class ActionCaptureModel : ActionBaseModel
	{
		public ActionCaptureModel(Pieces.PieceBaseModel.PieceType type, Pieces.PieceBaseModel.PieceColor color,
								  Board.CellModel from) : base(type, color)
		{
			From = from;
		}

		/// <summary>
		///		Celda en la que se encuentra la pieza a eliminar
		/// </summary>
		public Board.CellModel From { get; }
	}
}
