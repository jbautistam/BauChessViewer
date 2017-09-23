using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibChessGame.Board.Pieces
{
	/// <summary>
	///		Colección de <see cref="PieceBaseModel"/>
	/// </summary>
	public class PieceBaseModelCollection : List<PieceBaseModel>
	{
		/// <summary>
		///		Añade una pieza
		/// </summary>
		public void Add(PieceBaseModel.PieceType piece, PieceBaseModel.PieceColor color, int row, int column)
		{
			Add(piece, color, new CellModel(row, column));
		}

		/// <summary>
		///		Añade una pieza
		/// </summary>
		public void Add(PieceBaseModel.PieceType piece, PieceBaseModel.PieceColor color, CellModel cell)
		{
			switch (piece)
			{
				case PieceBaseModel.PieceType.Pawn:
						Add(new PawnModel(color, cell));
					break;
				case PieceBaseModel.PieceType.Rook:
						Add(new RookModel(color, cell));
					break;
				case PieceBaseModel.PieceType.Knight:
						Add(new KnightModel(color, cell));
					break;
				case PieceBaseModel.PieceType.Bishop:
						Add(new BishopModel(color, cell));
					break;
				case PieceBaseModel.PieceType.Queen:
						Add(new QueenModel(color, cell));
					break;
				case PieceBaseModel.PieceType.King:
						Add(new KingModel(color, cell));
					break;
			}
		}

		/// <summary>
		///		Clona las piezas
		/// </summary>
		public PieceBaseModelCollection Clone()
		{
			PieceBaseModelCollection pieces = new PieceBaseModelCollection();

				// Clona las piezas
				foreach (PieceBaseModel piece in this)
					pieces.Add(piece.Type, piece.Color, piece.Cell.Row, piece.Cell.Column);
				// Devuelve la colección
				return pieces;
		}

		/// <summary>
		///		Obtiene la pieza que está en una fila / columna
		/// </summary>
		public PieceBaseModel GetPiece(CellModel cell)
		{
			// Busca la pieza
			foreach (PieceBaseModel piece in this)
				if (piece.Cell.Row == cell.Row && piece.Cell.Column == cell.Column)
					return piece;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return null;
		}

		/// <summary>
		///		Comprueba si está vacía una celda
		/// </summary>
		public bool IsEmpty(CellModel cell)
		{
			return GetPiece(cell) == null;
		}
	}
}
