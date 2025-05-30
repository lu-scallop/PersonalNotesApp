using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalNotesApp.Model
{
    public class Base : INotifyPropertyChanged
    {
		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged(string propertyName) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		private bool _editaNome;

		
		public bool EditaNome
		{
			get => _editaNome;
			set
			{
				_editaNome = value;
				OnPropertyChanged(nameof(EditaNome));
			}
		}
		
		private string _nome = string.Empty;
		public string Nome 
		{
			get => _nome;
			set
			{
                if (_nome != value)
                {
					_nome = value;
					OnPropertyChanged(nameof(Nome));
                }
            }
		}


	}
}
