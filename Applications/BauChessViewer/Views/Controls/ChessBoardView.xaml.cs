using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Bau.Libraries.LibChessGame.Board;
using Bau.Libraries.LibChessGame.Movements;
using Bau.Libraries.LibChessGame.Pieces;

using BauChessViewer.ViewModels;

namespace BauChessViewer.Views.Controls
{
	/// <summary>
	///		Control para mostrar un tablero
	/// </summary>
	public partial class ChessBoardView : UserControl
	{
		/// <summary>
		///		Estructura para las figuras en el tablero
		/// </summary>
		private class Figure
		{
			public Figure(int row, int column, PieceBaseModel.PieceColor color, PieceBaseModel.PieceType? type, Image image, Label label)
			{
				Row = row;
				Column = column;
				Color = color;
				Type = type;
				Image = image;
				Label = label;
			}

			/// <summary>
			///		Fila
			/// </summary>
			public int Row { get; set; }

			/// <summary>
			///		Columna
			/// </summary>
			public int Column { get; set; }

			/// <summary>
			///		Color
			/// </summary>
			public PieceBaseModel.PieceColor Color { get; }

			/// <summary>
			///		Tipo de pieza (null si es una celda)
			/// </summary>
			public PieceBaseModel.PieceType? Type { get; }

			/// <summary>
			///		Imagen de la figura
			/// </summary>
			public Image Image { get; }

			/// <summary>
			///		Etiqueta
			/// </summary>
			public Label Label { get; }
		}

		public ChessBoardView()
		{
			InitializeComponent();
		}

		/// <summary>
		///		Inicializa el tablero
		/// </summary>
		public void Init(ChessGameViewModel chessGameViewModel)
		{
			ViewModel = chessGameViewModel;
			ViewModel.ResetGame += (sender, eventArgs) => Reset();
			ViewModel.ComboPathBoard.PropertyChanged += ComboPathImages_PropertyChanged;
			ViewModel.ComboPathPieces.PropertyChanged += ComboPathImages_PropertyChanged;
		}

		/// <summary>
		///		Inicializa el tablero
		/// </summary>
		private void Reset()
		{
			if (ViewModel.SelectedGame != null)
			{
				PieceBaseModel.PieceColor color = PieceBaseModel.PieceColor.White;

					// Limpia el canvas
					Cells.Clear();
					udtCanvas.Children.Clear();
					// Añade las celdas
					for (int row = 0; row < 8; row++)
					{
						// Dibuja por columnas
						for (int column = 0; column < 8; column++)
						{
							Cells.Add(CreateFigure(row, column, color, null));
							color = GetNextColor(color);
						}
						// Cambia el color de inicio de la siguiente fila
						color = GetNextColor(color);
					}
					// Añade las etiquetas
					for (int row = 0; row < 8; row++)
						Cells.Add(CreateLabel(row, -1, (char) ('A' + row)));
					for (int column = 0; column < 8; column++)
						Cells.Add(CreateLabel(-1, column, (char) ('1' + column)));
					// Añade las piezas
					foreach (PieceBaseModel piece in ViewModel.SelectedGame.GameBoard.GameBoard.Pieces)
						Cells.Add(CreateFigure(piece.Cell.Row, piece.Cell.Column, piece.Color, piece.Type));
					// Muestra las imágenes
					ShowImages();
			}
		}

		/// <summary>
		///		Crea una etiqueta
		/// </summary>
		private Figure CreateLabel(int row, int column, char content)
		{
			Label label = new Label 
									{ 
										Content = content.ToString(), 
										HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
										VerticalContentAlignment = System.Windows.VerticalAlignment.Center,
										FontSize = 16, 
										FontWeight = System.Windows.FontWeights.Bold 
									};

				// Añade la figura al canvas
				udtCanvas.Children.Add(label);
				// Devuelve la figura generada
				return new Figure(row, column, PieceBaseModel.PieceColor.White, null, null, label);
		}

		/// <summary>
		///		Crea una figura
		/// </summary>
		private Figure CreateFigure(int row, int column, PieceBaseModel.PieceColor color, PieceBaseModel.PieceType? type)
		{
			Figure image = new Figure(row, column, color, type, CreateImage(color, type), null);

				// Devuelve la imagen
				return image;
		}

		/// <summary>
		///		Crea una imagen y la añade al canvas
		/// </summary>
		private Image CreateImage(PieceBaseModel.PieceColor color, PieceBaseModel.PieceType? type)
		{
			Image view = new Image();

				// Carga la imagen
				view.Source = LoadImage(GetImageFileName(color, type));
				view.Stretch = Stretch.Fill;
				// Añade la imagen al control
				Canvas.SetTop(view, 0);
				Canvas.SetLeft(view, 0);
				// Añade el elemento al canvas
				udtCanvas.Children.Add(view);
				// Devuelve el control
				return view;
		}

