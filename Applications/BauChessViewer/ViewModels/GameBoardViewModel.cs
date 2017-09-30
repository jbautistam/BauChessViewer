using System;

using Bau.Libraries.LibChessGame.Board.Movements;
using BauChessViewer.ViewModels.Movements;

namespace BauChessViewer.ViewModels
{
	/// <summary>
	///		ViewModel del tablero de juego con la partida
	/// </summary>
	public class GameBoardViewModel : BaseViewModel
	{
		public GameBoardViewModel(PgnGameViewModel pgnGameViewModel)
		{
			PgnGameViewModel = pgnGameViewModel;
		}

		/// <summary>
		///		Juego
		/// </summary>
		internal PgnGameViewModel PgnGameViewModel { get; }
	}
}
