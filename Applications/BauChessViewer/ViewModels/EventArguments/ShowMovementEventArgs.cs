using System;

namespace BauChessViewer.ViewModels.EventArguments
{
	/// <summary>
	///		Argumentos del evento de mostrar movimientos
	/// </summary>
	public class ShowMovementEventArgs : EventArgs
	{
		public ShowMovementEventArgs(bool showAnimation)
		{
			ShowAnimation = showAnimation;
		}

		/// <summary>
		///		Indica si se debe mostrar la animación
		/// </summary>
		public bool ShowAnimation { get; }
	}
}
