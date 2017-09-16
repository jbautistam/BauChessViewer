using System;

namespace Bau.Libraries.LibChessGame.Movements
{
	/// <summary>
	///		Datos de un movimiento
	/// </summary>
	public class MovementFigureModel : MovementBaseModel
	{
		/// <summary>
		///		Tipo de movimiento
		/// </summary>
		public enum MovementType
		{
			/// <summary>Normal</summary>
			Normal,
			/// <summary>Captura</summary>
			Capture,
			/// <summary>Captura en passant</summary>
			CaptureEnPassant,
			/// <summary>Enroque corto</summary>
			CastleKingSide,
			/// <summary>Enroque largo</summary>
			CastleQueenSide,
			/// <summary>Pieza promocionada</summary>
			Promote
		}

		/// <summary>
		///		Color
		/// </summary>
		public Pieces.PieceBaseModel.PieceColor Color { get; internal set; }

		/// <summary>
		///		Tipo de movimiento
		/// </summary>
		public MovementType Type { get; internal set; }

		/// <summary>
		///		Texto del movimiento
		/// </summary>
		public string Text { get; internal set; }

		/// <summary>
		///		Pieza movida
		/// </summary>
		public Pieces.PieceBaseModel.PieceType OriginPiece { get; internal set; }

		///// <summary>
		/////		Celda origen
		///// </summary>
		//public Board.CellModel OriginCell { get; } = new Board.CellModel(-1, -1);

		///// <summary>
		/////		Pieza destino
		///// </summary>
		//public Pieces.PieceBaseModel.PieceType? TargetPiece { get; internal set; }

		///// <summary>
		/////		Celda destino
		///// </summary>
		//public Board.CellModel TargetCell { get; } = new Board.CellModel(-1, -1);

		/// <summary>
		///		Comentarios
		/// </summary>
		public string Remarks { get; internal set; }

		/// <summary>
		///		Acciones
		/// </summary>
		public ActionModelCollection Actions { get; } = new ActionModelCollection();
	}
}