		/// <summary>
		///		Cambia las imágenes
		/// </summary>
		private void ChangeImages()
		{
			foreach (Figure piece in Cells)
				if (piece != null && piece.Image != null)
					piece.Image.Source = LoadImage(GetImageFileName(piece.Color, piece.Type));
		}

		/// <summary>
		///		Obtiene el nombre de archivo a cargar
		/// </summary>
		private string GetImageFileName(PieceBaseModel.PieceColor color, PieceBaseModel.PieceType? type)
		{
			string pathBoard = ViewModel.ComboPathBoard.SelectedPathImages;
			string pathPieces = ViewModel.ComboPathPieces.SelectedPathImages;

				if (type == null)
					switch (color)
					{
						case PieceBaseModel.PieceColor.Black:
							return System.IO.Path.Combine(pathBoard, "BoardDark.gif");
						case PieceBaseModel.PieceColor.White:
							return System.IO.Path.Combine(pathBoard, "BoardLight.gif");
					}
				else
				{
					string pieceColor = color == PieceBaseModel.PieceColor.White ? "White" : "Black";

						switch (type)
						{
							case PieceBaseModel.PieceType.Pawn:
								return System.IO.Path.Combine(pathPieces, $"Pawn{pieceColor}.gif");
							case PieceBaseModel.PieceType.Rook:
								return System.IO.Path.Combine(pathPieces, $"Rook{pieceColor}.gif");
							case PieceBaseModel.PieceType.Knight:
								return System.IO.Path.Combine(pathPieces, $"Knight{pieceColor}.gif");
							case PieceBaseModel.PieceType.Bishop:
								return System.IO.Path.Combine(pathPieces, $"Bishop{pieceColor}.gif");
							case PieceBaseModel.PieceType.King:
								return System.IO.Path.Combine(pathPieces, $"King{pieceColor}.gif");
							case PieceBaseModel.PieceType.Queen:
								return System.IO.Path.Combine(pathPieces, $"Queen{pieceColor}.gif");
						}
				}
				return "";
		}

		/// <summary>
		///		Carga una imagen
		/// </summary>
		private BitmapImage LoadImage(string fileName)
		{
			BitmapImage image = null;

				// Crea un nuevo bitmap
				image = new BitmapImage();
				// Inicializa la carga de imagen
				image.BeginInit();
				// Carga la imagen
				if (!string.IsNullOrEmpty(fileName) && System.IO.File.Exists(fileName))
					image.UriSource = new Uri(fileName, UriKind.Relative);
				else
					image.UriSource = new Uri($"pack://application:,,,/Resources/Images/{System.IO.Path.GetFileName(fileName)}", UriKind.Absolute);
				// Finaliza la carga de la imagen
				image.CacheOption = BitmapCacheOption.OnLoad; // ... carga la imagen en este momento, no la deja en caché
				image.EndInit();
				// Devuelve la imagen cargada
				return image;
		}

		/// <summary>
		///		Obtiene el siguiente color
		/// </summary>
		private PieceBaseModel.PieceColor GetNextColor(PieceBaseModel.PieceColor color)
		{
			return color == PieceBaseModel.PieceColor.White ? PieceBaseModel.PieceColor.Black : PieceBaseModel.PieceColor.White;
		}

		/// <summary>
		///		Muestra las imágenes
		/// </summary>
		private void ShowImages()
		{
			int labelWidth = 30;
			int labelHeight = 30;
			int width = (int) (ActualWidth - labelWidth) / 8;
			int height = (int) (ActualHeight - labelHeight) / 8;

				foreach (Figure cell in Cells)
					if (cell.Label == null)
					{
						Canvas.SetTop(cell.Image, labelHeight + height * cell.Row);
						Canvas.SetLeft(cell.Image, labelWidth + width * cell.Column);
						cell.Image.Width = width;
						cell.Image.Height = height;
						if (cell.Type == null)
							Canvas.SetZIndex(cell.Image, 0);
						else
							Canvas.SetZIndex(cell.Image, 1);
					}
					else
					{
						if (cell.Column == -1) // ... cabeceras de fila
						{
							Canvas.SetTop(cell.Label, labelHeight + (height - labelHeight) / 2 + height * cell.Row);
							Canvas.SetLeft(cell.Label, 0);
						}
						else // ... cabeceras de columna
						{
							Canvas.SetTop(cell.Label, 0);
							Canvas.SetLeft(cell.Label, labelWidth +  (width - labelWidth) / 2 + width * cell.Column);
						}
						cell.Label.Width = labelWidth;
						cell.Label.Height = labelHeight;
						//cell.Image.Width = width;
						//cell.Image.Height = height;
					}
		}

