using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Bau.Libraries.LibChessGame.Board;
using Bau.Libraries.LibChessGame.Board.Movements;

namespace BauChessViewer.ViewModels.Movements
{
	/// <summary>
	///		Lista de movimientos
	/// </summary>
	public class MovementListViewModel : BaseViewModel
	{
		// Variables privadas
		private BaseMovementViewModel _selectedMovement;
		private MovementFigureViewModel _actualMovement;
		private int _actualMovementIndex = 0;

		public MovementListViewModel(PgnGameViewModel pgnGameViewModel)
		{
			PgnGameViewModel = pgnGameViewModel;
		}

		/// <summary>
		///		Carga los movimientos de la variación
		/// </summary>
		internal void LoadMovements(VariationModel variation)
		{
			// Limpia los movimientos
			Movements.Clear();
			FigureMovements.Clear();
			// Inicializa el tablero
			GameBoard = new GameBoardModel(variation);
			GameBoard.Reset();
			// Carga los movimientos
			if (variation != null)
				LoadVariationMovements(variation, 1, 1, 0);
			// Añade un comentario si no había ningún movimiento
			if (Movements.Count == 0)
				Movements.Add(new MovementRemarkViewModel(new MovementRemarksModel("No hay ningún movimiento en este juego")));
			// Inicializa los movimientos
			_actualMovementIndex = 0;
		}

		/// <summary>
		///		Carga en la lista una variación de movimientos
		/// </summary>
		private void LoadVariationMovements(VariationModel variation, int movementIndex, int movementLineIndex, int variationIndex)
		{
			MovementFigureDoubleViewModel lastMovement = null;
			MovementFigureVariationDoubleViewModel lastRecursiveMovement = null;

				foreach (MovementBaseModel movement in variation.Movements)
					switch (movement)
					{
						case MovementFigureModel move:
								// Añade a la lista un movimiento real o un movimiento recursivo
								if (variationIndex == 0)
								{
									// Añade el movimiento a la lista de movimientos de piezas (sin comentarios)
									FigureMovements.Add(move);
									// Crea el movimiento para la lista
									lastMovement = CreateRealMovement(lastMovement, move, movementIndex, movementLineIndex);
									lastRecursiveMovement = null;
								}
								else if (PgnGameViewModel.ShowVariations)
								{
									lastRecursiveMovement = CreateRecursiveMovement(lastRecursiveMovement, move, movementIndex, movementLineIndex, variationIndex);
									lastMovement = null;
								}
								// Incrementa los índices de movimientos
								movementIndex++;
								if (move.Color == Bau.Libraries.LibChessGame.Board.Pieces.PieceBaseModel.PieceColor.Black)
									movementLineIndex++;
							break;
						case MovementRemarksModel move:
								Movements.Add(new MovementRemarkViewModel(move));
								lastMovement = null;
								lastRecursiveMovement = null;
							break;
						case MovementGameEndModel move:
								Movements.Add(new MovementGameEndViewModel(move));
								lastMovement = null;
								lastRecursiveMovement = null;
							break;
					}
		}

		/// <summary>
		///		Crea un movimiento real
		/// </summary>
		private MovementFigureDoubleViewModel CreateRealMovement(MovementFigureDoubleViewModel lastMovement, MovementFigureModel move, 
																 int movementIndex, int movementLineIndex)
		{
			// Si es necesario, crea un nuevo movimiento
			if (lastMovement == null)
			{
				lastMovement = new MovementFigureDoubleViewModel(movementIndex, movementLineIndex);
				Movements.Add(lastMovement);
			}
			// Asigna el movimiento de blancas / negras
			if (move.Color == Bau.Libraries.LibChessGame.Board.Pieces.PieceBaseModel.PieceColor.White)
			{
				// Asigna el movimiento de blancas
				lastMovement.WhiteMovement = new MovementFigureViewModel(PgnGameViewModel, move, movementIndex, 0);
				// Carga la variación
				if (MustShowVariations(move))
				{
					LoadVariationMovements(move.Variation, movementIndex, movementLineIndex, 1);
					lastMovement = null;
				}
			}
			else // Asigna el movimiento de negras
			{
				// Asigna el movimiento de negras
				lastMovement.BlackMovement = new MovementFigureViewModel(PgnGameViewModel, move, movementIndex, 0);
				// Carga la variación
				if (MustShowVariations(move))
					LoadVariationMovements(move.Variation, movementIndex, movementLineIndex, 1);
				lastMovement = null;
			}
			// Devuelve el último movimiento añadido a la lista
			return lastMovement;
		}

