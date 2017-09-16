using System;
using System.Collections.Generic;

using Bau.Libraries.LibChessGame.Movements;
using Bau.Libraries.LibChessGame.Pieces;

namespace Bau.Libraries.LibChessGame.Board
{
	/// <summary>
	///		Modelo con los datos de un tablero
	/// </summary>
	/// <remarks>
	///		El tablero tiene esta estructura ( -> Blanca, X -> Negra)
	///		       01234567
	///		       --------
	///			8 | X X X X| 0
	///			7 |X X X X | 1
	///			6 | X X X X| 2
	///			5 |X X X X | 3
	///			4 | X X X X| 4
	///			3 |X X X X | 5 
	///			2 | X X X X| 6
	///			1 |X X X X | 7
	///		       --------
	///			   ABCDEFGH
	/// </remarks>
	public class GameBoardModel
	{
		/// <summary>
		///		Vacía el tablero (no deja ninguna pieza)
		/// </summary>
		public void Clear()
		{
			Pieces.Clear();
		}

		/// <summary>
		///		Añade una pieza
		/// </summary>
		public void AddPiece(PieceBaseModel.PieceType piece, PieceBaseModel.PieceColor color, int row, int column)
		{
			AddPiece(piece, color, new CellModel(row, column));
		}

		/// <summary>
		///		Obtiene el texto del tablero
		/// </summary>
		public string GetText()
		{
			string board = "  0 1 2 3 4 5 6 7" + Environment.NewLine + 
						   "  A B C D E F G H" + Environment.NewLine;

				// Obtiene el texto
				for (int row = 0; row < 8; row++)
				{
					// Añade el carácter con la fila
					board += $"{8 - row} ";
					for (int column = 0; column < 8; column++)
					{
						PieceBaseModel piece = GetPiece(new CellModel(row, column));

							if (piece == null)
								board += ".";
							else
								switch (piece.Type)
								{
									case PieceBaseModel.PieceType.Pawn:
											board += piece.Color == PieceBaseModel.PieceColor.White ? "P" : "p";
										break;
									case PieceBaseModel.PieceType.Bishop:
											board += piece.Color == PieceBaseModel.PieceColor.White ? "B" : "b";
										break;
									case PieceBaseModel.PieceType.Knight:
											board += piece.Color == PieceBaseModel.PieceColor.White ? "N" : "n";
										break;
									case PieceBaseModel.PieceType.Rook:
											board += piece.Color == PieceBaseModel.PieceColor.White ? "R" : "r";
										break;
									case PieceBaseModel.PieceType.King:
											board += piece.Color == PieceBaseModel.PieceColor.White ? "K" : "k";
										break;
									case PieceBaseModel.PieceType.Queen:
											board += piece.Color == PieceBaseModel.PieceColor.White ? "Q" : "q";
										break;
								}
							board += " ";
					}
					// Añade el número de fila y un salto de línea
					board += $" {8 - row} {row}" + Environment.NewLine;
				}
				// Añade el número de columna
				board += "  A B C D E F G H" + Environment.NewLine +
						 "  0 1 2 3 4 5 6 7";
				// Devuelve el texto
				return board;
		}

		/// <summary>
		///		Añade una pieza
		/// </summary>
		public void AddPiece(PieceBaseModel.PieceType piece, PieceBaseModel.PieceColor color, CellModel cell)
		{
			switch (piece)
			{
				case PieceBaseModel.PieceType.Pawn:
						AddPiece(new PawnModel(this, color, cell));
					break;
				case PieceBaseModel.PieceType.Rook:
						AddPiece(new RootModel(this, color, cell));
					break;
				case PieceBaseModel.PieceType.Knight:
						AddPiece(new KnightModel(this, color, cell));
					break;
				case PieceBaseModel.PieceType.Bishop:
						AddPiece(new BishopModel(this, color, cell));
					break;
				case PieceBaseModel.PieceType.Queen:
						AddPiece(new QueenModel(this, color, cell));
					break;
				case PieceBaseModel.PieceType.King:
						AddPiece(new KingModel(this, color, cell));
					break;
			}
		}

		/// <summary>
		///		Añade una pieza
		/// </summary>
		public void AddPiece(PieceBaseModel piece)
		{
			Pieces.Add(piece);
		}

