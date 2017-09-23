﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Bau.Libraries.LibChessGame.Board;
using Bau.Libraries.LibChessGame.Board.Movements;
using Bau.Libraries.LibChessGame.Games;
using BauChessViewer.ViewModels.Movements;

namespace BauChessViewer.ViewModels
{
	/// <summary>
	///		ViewModel del tablero de juego con la partida
	/// </summary>
	public class GameBoardViewModel : BaseViewModel
	{
		// Variables privadas
		private BaseMovementViewModel _selectedMovement;
		private int _actualMovement;
		private bool _isMoving = false;

		public GameBoardViewModel(GameViewModel game)
		{
			Game = game;
		}

		/// <summary>
		///		Inicializa el tablero
		/// </summary>
		public void Reset()
		{
			// Inicializa el tablero
			GameBoard.Reset(Game.Game);
			// Recoge los movimientos (sólo los de figuras)
			LoadMovements(Game.Game);
			// Inicializa el movimiento actual
			_actualMovement = 0;
			// Lanza el evento para inicializar el tablero
			Game.TopViewModel.RaiseEventReset();
		}

		/// <summary>
		///		Carga los movimientos de un juego
		/// </summary>
		private void LoadMovements(GameModel game)
		{
			// Limpia los movimientos
			Movements.Clear();
			FigureMovements.Clear();
			// Carga los movimientos
			if (game != null)
			{
				int moveIndex = 1;
				MovementFigureDoubleViewModel lastMovement = null;

					foreach (MovementBaseModel movement in game.Variation.Movements)
						switch (movement)
						{
							case MovementFigureModel move:
									MovementFigureViewModel movementFigure = new MovementFigureViewModel(this, move, moveIndex);

										// Añade el movimiento tanto a la lista de movimientos general (con comentarios)
										// como a la lista de movimientos de piezas (sin comentarios)
										FigureMovements.Add(move);
										// Añade el movimiento al movimiento de figuras doble
										if (lastMovement == null)
										{
											// Crea el
											lastMovement = new MovementFigureDoubleViewModel(moveIndex);
											Movements.Add(lastMovement);
										}
										// Asigna el movimiento de blancas / negras
										if (move.Color == Bau.Libraries.LibChessGame.Board.Pieces.PieceBaseModel.PieceColor.White)
											lastMovement.WhiteMovement = movementFigure;
										else // Asigna el movimiento de negras
										{	
											lastMovement.BlackMovement = movementFigure;
											lastMovement = null;
										}
										// Incrementa el índice de movimientos
										moveIndex++;
								break;
							case MovementRemarksModel move:
									Movements.Add(new MovementRemarkViewModel(move));
									lastMovement = null;
								break;
							case MovementGameEndModel move:
									Movements.Add(new MovementGameEndViewModel(move));
									lastMovement = null;
								break;
						}
			}
			// Añade un movimiento si no había ninguno
			if (Movements.Count == 0)
				Movements.Add(new MovementRemarkViewModel(new MovementRemarksModel("No hay ningún movimiento en este juego")));
		}

		/// <summary>
		///		Obtiene el siguiente movimiento (hacia atrás o hacia delante)
		/// </summary>
		internal MovementFigureModel GetMovement(bool back)
		{
			MovementFigureModel movement = null;

				// Obtiene el movimiento
				if (back)
				{
					if (_actualMovement > 0)
						movement = FigureMovements[--_actualMovement];
				}
				else
				{
					if (_actualMovement < FigureMovements.Count)
					{
						movement = FigureMovements[_actualMovement];
						_actualMovement++;
					}
				}
				// Selecciona el movimiento
				//? IsMoving = true y IsMoving = false debe estar dentro de este if para que 'SelectedMovement = x' no
				//? haga una llamada recursiva
				if (movement != null)
				{
					// Indica que ha comenzado a mover
					_isMoving = true;
					// Marca el movimiento seleccionado
					SelectMovement(_actualMovement);
					//SelectedMovement = movement;
					// Indica que se ha dejado de mover
					_isMoving = false;
				}
				// Devuelve el movimiento
				return movement;
		}

		/// <summary>
		///		Coloca la partida en un movimiento
		/// </summary>
		private void GoToMovement(MovementFigureViewModel movement)
		{
			// Limpia el tablero
			Reset();
			// Busca el movimiento
			while (_actualMovement >= 0 && _actualMovement < FigureMovements.Count &&
				   _actualMovement < movement.MovementIndex)
				Game.TopViewModel.RaiseEventNextMovement();
		}

		/// <summary>
		///		Reorre los movimientos hasta encontrar el buscado
		/// </summary>
		internal void MoveTo(MovementFigureViewModel movementFigureViewModel)
		{
			GoToMovement(movementFigureViewModel);
			SelectMovement(movementFigureViewModel.MovementIndex);
		}

		/// <summary>
		///		Selecciona el movimiento y deselecciona los demás
		/// </summary>
		private void SelectMovement(int index)
		{
			foreach (BaseMovementViewModel baseMovement in Movements)
			{
				MovementFigureDoubleViewModel movement = baseMovement as MovementFigureDoubleViewModel;

					if (movement != null)
					{
						if (movement.WhiteMovement != null)
						{
							movement.WhiteMovement.Selected = movement.WhiteMovement.MovementIndex == index;
							if (movement.WhiteMovement.Selected)
								SelectedMovement = movement;
						}
						if (movement.BlackMovement != null)
						{
							movement.BlackMovement.Selected = movement.BlackMovement.MovementIndex == index;
							if (movement.BlackMovement.Selected)
								SelectedMovement = movement;
						}
					}		
			}
		}

		/// <summary>
		///		Juego
		/// </summary>
		private GameViewModel Game { get; }

		/// <summary>
		///		Tablero de juego
		/// </summary>
		public GameBoardModel GameBoard { get; private set; } = new GameBoardModel();

		/// <summary>
		///		Lista de movimientos interna: sólo con los movimientos de piezas, sin comentarios
		/// </summary>
		private List<MovementFigureModel> FigureMovements { get; } = new List<MovementFigureModel>();

		/// <summary>
		///		Movimientos para mostrar en la vista: combina movimientos con comentarios y resultados
		/// </summary>
		public ObservableCollection<BaseMovementViewModel> Movements { get; } = new ObservableCollection<BaseMovementViewModel>();

		/// <summary>
		///		Movimiento seleccionado
		/// </summary>
		public BaseMovementViewModel SelectedMovement
		{
			get { return _selectedMovement; }
			set { CheckObject(ref _selectedMovement, value); }
		}	
	}
}
