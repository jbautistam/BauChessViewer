using System;
using Bau.Libraries.LibChessGame.Board;

namespace Bau.Libraries.LibChessGame.Pieces
{
	/// <summary>
	///		Clase base con los datos de una pieza
	/// </summary>
	public abstract class PieceBaseModel
	{
		/// <summary>
		///		Tipo de pieza
		/// </summary>
		public enum PieceType
		{
			/// <summary>Desconocido. No debería ocurrir</summary>
			Unknown,
			/// <summary>Peón</summary>
			Pawn,
			/// <summary>Torre</summary>
			Rook,
			/// <summary>Alfil</summary>
			Bishop,
			/// <summary>Caballo</summary>
			Knight,
			/// <summary>Rey</summary>
			King,
			/// <summary>Dama</summary>
			Queen
		}

		/// <summary>
		///		Color de la pieza
		/// </summary>
		public enum PieceColor
		{
			/// <summary>Blanca</summary>
			White,
			/// <summary>Negra</summary>
			Black
		}

		public PieceBaseModel(GameBoardModel board, PieceColor color, CellModel cell)
		{
			Board = board;
			Color = color;
			Cell = cell;
			IsMoved = false;
		}

		/// <summary>
		///		Comprueba si la pieza se puede mover a una fila / columna
		/// </summary>
		public abstract bool CanMoveTo(GameBoardModel board, CellModel target, Movements.MovementFigureModel.MovementType type);

		/// <summary>
		///		Comprueba si es legal un movimiento a una posición: si está vacío o si puede capturar
		/// </summary>
		internal bool IsLegal(GameBoardModel board, CellModel target, Movements.MovementFigureModel.MovementType type)
		{
			PieceBaseModel piece = board.GetPiece(target);	

				return piece == null || (type == Movements.MovementFigureModel.MovementType.Capture && piece != null && piece.Color != Color);
		}

		/// <summary>
		///		Comprueba si es un movimiento horizontal
		/// </summary>
		protected bool IsVerticalHorizontalMovement(CellModel target, bool onlyOneCell)
		{
			int rowDifference = Math.Abs(Cell.Row - target.Row);
			int columnDifference = Math.Abs(Cell.Column - target.Column);

				// Comprueba si puede mover
				return CheckOnlyOneCell(onlyOneCell, rowDifference, columnDifference) && 
							((rowDifference == 0 && columnDifference != 0) || (rowDifference != 0 && columnDifference == 0));
		}

		/// <summary>
		///		Comprueba si es un movimiento diagonal
		/// </summary>
		protected bool IsDiagonalMovement(CellModel target, bool onlyOneCell)
		{
			int rowDifference = Math.Abs(Cell.Row - target.Row);
			int columnDifference = Math.Abs(Cell.Column - target.Column);

				// Comprueba si puede mover
				return CheckOnlyOneCell(onlyOneCell, rowDifference, columnDifference) && (rowDifference != 0 && rowDifference == columnDifference);
		}

		/// <summary>
		///		Comprueba que sólo se ha movido una celda
		/// </summary>
		private bool CheckOnlyOneCell(bool onlyOneCell, int rowDiference, int columnDifference)
		{
			return !onlyOneCell || (onlyOneCell && Math.Max(rowDiference, columnDifference) == 1);
		}

		/// <summary>
		///		Tablero donde está la pieza
		/// </summary>
		public GameBoardModel Board { get; }

		/// <summary>
		///		Color de la pieza
		/// </summary>
		public PieceColor Color { get; }

		/// <summary>
		///		Indica si se ha movido
		/// </summary>
		public bool IsMoved { get; set; }

		/// <summary>
		///		Celda en la que se encuentra la pieza
		/// </summary>
		public CellModel Cell { get; set; }

		/// <summary>
		///		Tipo de pieza
		/// </summary>
		public abstract PieceType Type { get; }
	}
}
