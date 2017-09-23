using System;

namespace BauChessViewer.ViewModels.AdditionalInfo
{
	/// <summary>
	///		ViewModel para los elementos de <see cref="AdditionalInfoListViewModel"/>
	/// </summary>
	public class AdditionalInfoViewModel : BaseViewModel
	{
		// Variables privadas
		private string _header, _text;

		public AdditionalInfoViewModel(string header, string text)
		{
			Header = header;
			Text = text;
		}

		/// <summary>
		///		Cabecera
		/// </summary>
		public string Header
		{
			get { return _header; }
			set { CheckProperty(ref _header, value); }
		}

		/// <summary>
		///		Texto
		/// </summary>
		public string Text
		{
			get { return _text; }
			set { CheckProperty(ref _text, value); }
		}
	}
}