		/// <summary>
		///		Inicializa el tablero a la posición inicial
		/// </summary>
		public void Reset()
		{
			// Limpia el tablero
			Clear();
			// Añade las piezas blancas
			AddPiece(PieceBaseModel.PieceType.Rook, PieceBaseModel.PieceColor.White, 7, 0);
			AddPiece(PieceBaseModel.PieceType.Knight, PieceBaseModel.PieceColor.White, 7, 1);
			AddPiece(PieceBaseModel.PieceType.Bishop, PieceBaseModel.PieceColor.White, 7, 2);
			AddPiece(PieceBaseModel.PieceType.Queen, PieceBaseModel.PieceColor.White, 7, 3);
			AddPiece(PieceBaseModel.PieceType.King, PieceBaseModel.PieceColor.White, 7, 4);
			AddPiece(PieceBaseModel.PieceType.Bishop, PieceBaseModel.PieceColor.White, 7, 5);
			AddPiece(PieceBaseModel.PieceType.Knight, PieceBaseModel.PieceColor.White, 7, 6);
			AddPiece(PieceBaseModel.PieceType.Rook, PieceBaseModel.PieceColor.White, 7, 7);
			AddPiece(PieceBaseModel.PieceType.Pawn, PieceBaseModel.PieceColor.White, 6, 0);
			AddPiece(PieceBaseModel.PieceType.Pawn, PieceBaseModel.PieceColor.White, 6, 1);
			AddPiece(PieceBaseModel.PieceType.Pawn, PieceBaseModel.PieceColor.White, 6, 2);
			AddPiece(PieceBaseModel.PieceType.Pawn, PieceBaseModel.PieceColor.White, 6, 3);
			AddPiece(PieceBaseModel.PieceType.Pawn, PieceBaseModel.PieceColor.White, 6, 4);
			AddPiece(PieceBaseModel.PieceType.Pawn, PieceBaseModel.PieceColor.White, 6, 5);
			AddPiece(PieceBaseModel.PieceType.Pawn, PieceBaseModel.PieceColor.White, 6, 6);
			AddPiece(PieceBaseModel.PieceType.Pawn, PieceBaseModel.PieceColor.White, 6, 7);
			// Añade las piezas negras
			AddPiece(PieceBaseModel.PieceType.Rook, PieceBaseModel.PieceColor.Black, 0, 0);
			AddPiece(PieceBaseModel.PieceType.Knight, PieceBaseModel.PieceColor.Black, 0, 1);
			AddPiece(PieceBaseModel.PieceType.Bishop, PieceBaseModel.PieceColor.Black, 0, 2);
			AddPiece(PieceBaseModel.PieceType.Queen, PieceBaseModel.PieceColor.Black, 0, 3);
			AddPiece(PieceBaseModel.PieceType.King, PieceBaseModel.PieceColor.Black, 0, 4);
			AddPiece(PieceBaseModel.PieceType.Bishop, PieceBaseModel.PieceColor.Black, 0, 5);
			AddPiece(PieceBaseModel.PieceType.Knight, PieceBaseModel.PieceColor.Black, 0, 6);
			AddPiece(PieceBaseModel.PieceType.Rook, PieceBaseModel.PieceColor.Black, 0, 7);
			AddPiece(PieceBaseModel.PieceType.Pawn, PieceBaseModel.PieceColor.Black, 1, 0);
			AddPiece(PieceBaseModel.PieceType.Pawn, PieceBaseModel.PieceColor.Black, 1, 1);
			AddPiece(PieceBaseModel.PieceType.Pawn, PieceBaseModel.PieceColor.Black, 1, 2);
			AddPiece(PieceBaseModel.PieceType.Pawn, PieceBaseModel.PieceColor.Black, 1, 3);
			AddPiece(PieceBaseModel.PieceType.Pawn, PieceBaseModel.PieceColor.Black, 1, 4);
			AddPiece(PieceBaseModel.PieceType.Pawn, PieceBaseModel.PieceColor.Black, 1, 5);
			AddPiece(PieceBaseModel.PieceType.Pawn, PieceBaseModel.PieceColor.Black, 1, 6);
			AddPiece(PieceBaseModel.PieceType.Pawn, PieceBaseModel.PieceColor.Black, 1, 7);
		}

		/// <summary>
		///		Obtiene la pieza que está en una fila / columna
		/// </summary>
		public PieceBaseModel GetPiece(CellModel cell)
		{
			// Busca la pieza
			foreach (PieceBaseModel piece in Pieces)
				if (piece.Cell.Row == cell.Row && piece.Cell.Column == cell.Column)
					return piece;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return null;
		}

		/// <summary>
		///		Realiza los movimientos asociados a una serie de acciones
		/// </summary>
		internal void Execute(MovementFigureModel movement)
		{
			System.Diagnostics.Debug.WriteLine($"---->  {movement.Text} - {movement.Color}");
			foreach (ActionBaseModel action in movement.Actions)
				switch (action)
				{
					case ActionMoveModel child:
							Move(child);
						break;
					case ActionCaptureModel child:
							Capture(child);
						break;
					case ActionPromoteModel child:
							Promote(child);
						break;
				}
			System.Diagnostics.Debug.WriteLine(GetText());
			System.Diagnostics.Debug.WriteLine("Piezas: " + Pieces.Count);
		}

