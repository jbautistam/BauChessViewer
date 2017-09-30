using System;
using System.Windows;
using System.Windows.Controls;

namespace BauChessViewer.Views.Controls
{
	/// <summary>
	///		View para una lista de movimientos
	/// </summary>
	public partial class MovementsListView : UserControl
	{
		// Propiedades
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ViewModels.PgnGameViewModel),
																								  typeof(MovementsListView), new PropertyMetadata(null));
		// Variables privadas
		private ViewModels.PgnGameViewModel _pgnGameViewModel;

		public MovementsListView()
		{
			InitializeComponent();
		}

		/// <summary>
		///		ViewModel
		/// </summary>
		public ViewModels.PgnGameViewModel ViewModel 
		{ 
			get { return _pgnGameViewModel; }
			set 
			{
				if (!ReferenceEquals(_pgnGameViewModel, value))
					DataContext = _pgnGameViewModel = value;
			}
		}

		private void lstMovements_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sender is ListBox lstView && (DataContext as ViewModels.PgnGameViewModel)?.MovementsList?.SelectedMovement != null)
				lstView.ScrollIntoView((DataContext as ViewModels.PgnGameViewModel).MovementsList.SelectedMovement);
		}
	}
}
