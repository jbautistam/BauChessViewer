using System;
using System.Windows.Media;

using Bau.Libraries.LibChessGame.Movements;
using Bau.Libraries.LibChessGame.Pieces;

namespace BauChessViewer.ViewModels.Movements
{
	/// <summary>
	///		ViewModel del movimiento de una figura
	/// </summary>
	public class MovementFigureViewModel : BaseMovementViewModel
	{
		// Variables privadas
		private MovementFigureModel _blackMovement, _whiteMovement;
		private PieceBaseModel.PieceType _piece;
		private PieceBaseModel.PieceColor _color;
		private int _movementIndex;
		private string _movementNumber, _whiteMovementText, _blackMovementText, _time;
		private SolidColorBrush _foregroud;

		public MovementFigureViewModel(MovementFigureModel movement, int movementIndex)
		{
			// Inicializa las propiedades
			Piece = movement.OriginPiece;
			Color = movement.Color;
			MovementIndex = movementIndex;
			if (Color == PieceBaseModel.PieceColor.White)
			{
				WhiteMovement = movement;
				WhiteMovementText = movement.Text;
				MovementNumber = $"{movementIndex / 2 + 1}. ";
			}
			else
			{
				BlackMovement = movement;
				BlackMovementText = movement.Text;
			}
			Time = "01:07";
			// Inicializa los objetos adicionales
			Foreground = new SolidColorBrush(Colors.Black);
		}

		/// <summary>
		///		Movimiento
		/// </summary>
		public MovementFigureModel Movement 
		{ 
			get 
			{ 
				if (BlackMovement == null)
					return _whiteMovement; 
				else
					return _blackMovement;
			}
		}

		/// <summary>
		///		Movimiento
		/// </summary>
		public MovementFigureModel BlackMovement 
		{ 
			get { return _blackMovement; }
			set { CheckProperty(ref _blackMovement, value); }
		}

		/// <summary>
		///		Movimiento
		/// </summary>
		public MovementFigureModel WhiteMovement 
		{ 
			get { return _whiteMovement; }
			set { CheckProperty(ref _whiteMovement, value); }
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
		///		Texto con el número de movimiento (sólo para las blancas)
		/// </summary>
		public string MovementNumber
		{
			get { return _movementNumber; }
			set { CheckProperty(ref _movementNumber, value); }
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
		///		Texto del movimiento blanco
		/// </summary>
		public string WhiteMovementText
		{
			get { return _whiteMovementText; }
			set { CheckProperty(ref _whiteMovementText, value); }
		}

		/// <summary>
		///		Texto del movimiento negro
		/// </summary>
		public string BlackMovementText
		{
			get { return _blackMovementText; }
			set { CheckProperty(ref _blackMovementText, value); }
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
			get { return _foregroud; }
			set { CheckObject(ref _foregroud, value); }
		}
	}
}