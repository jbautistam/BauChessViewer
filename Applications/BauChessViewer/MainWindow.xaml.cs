using System;
using System.Windows;

using Bau.Libraries.LibChessGame.Board.Movements;

namespace BauChessViewer
{
	/// <summary>
	///		Ventana principal
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			ChessGameViewModel = new ViewModels.PgnGameViewModel(AppDomain.CurrentDomain.BaseDirectory);
			DataContext = ChessGameViewModel;
		}

		/// <summary>
		///		Inicializa el formulario
		/// </summary>
		private void InitForm()
		{
			// Inicializa el tablero
			ChessGameViewModel.Init();
			ChessGameViewModel.ShowNextMovement += (sender, evntArgs) => ShowMovement(false, evntArgs.ShowAnimation);
			udtBoard.Init(ChessGameViewModel);
			// Carga el archivo inicial
			if (!string.IsNullOrEmpty(Properties.Settings.Default.Game))
				LoadGame(Properties.Settings.Default.Game);
			// Inicia los directorios de imágenes
			if (!string.IsNullOrEmpty(Properties.Settings.Default.PathBoardImages))
				ChessGameViewModel.ComboPathBoard.SelectedPath = Properties.Settings.Default.PathBoardImages;
			if (!string.IsNullOrEmpty(Properties.Settings.Default.PathPieceImages))
				ChessGameViewModel.ComboPathPieces.SelectedPath = Properties.Settings.Default.PathPieceImages;
			// Asigna el resto de las propiedades
			ChessGameViewModel.ShowVariations = Properties.Settings.Default.ShowVariations;
			ChessGameViewModel.MustShowAnimation = Properties.Settings.Default.MustShowAnimations;
		}

		/// <summary>
		///		Abre la ventana de selección de un archivo
		/// </summary>
		private void OpenFile()
		{
			Microsoft.Win32.OpenFileDialog file = new Microsoft.Win32.OpenFileDialog();

				// Asigna las propiedades
				file.Multiselect = false;
				file.InitialDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data\\Samples");
				// file.FileName = defaultFileName;
				file.DefaultExt = "pgn";
				file.Filter = "Archivos PGN (*.pgn)|*.pgn|Todos los archivos|*.*";
				// Muestra el cuadro de diálogo y devuelve los archivos
				if (file.ShowDialog() ?? false && file.FileNames.Length > 0)
					LoadGame(file.FileNames[0]);
		}

		/// <summary>
		///		Carga un juego
		/// </summary>
		private void LoadGame(string fileName)
		{
			if (ChessGameViewModel.Load(fileName, out string error))
			{
				// Limpia el último movimiento
				ClearMovementPanel();
				// Guarda el archivo en la configuración
				Properties.Settings.Default.Game = fileName;
				Properties.Settings.Default.Save();
			}
			else
				MessageBox.Show(error);
		}

		/// <summary>
		///		Limpia el panel de último movimiento
		/// </summary>
		private void ClearMovementPanel()
		{
			lblMovement.Text = "";
			imgMovement.Source = null;
		}

		/// <summary>
		///		Muestra el movimiento siguiente / anterior
		/// </summary>
		private void ShowMovement(bool back, bool showAnimation)
		{
			if (ChessGameViewModel.ChessGame != null)
			{
				MovementFigureModel movement = ChessGameViewModel.GetMovement(back);

					// Limpia el panel
					ClearMovementPanel();
					// Obtiene el siguiente movimiento
					if (ChessGameViewModel.MovementsList.ActualMovement != null)
					{
						lblMovement.Text = ChessGameViewModel.MovementsList.ActualMovement.Text;
						imgMovement.Source = udtBoard.LoadImage(ChessGameViewModel.MovementsList.ActualMovement.Color, 
																ChessGameViewModel.MovementsList.ActualMovement.Piece);
						udtBoard.ShowMovement(movement, back, showAnimation);
					}
			}
		}

		/// <summary>
		///		Graba la configuración
		/// </summary>
		private void SaveConfiguration()
		{
			Properties.Settings.Default.PathBoardImages = ChessGameViewModel.ComboPathBoard.SelectedPath;
			Properties.Settings.Default.PathPieceImages = ChessGameViewModel.ComboPathPieces.SelectedPath;
			Properties.Settings.Default.ShowVariations = ChessGameViewModel.ShowVariations;
			Properties.Settings.Default.MustShowAnimations = ChessGameViewModel.MustShowAnimation;
			Properties.Settings.Default.Save();
		}

		/// <summary>
		///		ViewModel para el juego
		/// </summary>
		public ViewModels.PgnGameViewModel ChessGameViewModel { get; private set; }

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			InitForm();
		}

		private void cmdOpenFile_Click(object sender, RoutedEventArgs e)
		{	
			OpenFile();
		}

		private void cmdNextMovement_Click(object sender, RoutedEventArgs e)
		{
			ShowMovement(false, true);
		}

		private void cmdPreviousMovement_Click(object sender, RoutedEventArgs e)
		{
			ShowMovement(true, true);
		}

		private void lstMovements_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (sender is System.Windows.Controls.ListBox lstView && ChessGameViewModel?.MovementsList?.SelectedMovement != null)
				lstView.ScrollIntoView(ChessGameViewModel.MovementsList.SelectedMovement);
		}

		private void cboGame_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			ClearMovementPanel();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			SaveConfiguration();
		}
	}
}
