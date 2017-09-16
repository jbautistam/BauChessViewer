using System;

namespace Bau.Libraries.LibChessGame.Movements
{
	/// <summary>
	///		Clase con los datos del resultado final de un juego
	/// </summary>
	public class MovementGameEndModel : MovementBaseModel
	{
		public MovementGameEndModel(GameModel.ResultType result)
		{
			Result = result;
		}

		/// <summary>
		///		Resultado del juego
		/// </summary>
		public GameModel.ResultType Result { get; }
	}
}
