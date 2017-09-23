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
		private int _movementIndex;
		private string _text, _time;
		private bool _selected;
		private SolidColorBrush _foreGround;
		private SolidColorBrush _backGround;

		public MovementFigureViewModel(GameBoardViewModel gameBoard, MovementFigureModel movement, int movementIndex)
		{
			// Inicializa las propiedades
			GameBoard = gameBoard;
			Selected = false;
			Piece = movement.OriginPiece;
			Color = movement.Color;
			MovementIndex = movementIndex;
			Movement = movement;
			Text = movement.Text;
			Time = "01:07";
			// Inicializa los objetos adicionales
			Foreground = new SolidColorBrush(Colors.Black);
			Background = new SolidColorBrush(Colors.White);
			SelectMovementCommand = new BaseCommand(parameter => ExecuteMovement(), parameter => CanExecuteMovement());
		}

		/// <summary>
		///		Ejecuta un movimiento
		/// </summary>
		private void ExecuteMovement()
		{
			Selected = true;
			GameBoard.MoveTo(this);
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
		private GameBoardViewModel GameBoard { get; }

		/// <summary>
		///		Movimiento
		/// </summary>
		public MovementFigureModel Movement 
		{ 
			get { return _movement; }
			set { CheckProperty(ref _movement, value); }
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
				{
					if (value)
					{
						Foreground = new SolidColorBrush(Colors.White);
						Background = new SolidColorBrush(Colors.Blue);
					}
					else
					{
						Foreground = new SolidColorBrush(Colors.Black);
						Background = new SolidColorBrush(Colors.White);
					}
				}
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
		///		Tiempo del movimiento
		/// </summary>
		public string Time
		{
			get { return _time; }
			set { CheckProperty(ref _time, value); }
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