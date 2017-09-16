using System;
using System.Windows.Data;

using Bau.Libraries.LibChessGame.Movements;
using Bau.Libraries.LibChessGame.Pieces;

namespace BauChessViewer.Converters
{
	/// <summary>
	///		Conversor para los iconos asociados a los archivos del árbol de secciones
	/// </summary>
	public class FileIconConverter : IValueConverter
	{
		/// <summary>
		///		Convierte un tipo en un icono
		/// </summary>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{ 
			if (value is MovementFigureModel)
				return GetIcon(value as MovementFigureModel);
			else
				return null;
		}

		/// <summary>
		///		Obtiene el icono asociado a un <see cref="MovementFigureModel"/>
		/// </summary>
		private string GetIcon(MovementFigureModel movement)
		{	
			string pathImage = "/BauChessViewer;component/Resources/Images/";
			string nameColor = "";
			string namePiece = "";

				// Obtiene el nombre del color
				if (movement.Color == PieceBaseModel.PieceColor.White)
					nameColor = "White";
				else
					nameColor = "Black";
				// Obtiene el nombre de la pieza
				switch (movement.OriginPiece)
				{ 
					case PieceBaseModel.PieceType.Pawn:
							namePiece = "Pawn";
						break;
					case PieceBaseModel.PieceType.Rook:
							namePiece = "Rook";
						break;
					case PieceBaseModel.PieceType.Knight:
							namePiece = "Knight";
						break;
					case PieceBaseModel.PieceType.Bishop:
							namePiece = "Bishop";
						break;
					case PieceBaseModel.PieceType.Queen:
							namePiece = "Queen";
						break;
					case PieceBaseModel.PieceType.King:
							namePiece = "King";
						break;

				}
				// Obtiene el nombre de la imagen
				if (!string.IsNullOrEmpty(nameColor) && !string.IsNullOrEmpty(namePiece))
					return $"{pathImage}{namePiece}{nameColor}.gif";
				else
					return null;
		}

		/// <summary>
		///		Convierte un valor de vuelta
		/// </summary>
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{ 
			throw new NotImplementedException();
		}
	}
}