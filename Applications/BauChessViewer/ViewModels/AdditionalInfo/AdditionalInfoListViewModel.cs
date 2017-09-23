using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Bau.Libraries.LibChessGame.Movements;

namespace BauChessViewer.ViewModels.AdditionalInfo
{
	/// <summary>
	///		ViewModel para una lista con información adicional sobre el juego
	/// </summary>
	public class AdditionalInfoListViewModel
	{
		public AdditionalInfoListViewModel(GameViewModel game)
		{
			Game = game;
			LoadInfo(game.Game);
		}

		/// <summary>
		///		Carga la información del juego
		/// </summary>
		private void LoadInfo(GameModel game)
		{
			// Limpia la información
			Items.Clear();
			// Añade la información adicional
			foreach (KeyValuePair<string, string> info in game.AdditionalInfo)
				Items.Add(new AdditionalInfoViewModel(info.Key, info.Value));
			// Añade las etiquetas
			foreach (KeyValuePair<string, string> tag in game.Tags)
				Items.Add(new AdditionalInfoViewModel(tag.Key, tag.Value));
		}

		/// <summary>
		///		ViewModel del juego
		/// </summary>
		public GameViewModel Game { get; }

		/// <summary>
		///		Elementos
		/// </summary>
		public ObservableCollection<AdditionalInfoViewModel> Items { get; } = new ObservableCollection<AdditionalInfoViewModel>();
	}
}
