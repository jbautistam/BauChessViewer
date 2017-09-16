using System;

namespace Bau.Libraries.LibChessGame.Movements
{
	/// <summary>
	///		Clase con los datos de un comentario asociado a un movimiento
	/// </summary>
	public class MovementRemarksModel : MovementBaseModel
	{
		public MovementRemarksModel(string remarks)
		{
			Remarks = remarks;
		}

		/// <summary>
		///		Comentario
		/// </summary>
		public string Remarks { get; }
	}
}
