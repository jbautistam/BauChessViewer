using System;

using Bau.Libraries.LibChessGame.Games;

namespace BauChessViewer.ViewModels
{
	/// <summary>
	///		ViewModel con los datos de un juego
	/// </summary>
	public class GameViewModel : BaseViewModel
	{
		// Variables privadas
		private string _event, _round, _site, _whitePlayer, _blackPlayer, _title;
		private string _date;
		private AdditionalInfo.AdditionalInfoListViewModel _informationList;

		public GameViewModel(PgnGameViewModel chessGameViewModel, GameModel game)
		{
			// Asigna el juego
			TopViewModel = chessGameViewModel;
			Game = game;
			// Carga los datos
			if (string.IsNullOrEmpty(game.Event))
				Event = "Sin evento definido";
			else
				Event = game.Event;
			Round = game.Round;
			Site = game.Site;
			WhitePlayer = game.WhitePlayer;
			BlackPlayer = game.BlackPlayer;
			if (game.Year != null && game.Month != null && game.Day != null)
				Date = $"{game.Year}-{game.Month}-{game.Day}";
			else if (game.Month != null && game.Day != null)
				Date = $"{game.Month}-{game.Day}";
			Title = $"{Event}/{Round}: {WhitePlayer} - {BlackPlayer}";
			// Carga la información adicional
			InformationList = new AdditionalInfo.AdditionalInfoListViewModel(this);
		}

		/// <summary>
		///		ViewModel principal
		/// </summary>
		public PgnGameViewModel TopViewModel { get; }

		/// <summary>
		///		Juego
		/// </summary>
		public GameModel Game { get; }

		/// <summary>
		///		Evento
		/// </summary>
		public string Event
		{
			get { return _event; }
			set { CheckProperty(ref _event, value); }
		}

		/// <summary>
		///		Ronda
		/// </summary>
		public string Round
		{
			get { return _round; }
			set { CheckProperty(ref _round, value); }
		}

		/// <summary>
		///		Sitio
		/// </summary>
		public string Site
		{
			get { return _site; }
			set { CheckProperty(ref _site, value); }
		}

		/// <summary>
		///		Jugador de blancas
		/// </summary>
		public string WhitePlayer
		{
			get { return _whitePlayer; }
			set { CheckProperty(ref _whitePlayer, value); }
		}

		/// <summary>
		///		Jugador de negras
		/// </summary>
		public string BlackPlayer
		{
			get { return _blackPlayer; }
			set { CheckProperty(ref _blackPlayer, value); }
		}

		/// <summary>
		///		Fecha
		/// </summary>
		public string Date
		{
			get { return _date; }
			set { CheckProperty(ref _date, value); }
		}

		/// <summary>
		///		Título
		/// </summary>
		public string Title
		{
			get { return _title; }
			set { CheckProperty(ref _title, value); }
		}

		/// <summary>
		///		ViewModel de información adicional
		/// </summary>
		public AdditionalInfo.AdditionalInfoListViewModel InformationList
		{
			get { return _informationList; }
			set { CheckObject(ref _informationList, value); }
		}
	}
}
