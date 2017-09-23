using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibChessGame.Games
{
	/// <summary>
	///		Clase con los datos de un juego
	/// </summary>
	public class GameModel
	{
		/// <summary>
		///		Resultado del juego
		/// </summary>
		public enum ResultType
		{
			/// <summary>Ganan las blancas</summary>
			WhiteWins,
			/// <summary>Ganan las negras</summary>
			BlackWins,
			/// <summary>Empate</summary>
			Draw,
			/// <summary>Juego no finalizado</summary>
			Open
		}

		/// <summary>
		///		Evento
		/// </summary>
		public string Event { get; internal set; }

		/// <summary>
		///		Ronda
		/// </summary>
		public string Round { get; internal set; }

		/// <summary>
		///		Lugar
		/// </summary>
		public string Site { get; internal set; }

		/// <summary>
		///		Nombre del jugador de blancas
		/// </summary>
		public string WhitePlayer { get; internal set; }

		/// <summary>
		///		Nombre del jugador de negras
		/// </summary>
		public string BlackPlayer { get; internal set; }

		/// <summary>
		///		Estado inicial del tablero
		/// </summary>
		public Board.BoardSetup Board { get; } = new Board.BoardSetup();

		/// <summary>
		///		Resultado
		/// </summary>
		public ResultType Result { get; internal set; }

		/// <summary>
		///		Año
		/// </summary>
		public int? Year { get; internal set; }

		/// <summary>
		///		Mes
		/// </summary>
		public int? Month { get; internal set; }

		/// <summary>
		///		Día
		/// </summary>
		public int? Day { get; internal set; }

		/// <summary>
		///		Información adicional
		/// </summary>
		public List<KeyValuePair<string, string>> AdditionalInfo { get; } = new List<KeyValuePair<string, string>>();

		/// <summary>
		///		Etiquetas
		/// </summary>
		public List<KeyValuePair<string, string>> Tags { get; } = new List<KeyValuePair<string, string>>();

		/// <summary>
		///		Movimientos
		/// </summary>
		public Board.Movements.MovementModelCollection Movements { get; } = new Board.Movements.MovementModelCollection();

		/// <summary>
		///		Error de interpretación
		/// </summary>
		public string ParseError { get; internal set; }
	}
}
