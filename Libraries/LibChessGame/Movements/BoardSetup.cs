using System;
using System.Collections.Generic;

using Bau.Libraries.LibChessGame.Pieces;

namespace Bau.Libraries.LibChessGame.Movements
{
    /// <summary>
    ///		Configuración del tablero, se utiliza cuando la partida no está completa si no que se ha creado a partir de una posición inicial
    /// </summary>
    public class BoardSetup
    {
        /// <summary>
        ///		Indica si es un movimiento de blancas
        /// </summary>
        public bool IsWhiteMove { get; set; }

        /// <summary>
        ///		Indica si las blancas pueden enrocar por el lado de rey (enroque corto)
        /// </summary>
        public bool CanWhiteCastleKingSide { get; set; }

        /// <summary>
        ///		Indica si las blancas pueden enrocar por el lado de dama (enroque largo)
        /// </summary>
        public bool CanWhiteCastleQueenSide { get; set; }

        /// <summary>
        ///		Indica si las negras pueden enrocar por el lado de rey (enroque corto)
        /// </summary>
        public bool CanBlackCastleKingSide { get; set; }

        /// <summary>
        ///		Indica si las negras pueden enrocar por el lado de dama (enroque largo)
        /// </summary>
        public bool CanBlackCastleQueenSide { get; set; }

		/// <summary>
		///		Fila del peón que se puede capturar al paso (si hay alguno)
		/// </summary>
        public int? EnPassantRow { get; set; }

		/// <summary>
		///		Columna del peón que se puede capturar al paso (si hay alguno)
		/// </summary>
		public int? EnPassantColumn { get; set; }

        /// <summary>
		///		Número de movimientos (por jugador) antes de este movimiento
        /// </summary>
        public int HalfMoveClock { get; set; }

        /// <summary>
        ///		Número de movimientos total
        /// </summary>
        public int FullMoveCount { get; set; }

		/// <summary>
		///		Posición de las piezas
		/// </summary>
		public List<PieceBaseModel> Pieces { get; } = new List<PieceBaseModel>();
    }
}