		/// <summary>
		///		Muestra un movimiento
		/// </summary>
		internal void ShowMovement(MovementFigureViewModel movement, bool backMovement)
		{
			// Recorre las acciones del movimiento
			if (!backMovement)
				foreach (ActionBaseModel action in movement.Movement.Actions)
					switch (action)
					{
						case ActionMoveModel move:
								MovePiece(move);
							break;
						case ActionCaptureModel move:
								CapturePiece(move);
							break;
						case ActionPromoteModel move:
								PromotePiece(move);
							break;
					}
			else
				for (int index = movement.Movement.Actions.Count - 1; index >= 0; index--)
					switch (movement.Movement.Actions[index])
					{
						case ActionMoveModel move:
								UndoMovePiece(move);
							break;
						case ActionCaptureModel move:
								UndoCapturePiece(move);
							break;
						case ActionPromoteModel move:
								UndoPromotePiece(move);
							break;
					}
			// Muestra las imágenes
			ShowImages();
		}

		/// <summary>
		///		Mueve una pieza
		/// </summary>
		private void MovePiece(ActionMoveModel action)
		{
			Figure piece = SearchPiece(action.Type, action.Color, action.From);

				// Cambia la posición de la pieza
				if (piece.Type != null)
				{
					piece.Column = action.To.Column;
					piece.Row = action.To.Row;
				}
				else
					System.Diagnostics.Debug.WriteLine("No se encuentra");
		}

		/// <summary>
		///		Deshace el movimiento de una pieza
		/// </summary>
		private void UndoMovePiece(ActionMoveModel action)
		{
			Figure piece = SearchPiece(action.Type, action.Color, action.To);

				// Cambia la posición de la pieza
				if (piece.Type != null)
				{
					piece.Column = action.From.Column;
					piece.Row = action.From.Row;
				}
				else
					System.Diagnostics.Debug.WriteLine("No se encuentra");
		}

		/// <summary>
		///		Busca una pieza
		/// </summary>
		private Figure SearchPiece(PieceBaseModel.PieceType type, PieceBaseModel.PieceColor color, CellModel cell)
		{
			// Busca la pieza
			foreach (Figure figure in Cells)
				if (figure.Type == type && figure.Color == color && figure.Row == cell.Row && figure.Column == cell.Column)
					return figure;
			// Devuelve una pieza vacía
			return new Figure(-1, -1, color, null, null, null);
		}

		/// <summary>
		///		Captura una pieza
		/// </summary>
		private void CapturePiece(ActionCaptureModel action)
		{
			Figure figure = SearchPiece(action.Type, action.Color, action.From);

				if (figure.Type != null)
				{	
					Cells.Remove(figure);
					udtCanvas.Children.Remove(figure.Image);
				}
				else
					System.Diagnostics.Debug.WriteLine("No se encuentra");
		}

		/// <summary>
		///		Deshace el movimiento de captura de una pieza
		/// </summary>
		private void UndoCapturePiece(ActionCaptureModel action)
		{
			Cells.Add(CreateFigure(action.From.Row, action.From.Column, action.Color, action.Type));
		}

		/// <summary>
		///		Promociona una pieza
		/// </summary>
		private void PromotePiece(ActionPromoteModel action)
		{
			Cells.Add(CreateFigure(action.To.Row, action.To.Column, action.Color, action.Type));
		}

		/// <summary>
		///		Deshace el movimiento de promoción de una pieza
		/// </summary>
		private void UndoPromotePiece(ActionPromoteModel action)
		{
			Figure figure = SearchPiece(action.Type, action.Color, action.To);

				if (figure.Type != null)
				{	
					Cells.Remove(figure);
					udtCanvas.Children.Remove(figure.Image);
				}
				else
					System.Diagnostics.Debug.WriteLine("No se encuentra");
		}

		/// <summary>
		///		Controles con los datos de las celdas
		/// </summary>
		private List<Figure> Cells { get; } = new List<Figure>();

		/// <summary>
		///		ViewModel
		/// </summary>
		public ChessGameViewModel ViewModel { get; private set; }

		private void UserControl_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
		{
			ShowImages();
		}

		private void ComboPathImages_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals(nameof(PathComboImagesViewModel.SelectedPathImages), StringComparison.CurrentCultureIgnoreCase))
				ChangeImages();
		}
	}
}
