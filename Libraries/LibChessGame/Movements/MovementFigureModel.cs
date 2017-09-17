using System;
using ilf.pgn.Data;

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
			CastleQueenSide
		}

		/// <summary>
		///		Anotaciones del movimiento
		/// </summary>
		public enum AnnotationType
		{
			MindBlowing,
			Brilliant,
			Good,
			Interesting,
			Dubious,
			Mistake,
			Blunder,
			Abysmal,
			FascinatingButUnsound,
			Unclear,
			WithCompensation,
			EvenPosition,
			SlightAdvantageWhite,
			SlightAdvantageBlack,
			AdvantageWhite,
			AdvantageBlack,
			DecisiveAdvantageWhite,
			DecisiveAdvantageBlack,
			Space,
			Initiative,
			Development,
			Counterplay,
			Countering,
			Idea,
			TheoreticalNovelty,
			UnknownAnnotation
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

		/// <summary>
		///		Pieza promocionada
		/// </summary>
		public Pieces.PieceBaseModel.PieceType? PromotedPiece { get; internal set; }

		/// <summary>
		///		Jaque
		/// </summary>
		public bool IsCheck { get; internal set; }

		/// <summary>
		///		Jaque doble
		/// </summary>
		public bool IsDoubleCheck { get; internal set; }

		/// <summary>
		///		Jaque mate
		/// </summary>
		public bool IsCheckMate { get; internal set; }

		/// <summary>
		///		Comentarios
		/// </summary>
		public string Remarks { get; internal set; }

		/// <summary>
		///		Anotación del movimiento
		/// </summary>
		public AnnotationType? Annotation { get; set; }

		/// <summary>
		///		Acciones
		/// </summary>
		public ActionModelCollection Actions { get; } = new ActionModelCollection();
	}
}