		/// <summary>
		///		Crea un movimiento de una variación
		/// </summary>
		private MovementFigureVariationDoubleViewModel CreateRecursiveMovement(MovementFigureVariationDoubleViewModel lastMovement, 
																			   MovementFigureModel move, int movementIndex, int movementLineIndex,
																			   int variationIndex)
		{
			// Si es necesario, crea un nuevo movimiento
			if (lastMovement == null)
			{
				lastMovement = new MovementFigureVariationDoubleViewModel(movementIndex, movementLineIndex, variationIndex);
				Movements.Add(lastMovement);
			}
			// Asigna el movimiento de blancas / negras
			if (move.Color == Bau.Libraries.LibChessGame.Board.Pieces.PieceBaseModel.PieceColor.White)
			{
				// Asigna el movimiento de blancas
				lastMovement.WhiteMovement = new MovementFigureViewModel(PgnGameViewModel, move, movementIndex, variationIndex);
				// Carga la variación
				if (MustShowVariations(move))
				{
					LoadVariationMovements(move.Variation, movementIndex, movementLineIndex, variationIndex + 1);
					lastMovement = null;
				}
			}
			else // Asigna el movimiento de negras
			{
				// Asigna el movimiento de negras
				lastMovement.BlackMovement = new MovementFigureViewModel(PgnGameViewModel, move, movementIndex, variationIndex);
				// Carga la variación
				if (MustShowVariations(move))
					LoadVariationMovements(move.Variation, movementIndex, movementLineIndex, variationIndex + 1);
				lastMovement = null;
			}
			// Devuelve el último movimiento añadido a la lista
			return lastMovement;
		}

		/// <summary>
		///		Indica si se deben mostrar las variaciones
		/// </summary>
		private bool MustShowVariations(MovementFigureModel movement)
		{
			return PgnGameViewModel.ShowVariations && movement.Variation != null;
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
					if (_actualMovementIndex > 0)
						movement = FigureMovements[--_actualMovementIndex];
				}
				else
				{
					if (_actualMovementIndex < FigureMovements.Count)
					{
						movement = FigureMovements[_actualMovementIndex];
						_actualMovementIndex++;
					}
				}
				// Selecciona el movimiento
				if (movement != null)
					SelectMovement(_actualMovementIndex);
				// Devuelve el movimiento
				return movement;
		}

		/// <summary>
		///		Coloca la partida en un movimiento
		/// </summary>
		private void GoToMovement(MovementFigureViewModel movement)
		{
			// Inicializa el índice de movimientos
			_actualMovementIndex = 0;
			// Limpia el tablero
			GameBoard.Reset();
			PgnGameViewModel.RaiseEventReset();
			// Busca el movimiento
			while (_actualMovementIndex >= 0 && _actualMovementIndex < FigureMovements.Count &&
				   _actualMovementIndex < movement.MovementIndex)
				PgnGameViewModel.RaiseEventNextMovement();
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
				if (baseMovement is MovementFigureDoubleViewModel movement)
				{
					// Selecciona el movimiento blanco
					if (movement.WhiteMovement != null)
					{
						movement.WhiteMovement.Selected = movement.WhiteMovement.MovementIndex == index;
						if (movement.WhiteMovement.Selected)
						{
							SelectedMovement = movement;
							ActualMovement = movement.WhiteMovement;
						}
					}
					// Selecciona el movimiento negro
					if (movement.BlackMovement != null)
					{
						movement.BlackMovement.Selected = movement.BlackMovement.MovementIndex == index;
						if (movement.BlackMovement.Selected)
						{
							SelectedMovement = movement;
							ActualMovement = movement.BlackMovement;
						}
					}
				}
				else if (baseMovement is MovementFigureVariationDoubleViewModel variation)
				{
					if (variation.WhiteMovement != null)
						variation.WhiteMovement.Selected = false;
					if (variation.BlackMovement != null)
						variation.BlackMovement.Selected = false;
				}
		}

		/// <summary>
		///		ViewModel de un tablero
		/// </summary>
		public PgnGameViewModel PgnGameViewModel { get; }

		/// <summary>
		///		Tablero de juego
		/// </summary>
		public GameBoardModel GameBoard { get; private set; }

		/// <summary>
		///		Variación
		/// </summary>
		public VariationModel Variation { get; }

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

		/// <summary>
		///		Movimiento actual
		/// </summary>
		public MovementFigureViewModel ActualMovement
		{
			get { return _actualMovement; }
			set { CheckObject(ref _actualMovement, value); }
		}
	}
}
