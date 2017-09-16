using System;
using System.Windows.Media;
using Bau.Libraries.LibChessGame.Movements;
using Bau.Libraries.LibChessGame.Pieces;

namespace BauChessViewer.ViewModels
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
		private string _whiteMovementText, _blackMovementText, _time;
		private SolidColorBrush _background, _foregroud;

		public MovementFigureViewModel(MovementFigureModel movement)
		{
			// Inicializa las propiedades
			Piece = movement.OriginPiece;
			Color = movement.Color;
			if (Color == PieceBaseModel.PieceColor.White)
			{
				WhiteMovement = movement;
				WhiteMovementText = movement.Text;
			}
			else
			{
				BlackMovement = movement;
				BlackMovementText = movement.Text;
			}
			Time = "01:07";
			// Inicializa los objetos adicionales
			Background = new SolidColorBrush(Colors.White);
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
		///		Color de fondo
		/// </summary>
		public SolidColorBrush Background 
		{ 
			get { return _background; }
			set { CheckObject(ref _background, value); }
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