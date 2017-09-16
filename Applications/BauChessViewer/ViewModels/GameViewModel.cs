using System;
using System.Collections.ObjectModel;

using Bau.Libraries.LibChessGame.Movements;

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
		private GameBoardViewModel _gameBoard;

		public GameViewModel(GameModel game)
		{
			// Asigna el juego
			Game = game;
			GameBoard = new GameBoardViewModel(this);
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
		}

		/// <summary>
		///		Inicializa el tablero
		/// </summary>
		internal void Init()
		{
			GameBoard.Reset();
		}

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
		///		Tablero de juego
		/// </summary>
		public GameBoardViewModel GameBoard
		{
			get { return _gameBoard; }
			set { CheckObject(ref _gameBoard, value); }
		}
	}
}
