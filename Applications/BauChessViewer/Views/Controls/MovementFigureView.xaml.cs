using System;
using System.Windows;
using System.Windows.Controls;

namespace BauChessViewer.Views.Controls
{
	/// <summary>
	///		Control que muestra el icono y el texto de un movimiento
	/// </summary>
	public partial class MovementFigureView : UserControl
	{
		// Propiedades
		public static readonly DependencyProperty MovementProperty = DependencyProperty.Register(nameof(Movement), typeof(ViewModels.Movements.MovementFigureViewModel),
																								  typeof(MovementFigureView), new PropertyMetadata(null));

		public MovementFigureView()
		{
			InitializeComponent();
			grdMovement.DataContext = this;
		}

		/// <summary>
		///		ViewModel
		/// </summary>
		public ViewModels.Movements.MovementFigureViewModel Movement 
		{ 
			get { return (ViewModels.Movements.MovementFigureViewModel) GetValue(MovementProperty); }
			set { SetValue(MovementProperty, value); }
		}
	}
}
