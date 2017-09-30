using System;
using System.Windows.Data;

namespace BauChessViewer.Converters
{
	/// <summary>
	///		Conversor para transformar si un objeto es nulo en un enumerado "Visibility"
	/// </summary>
	public class ObjectToVisibilityConverter : IValueConverter
	{
		/// <summary>
		///		Convierte un valor boolean en un valor de FontWeight
		/// </summary>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{ 
			if (value != null)
				return System.Windows.Visibility.Visible;
			else
				return System.Windows.Visibility.Hidden;
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
