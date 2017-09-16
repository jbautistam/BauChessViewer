using System;
using System.Collections.ObjectModel;
using Bau.Libraries.LibChessGame.Board;
using Bau.Libraries.LibChessGame.Movements;

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
			GameBoard.Reset();
			// Recoge los movimientos (sólo los de figuras)
			LoadMovements(Game.Game);
			// Inicializa el movimiento actual
			_actualMovement = 0;
		}

		/// <summary>
		///		Carga los movimientos de un juego
		/// </summary>
		private void LoadMovements(GameModel game)
		{
			// Limpia los movimientos
			Movements.Clear();
			// Carga los movimientos
			if (game != null)
				foreach (MovementBaseModel movement in game.Movements)
					switch (movement)
					{
						case MovementFigureModel move:
								Movements.Add(new MovementFigureViewModel(move));
							break;
						case MovementRemarksModel move:
								Movements.Add(new MovementRemarkViewModel(move));
							break;
						case MovementGameEndModel move:
								Movements.Add(new MovementGameEndViewModel(move));
							break;
					}
			// Añade un movimiento si no había ninguno
			if (Movements.Count == 0)
				Movements.Add(new MovementRemarkViewModel(new MovementRemarksModel("No hay ningún movimiento en este juego")));
			// Selecciona el primer movimiento
			SelectedMovement = Movements[0];
		}

		/// <summary>
		///		Obtiene el siguiente movimiento (hacia atrás o hacia delante)
		/// </summary>
		internal MovementFigureViewModel GetMovement(bool back)
		{
			MovementFigureViewModel movement = null;

				// Obtiene el movimiento
				if (back)
					movement = GetPreviousMovement();
				else
					movement = GetNextMovement();
				// Selecciona el movimiento
				if (movement != null)
					SelectedMovement = movement;
				// Devuelve el movimiento
				return movement;
		}

		/// <summary>
		///		Obtiene el movimiento anterior
		/// </summary>
		private MovementFigureViewModel GetPreviousMovement()
		{
			MovementFigureViewModel movement = null;

				// Decrementa el movimiento para obtener el movimiento que se acaba de hacer
				_actualMovement = Math.Max(0, _actualMovement - 1);
				// Busca el siguiente movimiento válido
				while (_actualMovement >= 0 && movement == null)
					if (Movements[_actualMovement] is MovementFigureViewModel)
						movement = Movements[_actualMovement] as MovementFigureViewModel;
					else
						_actualMovement--;
				// Devuelve el movimiento
				return movement;
		}

		/// <summary>
		///		Obtiene el siguiente movimiento
		/// </summary>
		private MovementFigureViewModel GetNextMovement()
		{
			MovementFigureViewModel movement = null;

				// Busca el siguiente movimiento válido
				while (_actualMovement < Movements.Count && movement == null)
					if (Movements[_actualMovement] is MovementFigureViewModel)
						movement = Movements[_actualMovement++] as MovementFigureViewModel;
					else
						_actualMovement++;
				// Devuelve el movimiento
				return movement;
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
		///		Movimientos
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
