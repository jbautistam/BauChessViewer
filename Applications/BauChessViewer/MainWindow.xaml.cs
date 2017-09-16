﻿using System;
using System.Windows;
using System.Windows.Input;

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
			ChessGameViewModel = new ViewModels.ChessGameViewModel(AppDomain.CurrentDomain.BaseDirectory);
			DataContext = ChessGameViewModel;
		}

		/// <summary>
		///		Inicializa el formulario
		/// </summary>
		private void InitForm()
		{
			// Inicializa el tablero
			ChessGameViewModel.Init();
			udtBoard.Init(ChessGameViewModel);
			// Carga el archivo inicial
			if (!string.IsNullOrEmpty(Properties.Settings.Default.Game))
				LoadGame(Properties.Settings.Default.Game);
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
				Properties.Settings.Default.Game = fileName;
				Properties.Settings.Default.Save();
			}
			else
				MessageBox.Show(error);
		}

		/// <summary>
		///		Muestra el movimiento siguiente / anterior
		/// </summary>
		private void ShowMovement(bool back)
		{
			if (ChessGameViewModel.ChessGame != null)
			{
				ViewModels.MovementFigureViewModel movement = ChessGameViewModel.SelectedGame.GameBoard.GetMovement(back);

					if (movement != null)
					{
						lblMovement.Text = movement.Movement.Text;
						udtBoard.ShowMovement(movement, back);
					}
			}
		}

		/// <summary>
		///		ViewModel para el juego
		/// </summary>
		public ViewModels.ChessGameViewModel ChessGameViewModel { get; private set; }

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			InitForm();
		}

		private void Button_MouseDown(object sender, MouseButtonEventArgs e)
		{
		}

		private void cmdOpenFile_Click(object sender, RoutedEventArgs e)
		{	
			OpenFile();
		}

		private void cmdNextMovement_Click(object sender, RoutedEventArgs e)
		{
			ShowMovement(false);
		}

		private void cmdPreviousMovement_Click(object sender, RoutedEventArgs e)
		{
			ShowMovement(true);
		}
	}
}