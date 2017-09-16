using System;

namespace Bau.Libraries.LibChessGame.Movements
{
	/// <summary>
	///		Clase con los datos de una acción
	/// </summary>
	public abstract class ActionBaseModel
	{
		public ActionBaseModel(Pieces.PieceBaseModel.PieceType type, Pieces.PieceBaseModel.PieceColor color)
		{
			Type = type;
			Color = color;
		}

		/// <summary>
		///		Tipo de pieza
		/// </summary>
		public Pieces.PieceBaseModel.PieceType Type { get; }

		/// <summary>
		///		Color de la pieza
		/// </summary>
		public Pieces.PieceBaseModel.PieceColor Color { get; }
	}
}