		/// <summary>
		///		Busca la pieza de un color que se pueden mover a una celda
		/// </summary>
		internal PieceBaseModel SearchMoveTo(MovementFigureModel movement, CellModel target, CellModel origin)
		{
			List<PieceBaseModel> pieces = new List<PieceBaseModel>();

				// Busca las piezas
				foreach (PieceBaseModel piece in Pieces)
					if (piece.Type == movement.OriginPiece && piece.Color == movement.Color && 
							piece.CanMoveTo(this, target, movement.Type))
						pieces.Add(piece);
				// Si se le ha pasado una fila / columna para ajustarlo, obtiene la pieza que estaba inicialmente en esa posición
				if (pieces.Count > 1)
					if (origin != null && (origin.Row != -1 || origin.Column != -1))
						foreach (PieceBaseModel piece in pieces)
							if ((origin.Column == -1 && piece.Cell.Row == origin.Row) ||
									(origin.Row == -1 && piece.Cell.Column == origin.Column) ||
									(piece.Cell.Row == origin.Row && piece.Cell.Column == origin.Column))
								return piece;
				// Devuelve la primera pieza localizada
				if (pieces.Count > 0)
					return pieces[0];
				else
					return null;
		}

		/// <summary>
		///		Comprueba si puede capturar una pieza
		/// </summary>
		internal bool CanCapture(PieceBaseModel piece, CellModel cell)
		{
			PieceBaseModel target = GetPiece(cell);

				return target != null && target.Color != piece.Color;
		}

		/// <summary>
		///		Comprueba si está vacía el camino entre una fila y una columna
		/// </summary>
		internal bool IsEmpty(CellModel start, CellModel end)
		{
			int verticalSign = -1 * Math.Sign(start.Row - end.Row);
			int horizontalSign = -1 * Math.Sign(start.Column - end.Column);
			int row, column;

				// Comprueba el recorrido
				row = start.Row;
				column = start.Column;
				do
				{
					// Comprueba si la celda está ocupada saltándose la primera celda ...
					if ((row != start.Row || column != start.Column) && !IsEmpty(new CellModel(row, column)))
						return false;
					// Incrementa fila / columna
					row += verticalSign;
					column += horizontalSign;
				}
				while (row != end.Row || column != end.Column);
				// Si ha llegado hasta aquí es porque las celdas intermedias estaban vacías
				return true;
		}

		/// <summary>
		///		Intercambia dos valores
		/// </summary>
		private void Swap(ref int first, ref int second)
		{
			if (second < first)
			{
				int temporal = first;

					first = second;
					second = temporal;
			}
		}

		/// <summary>
		///		Mueve la pieza a una fila / columna
		/// </summary>
		private void Move(ActionMoveModel action)
		{
			PieceBaseModel piece = GetPiece(action.Type, action.Color, action.From);

				// Mueve la pieza
				piece.Cell = action.To;
				piece.IsMoved = true;

				System.Diagnostics.Debug.WriteLine($"Move: {action.Type}-{action.Color} -> {action.From} / {action.To}");
		}

		/// <summary>
		///		Obtiene la pieza de un color que está en una fila / columna
		/// </summary>
		private PieceBaseModel GetPiece(PieceBaseModel.PieceType type, PieceBaseModel.PieceColor color, CellModel cell)
		{
			// Busca la pieza
			foreach (PieceBaseModel piece in Pieces)
				if (piece.Type == type && piece.Color == color && piece.Cell.Row == cell.Row && piece.Cell.Column == cell.Column)
					return piece;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return null;
		}

		/// <summary>
		///		Elimina una pieza
		/// </summary>
		private void Capture(ActionCaptureModel action)
		{
			//? No comprueba Pieces[index].Piece porque en ocasiones me he encontrado
			//? que se ha leído otro tipo de pieza en la captura
			for (int index = Pieces.Count - 1; index >= 0; index--)
				if (Pieces[index].Color == action.Color && 
						Pieces[index].Cell.Row == action.From.Row && Pieces[index].Cell.Column == action.From.Column)
				{
					System.Diagnostics.Debug.WriteLine($"Capture: {Pieces[index].Type}-{Pieces[index].Color} -> {action.From}");
					Pieces.RemoveAt(index);
				}
		}

		/// <summary>
		///		Crea una pieza promocionada
		/// </summary>
		private void Promote(ActionPromoteModel action)
		{
			AddPiece(action.Type, action.Color, action.To.Row, action.To.Column);
			System.Diagnostics.Debug.WriteLine($"Promote: {action.Type}-{action.Color} -> {action.To}");
		}

		/// <summary>
		///		Comprueba si está vacía una celda
		/// </summary>
		internal bool IsEmpty(CellModel cell)
		{
			return GetPiece(cell) == null;
		}

		/// <summary>
		///		Piezas del tablero
		/// </summary>
		public List<PieceBaseModel> Pieces { get; } = new List<PieceBaseModel>();
	}
}
