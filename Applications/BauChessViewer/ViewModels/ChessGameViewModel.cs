using System;
using System.Collections.ObjectModel;

using Bau.Libraries.LibChessGame;
using Bau.Libraries.LibChessGame.Movements;

namespace BauChessViewer.ViewModels
{
	/// <summary>
	///		ViewModel para un <see cref="ChessGameModel"/>
	/// </summary>
	public class ChessGameViewModel : BaseViewModel
	{
		// Constantes privadas
		private const string SubPathBoard = @"Data\Graphics\Boards";
		private const string SubPathPiecess = @"Data\Graphics\Pieces";
		// Eventos públicos
		public event EventHandler ResetGame;
		public event EventHandler ShowNextMovement;
		// Variables privadas
		private string _fileName;
		private GameViewModel _selectedGame;
		private PathComboImagesViewModel _comboPathBoard, _comboPathPieces;

		public ChessGameViewModel(string pathApplication)
		{
			PathApplication = pathApplication;
			Init();
		}

		/// <summary>
		///		Inicializa el ViewModel
		/// </summary>
		public void Init()
		{
			ComboPathBoard = new PathComboImagesViewModel(System.IO.Path.Combine(PathApplication, SubPathBoard));
			ComboPathPieces = new PathComboImagesViewModel(System.IO.Path.Combine(PathApplication, SubPathPiecess));
			Load(new ChessGameModel());
		}

		/// <summary>
		///		Carga un archivo
		/// </summary>
		public bool Load(string fileName, out string error)
		{
			// Inicializa los argumentos de salida
			error = "";
			// Carga el juego
			if (!string.IsNullOrEmpty(fileName) && System.IO.File.Exists(fileName))
			{
				// Inicializa el juego
				ChessGame = new ChessGameModel();
				// Carga el juego
				try
				{
					// Carga el juego
					ChessGame.Load(fileName);
					// y lo muestra
					Load(ChessGame);
					// Asigna el nombre de archivo
					FileName = System.IO.Path.GetFileName(fileName);
				}
				catch (Exception exception)
				{
					error = $"Error al cargar el juego: {exception.Message}";
				}
			}
			// Devuelve el valor que indica si los datos son correctos
			return string.IsNullOrEmpty(error);
		}

		/// <summary>
		///		Carga los datos de un juego
		/// </summary>
		private void Load(ChessGameModel chessGame)
		{
			// Asigna el archivo
			ChessGame = chessGame;
			// Limpia la lista
			Games.Clear();
			// Carga los juegos
			foreach (GameModel game in chessGame.Games)
				Games.Add(new GameViewModel(this, game));
			// Selecciona un elemento
			if (Games.Count > 0)
				SelectedGame = Games[0];
			else
				SelectedGame = new GameViewModel(this, new GameModel());
		}

		/// <summary>
		///		Modifica el juego seleccionado
		/// </summary>
		private void UpdateSelectedGame()
		{
			SelectedGame?.GameBoard.Reset();
			RaiseEventReset();
		}

		/// <summary>
		///		lanza el evento de reset
		/// </summary>
		internal void RaiseEventReset()
		{
			ResetGame?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		///		lanza el evento de siguiente movimiento
		/// </summary>
		internal void RaiseEventNextMovement()
		{
			ShowNextMovement?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		///		Datos del archivo
		/// </summary>
		public ChessGameModel ChessGame { get; private set; }

		/// <summary>
		///		Nombre del archivo abierto
		/// </summary>
		public string FileName
		{
			get { return _fileName; }
			set { CheckProperty(ref _fileName, value); }
		}

		/// <summary>
		///		Juegos
		/// </summary>
		public ObservableCollection<GameViewModel> Games { get; } = new ObservableCollection<GameViewModel>();

		/// <summary>
		///		Juego seleccionado
		/// </summary>
		public GameViewModel SelectedGame
		{
			get { return _selectedGame; }
			set 
			{ 
				if (CheckObject(ref _selectedGame, value))
					UpdateSelectedGame();
			}
		}

		/// <summary>
		///		Directorio de la aplicación
		/// </summary>
		public string PathApplication { get; }

		/// <summary>
		///		Combo para los directorios de imágenes del tablero
		/// </summary>
		public PathComboImagesViewModel ComboPathBoard 
		{ 
			get { return _comboPathBoard; }
			set { CheckObject(ref _comboPathBoard, value); }
		}

		/// <summary>
		///		Combo para los directorios de imágenes de las piedas
		/// </summary>
		public PathComboImagesViewModel ComboPathPieces 
		{ 
			get { return _comboPathPieces; }
			set { CheckObject(ref _comboPathPieces, value); }
		}
	}
}
