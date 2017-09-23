using System;

namespace Bau.Libraries.LibChessGame.Board.Movements
{
	/// <summary>
	///		Clase con los datos del resultado final de un juego
	/// </summary>
	public class MovementGameEndModel : MovementBaseModel
	{
		public MovementGameEndModel(Games.GameModel.ResultType result)
		{
			Result = result;
		}

		/// <summary>
		///		Resultado del juego
		/// </summary>
		public Games.GameModel.ResultType Result { get; }
	}
}
