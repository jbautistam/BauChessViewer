using System;
using System.Collections.ObjectModel;

namespace BauChessViewer.ViewModels
{
	/// <summary>
	///		ViewModel para los combos de imágenes
	/// </summary>
	public class PathComboImagesViewModel : BaseViewModel
	{
		// Variables privadas
		private string _selectedPath, _selectedPathImages;

		public PathComboImagesViewModel(string pathBase)
		{
			PathBase = pathBase;
			Init();
		}

		/// <summary>
		///		Inicializa los elementos del viewModel
		/// </summary>
		public void Init()
		{
			// Limpia los directorios
			Paths.Clear();
			// Carga los directorios
			if (System.IO.Directory.Exists(PathBase))
				foreach (string path in System.IO.Directory.GetDirectories(PathBase))
					Paths.Add(System.IO.Path.GetFileName(path));
			Paths.Add("Predeterminado");
			// Selecciona el primer elemento
			if (Paths.Count > 0)
				SelectedPath = Paths[0];
		}

		/// <summary>
		///		Directorio base
		/// </summary>
		public string PathBase { get; }

		/// <summary>
		///		Archivos
		/// </summary>
		public ObservableCollection<string> Paths { get; } = new ObservableCollection<string>();

		/// <summary>
		///		Nombre del directorio seleccionado (sin el directorio base)
		/// </summary>
		public string SelectedPath
		{
			get { return _selectedPath; }
			set 
			{ 
				if (CheckProperty(ref _selectedPath, value))
				{
					if (!string.IsNullOrWhiteSpace(_selectedPath))
						SelectedPathImages = System.IO.Path.Combine(PathBase, _selectedPath);
				}
			}
		}

		/// <summary>
		///		Nombre completo del directorio de imágenes
		/// </summary>
		public string SelectedPathImages
		{
			get { return _selectedPathImages; }
			set { CheckProperty(ref _selectedPathImages, value); }
		}
	}
}
