using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Bau.Libraries.LibChessGame.Games;

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
				Add(info.Key, info.Value);
			// Añade las etiquetas
			foreach (KeyValuePair<string, string> tag in game.Tags)
				Add(tag.Key, tag.Value);
				
		}

		/// <summary>
		///		Añade un elemento
		/// </summary>
		private void Add(string header, string text)
		{
			if (!Exists(header, text))
				Items.Add(new AdditionalInfoViewModel(header, text));
		}

		/// <summary>
		///		Comprueba si existe un elemento en la lista
		/// </summary>
		private bool Exists(string header, string text)
		{
			// Busca el elemento en la lista
			if (!string.IsNullOrWhiteSpace(header))
				foreach (AdditionalInfoViewModel item in Items)
					if (header.Equals(item.Header, StringComparison.CurrentCultureIgnoreCase))
						return true;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return false;
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
