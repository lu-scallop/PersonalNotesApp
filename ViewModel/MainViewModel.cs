using PersonalNotesApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace PersonalNotesApp.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public string CaminhoRaiz 
        {
            get 
            {
                var caminho = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Estrutura");
                Directory.CreateDirectory(caminho);
                return caminho;
            }
        }
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
        public void Salvar(string caminhoRaiz, ObservableCollection<Base> pastas)
        {
            foreach (var item in pastas)
            {
                if (item is Pasta pasta)
                {
                    Directory.CreateDirectory(Path.Combine(caminhoRaiz, pasta.Nome));
                    Salvar(Path.Combine(caminhoRaiz, pasta.Nome), pasta.SubPastas);
                }
                else if (item is Anotacao anotacao)
                {
                    var caminhoArquivo = Path.Combine(caminhoRaiz, anotacao.Nome + ".md");
                    Console.WriteLine(caminhoArquivo);
                    File.WriteAllText(caminhoArquivo, anotacao.Texto);
                }
            }
        }


    }
}
