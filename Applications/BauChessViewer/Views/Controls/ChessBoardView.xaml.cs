using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

using Bau.Libraries.LibChessGame.Board;
using Bau.Libraries.LibChessGame.Board.Movements;
using Bau.Libraries.LibChessGame.Board.Pieces;
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

		/// <summary>
		///		Acción de un movimiento para su visualización
		/// </summary>
		private class ActionViewMovement
		{
			/// <summary>
			///		Tipo de acción
			/// </summary>
			public enum ActionType
			{
				/// <summary>Mover</summary>
				Move,
				/// <summary>Crear</summary>
				Create,
				/// <summary>Destruir</summary>
				Destroy
			}

			public ActionViewMovement(ActionType type, Figure figure, int endRow, int endColumn)
			{
				Type = type;
				Figure = figure;
				EndRow = endRow;
				EndColumn = endColumn;
			}

			/// <summary>
			///		Tipo de acción
			/// </summary>
			public ActionType Type { get; }

			/// <summary>
			///		Figura asociada
			/// </summary>
			public Figure Figure { get; }

			/// <summary>
			///		Fila final del movimiento
			/// </summary>
			public int EndRow { get; }

			/// <summary>
			///		Columna final del movimiento
			/// </summary>
			public int EndColumn { get; }
		}

		public ChessBoardView()
		{
			InitializeComponent();
		}

		/// <summary>
		///		Inicializa el tablero
		/// </summary>
		public void Init(PgnGameViewModel chessGameViewModel)
		{
			ViewModel = chessGameViewModel;
			ViewModel.ComboPathBoard.PropertyChanged += ComboPathImages_PropertyChanged;
			ViewModel.ComboPathPieces.PropertyChanged += ComboPathImages_PropertyChanged;
			ViewModel.ResetGame += (sender, eventArgs) => Reset();
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
						Cells.Add(CreateLabel(row, -1, (char) ('0' + 8 - row)));
					for (int column = 0; column < 8; column++)
						Cells.Add(CreateLabel(-1, column, (char) ('A' + column)));
					// Añade las piezas
					foreach (PieceBaseModel piece in ViewModel.MovementsList.GameBoard.Pieces)
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
			return new Figure(row, column, color, type, CreateImage(color, type), null);
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
		///		Obtiene el nombre de archivo de imagen a cargar
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
		internal BitmapImage LoadImage(PieceBaseModel.PieceColor color, PieceBaseModel.PieceType? type)
		{
			return LoadImage(GetImageFileName(color, type));
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
					}
		}

		/// <summary>
		///		Muestra un movimiento
		/// </summary>
		internal void ShowMovement(MovementFigureModel movement, bool backMovement, bool showAnimation)
		{
			if (movement != null)
			{
				Storyboard sbStoryBoard = new Storyboard();
				List<ActionViewMovement> actions = new List<ActionViewMovement>();

					// Asigna el evento de fin de animación
					sbStoryBoard.Completed += (sender, evntArgs) => ExecuteActions(actions);
					// Limpia el storyBoard
					sbStoryBoard.Children.Clear();
					// Asigna las propiedades de duración
					sbStoryBoard.BeginTime = TimeSpan.FromSeconds(0);
					sbStoryBoard.Duration = new Duration(TimeSpan.FromSeconds(5));
					// Recorre las acciones del movimiento hacia delante o hacia atrás
					if (!backMovement)
						foreach (ActionBaseModel action in movement.Actions)
							switch (action)
							{
								case ActionMoveModel move:
										actions.Add(GetActionMovePiece(move));
									break;
								case ActionCaptureModel move:
										actions.Add(GetActionCapturePiece(move));
									break;
								case ActionPromoteModel move:
										actions.Add(GetActionPromotePiece(move));
									break;
							}
					else
						for (int index = movement.Actions.Count - 1; index >= 0; index--)
							switch (movement.Actions[index])
							{
								case ActionMoveModel move:
										actions.Add(GetActionUndoMovePiece(move));
									break;
								case ActionCaptureModel move:
										actions.Add(GetActionUndoCapturePiece(move));
									break;
								case ActionPromoteModel move:
										actions.Add(GetActionUndoPromotePiece(move));
									break;
							}
					// Crea las animaciones

					// Muestra las imágenes
					if (showAnimation && sbStoryBoard.Children.Count > 0)
						sbStoryBoard.Begin();
					// else
						ExecuteActions(actions);
			}
		}

		/// <summary>
		///		Añade una animación al storyBoard
		/// </summary>
		private void AddAnimationToStoryBoard(Storyboard storyBoard, DependencyObject control, AnimationTimeline animation, 
											  int startSeconds, int endSeconds, PropertyPath propertyPath)
		{   
			// Asigna las propiedades de inicio y duración
			animation.BeginTime = TimeSpan.FromSeconds(startSeconds);
			animation.Duration = TimeSpan.FromSeconds(endSeconds);
			//// Asigna las propiedades de velocidad
			//if (action.Parameters.AccelerationRatio != null)
			//	animation.AccelerationRatio = action.Parameters.AccelerationRatio ?? 0;
			//else if (action.TimeLine.Parameters.AccelerationRatio != null)
			//	animation.AccelerationRatio = action.TimeLine.Parameters.AccelerationRatio ?? 0;
			//if (action.Parameters.DecelerationRatio != null)
			//	animation.DecelerationRatio = action.Parameters.DecelerationRatio ?? 0;
			//else if (action.TimeLine.Parameters.DecelerationRatio != null)
			//	animation.DecelerationRatio = action.TimeLine.Parameters.DecelerationRatio ?? 0;
			//if (action.Parameters.SpeedRatio != null)
			//	animation.SpeedRatio = action.Parameters.SpeedRatio ?? 0;
			//else if (action.TimeLine.Parameters.SpeedRatio != null)
			//	animation.SpeedRatio = action.TimeLine.Parameters.SpeedRatio ?? 0;
			// Añade los datos a la animación
			Storyboard.SetTarget(animation, control);
			Storyboard.SetTargetProperty(animation, propertyPath);
			// Añade la animación al storyboard
			storyBoard.Children.Add(animation);
		}

		/// <summary>
		///		Obtiene una acción para mover una pieza
		/// </summary>
		private ActionViewMovement GetActionMovePiece(ActionMoveModel action)
		{
			Figure piece = SearchPiece(action.Type, action.Color, action.From);

				// Obtiene el movimiento
				if (piece.Type != null)
					return new ActionViewMovement(ActionViewMovement.ActionType.Move, piece, action.To.Row, action.To.Column);
				else
					return null;
		}

		/// <summary>
		///		Obtiene el movimiento para deshacer el movimiento de una pieza
		/// </summary>
		private ActionViewMovement GetActionUndoMovePiece(ActionMoveModel action)
		{
			Figure piece = SearchPiece(action.Type, action.Color, action.To);

				// Cambia la posición de la pieza
				if (piece.Type != null)
					return new ActionViewMovement(ActionViewMovement.ActionType.Move, piece, action.From.Row, action.From.Column);
				else
					return null;
		}

		/// <summary>
		///		Obtiene el movimiento para capturar una pieza
		/// </summary>
		private ActionViewMovement GetActionCapturePiece(ActionCaptureModel action)
		{
			Figure figure = SearchPiece(action.Type, action.Color, action.From);

				if (figure.Type != null)
					return new ActionViewMovement(ActionViewMovement.ActionType.Destroy, figure, 0, 0);
				else
					return null;
		}

		/// <summary>
		///		Obtiene el movimiento para deshacer la captura de una pieza
		/// </summary>
		private ActionViewMovement GetActionUndoCapturePiece(ActionCaptureModel action)
		{
			return new ActionViewMovement(ActionViewMovement.ActionType.Create, 
										  CreateFigure(action.From.Row, action.From.Column, action.Color, action.Type),
										  action.From.Row, action.From.Column);
		}

		/// <summary>
		///		Promociona una pieza
		/// </summary>
		private ActionViewMovement GetActionPromotePiece(ActionPromoteModel action)
		{
			return new ActionViewMovement(ActionViewMovement.ActionType.Create,
										  CreateFigure(action.To.Row, action.To.Column, action.Color, action.Type),
										  action.To.Row, action.To.Column);
		}

		/// <summary>
		///		Deshace el movimiento de promoción de una pieza
		/// </summary>
		private ActionViewMovement GetActionUndoPromotePiece(ActionPromoteModel action)
		{
			Figure figure = SearchPiece(action.Type, action.Color, action.To);

				if (figure.Type != null)
					return new ActionViewMovement(ActionViewMovement.ActionType.Destroy, figure,
												  action.To.Row, action.To.Column);
				else
					return null;
		}

		/// <summary>
		///		Ejecuta una serie de acciones sobre la lista de figuras
		/// </summary>
		private void ExecuteActions(List<ActionViewMovement> actions)
		{
			// Ejecuta las acciones
			foreach (ActionViewMovement action in actions)
				switch (action.Type)
				{
					case ActionViewMovement.ActionType.Move:
							action.Figure.Row = action.EndRow;
							action.Figure.Column = action.EndColumn;
						break;
					case ActionViewMovement.ActionType.Create:
							Cells.Add(action.Figure);
						break;
					case ActionViewMovement.ActionType.Destroy:
							Cells.Remove(action.Figure);
							udtCanvas.Children.Remove(action.Figure.Image);
						break;
				}
			// Muestra las imágenes
			ShowImages();
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
		///		Controles con los datos de las celdas
		/// </summary>
		private List<Figure> Cells { get; } = new List<Figure>();

		/// <summary>
		///		ViewModel
		/// </summary>
		public PgnGameViewModel ViewModel { get; private set; }

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
