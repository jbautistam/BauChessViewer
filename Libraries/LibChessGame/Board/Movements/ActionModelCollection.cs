﻿using System;
using System.Collections.Generic;

using Bau.Libraries.LibChessGame.Board.Pieces;

namespace Bau.Libraries.LibChessGame.Board.Movements
{
	/// <summary>
	///		Colección de <see cref="ActionBaseModel"/>
	/// </summary>
	public class ActionModelCollection : List<ActionBaseModel>
	{
		/// <summary>
		///		Crea las acciones de enroque
		/// </summary>
		internal void CreateCastleKingActions(GameBoardModel board, MovementFigureModel movement)
		{
			int row = movement.Color == PieceBaseModel.PieceColor.White ? 7 : 0;

				if (movement.Type == MovementFigureModel.MovementType.CastleKingSide) // enroque corto
				{
					CreateMoveAction(PieceBaseModel.PieceType.Rook, movement.Color, new CellModel(row, 7), new CellModel(row, 5));
					CreateMoveAction(PieceBaseModel.PieceType.King, movement.Color, new CellModel(row, 4), new CellModel(row, 6));
				}
				else // enroque largo
				{
					CreateMoveAction(PieceBaseModel.PieceType.Rook, movement.Color, new CellModel(row, 0), new CellModel(row, 3));
					CreateMoveAction(PieceBaseModel.PieceType.King, movement.Color, new CellModel(row, 4), new CellModel(row, 2));
				}
		}

		/// <summary>
		///		Crea las acciones asociadas a un movimiento
		/// </summary>
		internal void CreateActions(GameBoardModel board, MovementFigureModel movement, 
									CellModel target, CellModel origin, PieceBaseModel.PieceType? targetPiece)
		{
			PieceBaseModel piece = board.SearchMoveTo(movement, target, origin);

				// Comprueba que se haya localizado la pieza que hizo el movimiento
				if (piece == null)
					throw new Exceptions.GameReaderException($"No se encuentra ninguna pieza que pueda realizar el movimiento {movement.Text}");
				// Crea el movimiento
				CreateMoveAction(piece.Type, piece.Color, piece.Cell, target);
				// Crea la captura
				if (movement.Type == MovementFigureModel.MovementType.Capture || movement.Type == MovementFigureModel.MovementType.CaptureEnPassant)
					CreateCaptureAction(board, targetPiece, GetNextColor(movement.Color), target);
				// Crea la promoción
				if (movement.PromotedPiece != null)
				{
					// Crea un movimiento para eliminar el peón que se ha promocionado
					//? No llama a CreateCaptureAction porque si ha habido una captura en este mismo movimiento
					//? la pieza aún no se habrá eliminado del destino y por tanto nos creará una captura
					//? sobre la pieza que se ha eliminado ya
					Add(new ActionCaptureModel(piece.Type, movement.Color, target));
					// Crea una promoción
					CreatePromoteAction(movement.PromotedPiece, movement.Color, target);
				}
		}

		/// <summary>
		///		Obtiene el siguiente color
		/// </summary>
		private PieceBaseModel.PieceColor GetNextColor(PieceBaseModel.PieceColor color)
		{
			return color == PieceBaseModel.PieceColor.White ? PieceBaseModel.PieceColor.Black : PieceBaseModel.PieceColor.White;
		}

		/// <summary>
		///		Crea un movimiento
		/// </summary>
		private void CreateMoveAction(PieceBaseModel.PieceType type, PieceBaseModel.PieceColor color, CellModel from, CellModel to)
		{
			Add(new ActionMoveModel(type, color, from, to));
		}

		/// <summary>
		///		Crea una captura
		/// </summary>
		private void CreateCaptureAction(GameBoardModel board, PieceBaseModel.PieceType? piece, PieceBaseModel.PieceColor color, CellModel from)
		{
			PieceBaseModel targetPiece = board.Pieces.GetPiece(from);

				//? En ocasiones la pieza leída no es la real del tablero, por eso buscamos la pieza adecuada
				// Añade la pieza
				Add(new ActionCaptureModel(targetPiece?.Type ?? piece ?? PieceBaseModel.PieceType.Unknown, color, from));
		}

		/// <summary>
		///		Crea un promoción
		/// </summary>
		private void CreatePromoteAction(PieceBaseModel.PieceType? piece, PieceBaseModel.PieceColor color, CellModel to)
		{
			if (piece != null)
				Add(new ActionPromoteModel(piece ?? PieceBaseModel.PieceType.Unknown, color, to));
		}
	}
}