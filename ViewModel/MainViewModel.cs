using PersonalNotesApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PersonalNotesApp.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<Base> Pastas { get; set; }
        public Anotacao? AnotacaoSelecionada => ItemSelecionado as Anotacao;
        public Base _selecionado;
        public Base ItemSelecionado 
        {   get => _selecionado;
            set 
            {
                _selecionado = value;
                OnPropertyChanged(nameof(ItemSelecionado));
            } 
        }

        public MainViewModel()
        {
            Pastas = new ObservableCollection<Base>();


		}
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AdicionaNovaPasta()
        {
            var pasta = new Pasta("Nova Pasta");
            Pastas.Add(pasta);
        }

		public void AdicionaSubPasta(Pasta pastaSelecionada)
		{
            pastaSelecionada.SubPastas.Add(new Pasta("Nova Pasta"));
		}

		public void AdicionaNovaAnotacao()
        {
            var anotacao = new Anotacao("Nova Anotação");
            Pastas.Add(anotacao);
        }
        public void AdicionaNovaAnotacaoEmSubPasta(Pasta pastaSelecionada)
        {
            pastaSelecionada.SubPastas.Add(new Anotacao("Nova Anotação"));
        }


    }
}
