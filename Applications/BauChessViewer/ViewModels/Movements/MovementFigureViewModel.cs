using System;
using System.Windows.Media;

using Bau.Libraries.LibChessGame.Board.Movements;
using Bau.Libraries.LibChessGame.Board.Pieces;

namespace BauChessViewer.ViewModels.Movements
{
	/// <summary>
	///		ViewModel del movimiento de una figura
	/// </summary>
	public class MovementFigureViewModel : BaseMovementViewModel
	{
		// Variables privadas
		private MovementFigureModel _movement;
		private PieceBaseModel.PieceType _piece;
		private PieceBaseModel.PieceColor _color;
		private int _movementIndex, _variationIndex, _fontSize, _imageWidht;
		private string _text, _time;
		private bool _hasVariation, _selected;
		private SolidColorBrush _foreGround;
		private SolidColorBrush _backGround;

		public MovementFigureViewModel(PgnGameViewModel pgnGameViewModel, MovementFigureModel movement, int movementIndex, int variationIndex)
		{
			// Inicializa las propiedades
			PgnGameViewModel = pgnGameViewModel;
			VariationIndex = variationIndex;
			Selected = false;
			Piece = movement.OriginPiece;
			Color = movement.Color;
			MovementIndex = movementIndex;
			Movement = movement;
			Text = movement.Text;
			HasVariation = movement.Variation != null;
			Time = "01:07";
			// Inicializa la forma en que se debe ver el movimiento en la lista (tamaños de fuente, tamaños de imagen...)
			AssignViewSizes(variationIndex, Selected);
			// Inicializa los comandos
			SelectMovementCommand = new BaseCommand(parameter => ExecuteMovement(), parameter => CanExecuteMovement());
		}

		/// <summary>
		///		Asigna los tamaños y fuentes que se deben mostrar en la lista de movimientos dependiendo de la variación
		/// </summary>
		private void AssignViewSizes(int variationIndex, bool isSelected)
		{
			FontSize = GetFontSize(variationIndex, new int[] { 25, 20, 18, 16, 14 });
			ImageWidth = GetImageSize(variationIndex, new int[] { 32, 20, 18, 16, 14 });
			if (Selected)
			{
				Foreground = new SolidColorBrush(Colors.White);
				Background = new SolidColorBrush(Colors.Navy);
			}
			else
			{
				Foreground = GetForeGround(variationIndex, new SolidColorBrush[] 
																		{
																			new SolidColorBrush(Colors.Black),
																			new SolidColorBrush(Colors.Green),
																			new SolidColorBrush(Colors.Navy),
																			new SolidColorBrush(Colors.Blue),
																			new SolidColorBrush(Colors.Maroon)
																		});
				Background = new SolidColorBrush(Colors.White);
			}
		}

		/// <summary>
		///		Obtiene el tamaño de fuente dependiendo de la variación
		/// </summary>
		private int GetFontSize(int variationIndex, int [] fontSizes)
		{
			if (variationIndex < fontSizes.Length)
				return fontSizes[variationIndex];
			else
				return fontSizes[fontSizes.Length - 1];
		}

		/// <summary>
		///		Obtiene el tamaño de imagen dependiendo de la variación
		/// </summary>
		private int GetImageSize(int variationIndex, int[] imageSizes)
		{
			if (variationIndex < imageSizes.Length)
				return imageSizes[variationIndex];
			else
				return imageSizes[imageSizes.Length - 1];
		}

		/// <summary>
		///		Obtiene el color de texto dependiendo de la variación
		/// </summary>
		private SolidColorBrush GetForeGround(int variationIndex, SolidColorBrush[] brushes)
		{
			if (variationIndex < brushes.Length)
				return brushes[variationIndex];
			else
				return brushes[brushes.Length - 1];
		}

		/// <summary>
		///		Ejecuta un movimiento
		/// </summary>
		private void ExecuteMovement()
		{
			Selected = true;
			PgnGameViewModel.MoveTo(this);
		}

		/// <summary>
		///		Indica si puede ejecutar el movimiento
		/// </summary>
		private bool CanExecuteMovement()
		{
			return Movement != null;
		}

		/// <summary>
		///		Tablero de juego
		/// </summary>
		private PgnGameViewModel PgnGameViewModel { get; }

		/// <summary>
		///		Movimiento
		/// </summary>
		public MovementFigureModel Movement 
		{ 
			get { return _movement; }
			set { CheckProperty(ref _movement, value); }
		}

		/// <summary>
		///		Indice de la variación
		/// </summary>
		public int VariationIndex
		{
			get { return _variationIndex; }
			set { CheckProperty(ref _variationIndex, value); }
		}

		/// <summary>
		///		Indica si está dentro de una variación
		/// </summary>
		private bool IsRecursive
		{
			get { return VariationIndex > 0; }
		}

		/// <summary>
		///		Indica si el movimiento está seleccionado
		/// </summary>
		public bool Selected
		{
			get { return _selected; }
			set 
			{ 
				if (CheckProperty(ref _selected, value))
					AssignViewSizes(VariationIndex, value);
			}
		}

		/// <summary>
		///		Número de movimiento
		/// </summary>
		public int MovementIndex
		{
			get { return _movementIndex; }
			set { CheckProperty(ref _movementIndex, value); }
		}

		/// <summary>
		///		Nombre de la pieza
		/// </summary>
		public PieceBaseModel.PieceType Piece 
		{ 
			get { return _piece; }
			set { CheckProperty(ref _piece, value); }
		}

		/// <summary>
		///		Color de la pieza
		/// </summary>
		public PieceBaseModel.PieceColor Color
		{
			get { return _color; }
			set { CheckProperty(ref _color, value); }
		}

		/// <summary>
		///		Texto del movimiento
		/// </summary>
		public string Text
		{
			get { return _text; }
			set { CheckProperty(ref _text, value); }
		}

		/// <summary>
		///		Indica si el movimiento tiene una variación
		/// </summary>
		public bool HasVariation
		{
			get { return _hasVariation; }
			set { CheckProperty(ref _hasVariation, value); }
		}

		/// <summary>
		///		Tiempo del movimiento
		/// </summary>
		public string Time
		{
			get { return _time; }
			set { CheckProperty(ref _time, value); }
		}

		/// <summary>
		///		Tamaño de la fuente
		/// </summary>
		public int FontSize
		{
			get { return _fontSize; }
			set { CheckProperty(ref _fontSize, value); }
		}

		/// <summary>
		///		Ancho de la imagen
		/// </summary>
		public int ImageWidth
		{
			get { return _imageWidht; }
			set { CheckProperty(ref _imageWidht, value); }
		}

		/// <summary>
		///		Color de texto
		/// </summary>
		public SolidColorBrush Foreground
		{ 
			get { return _foreGround; }
			set { CheckObject(ref _foreGround, value); }
		}

		/// <summary>
		///		Color de fondo
		/// </summary>
		public SolidColorBrush Background
		{ 
			get { return _backGround; }
			set { CheckObject(ref _backGround, value); }
		}

		/// <summary>
		///		Comando para selección del movimiento
		/// </summary>
		public BaseCommand SelectMovementCommand { get; }
	}
}