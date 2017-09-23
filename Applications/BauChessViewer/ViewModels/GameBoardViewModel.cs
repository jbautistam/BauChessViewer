using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Bau.Libraries.LibChessGame.Board;
using Bau.Libraries.LibChessGame.Movements;
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

					foreach (MovementBaseModel movement in game.Movements)
						switch (movement)
						{
							case MovementFigureModel move:
									MovementFigureViewModel movementFigure = new MovementFigureViewModel(move, moveIndex);

										// Añade el movimiento tanto a la lista de movimientos general (con comentarios)
										// como a la lista de movimientos de piezas (sin comentarios)
										Movements.Add(movementFigure);
										FigureMovements.Add(movementFigure);
										// Incrementa el índice de movimientos
										moveIndex++;
								break;
							case MovementRemarksModel move:
									Movements.Add(new MovementRemarkViewModel(move));
								break;
							case MovementGameEndModel move:
									Movements.Add(new MovementGameEndViewModel(move));
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
		internal MovementFigureViewModel GetMovement(bool back)
		{
			MovementFigureViewModel movement = null;

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
				//? IsMoving = true y IsMoving = false debe estar dentro de este if para que SelectedMovement = x no
				//? haga una llamada recursiva
				if (movement != null)
				{
					// Indica que ha comenzado a mover
					_isMoving = true;
					// Marca el movimiento seleccionado
					SelectedMovement = movement;
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
				   FigureMovements[_actualMovement].MovementIndex <= movement.MovementIndex)
				Game.TopViewModel.RaiseEventNextMovement();
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
		private List<MovementFigureViewModel> FigureMovements { get; } = new List<MovementFigureViewModel>();

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
			set 
			{ 
				if (CheckObject(ref _selectedMovement, value) && !_isMoving && SelectedMovement is MovementFigureViewModel)
					GoToMovement(SelectedMovement as MovementFigureViewModel);
			}
		}
	}
}
