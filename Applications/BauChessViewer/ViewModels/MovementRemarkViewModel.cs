using System;

using Bau.Libraries.LibChessGame.Movements;

namespace BauChessViewer.ViewModels
{
	/// <summary>
	///		ViewModel para un movimiento de comentario
	/// </summary>
	public class MovementRemarkViewModel : BaseMovementViewModel
	{
		// Variables privadas
		private string _text;

		public MovementRemarkViewModel(MovementRemarksModel movement)
		{
			Movement = movement;
			Text = movement.Remarks;
		}

		/// <summary>
		///		Movimiento
		/// </summary>
		public MovementRemarksModel Movement { get; }

		/// <summary>
		///		Texto del comentario
		/// </summary>
		public string Text
		{
			get { return _text; }
			set { CheckProperty(ref _text, value); }
		}
	}
}
