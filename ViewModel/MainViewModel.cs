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
using System.Windows.Documents;

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

        public FlowDocument DocumentoSelecionado => 
            (ItemSelecionado as Anotacao)?.Conteudo;
        public Anotacao? AnotacaoSelecionada => ItemSelecionado as Anotacao;
        public Base _selecionado;
        public Base ItemSelecionado 
        {   get => _selecionado;
            set 
            {
                _selecionado = value;
                OnPropertyChanged(nameof(ItemSelecionado));
				OnPropertyChanged(nameof(DocumentoSelecionado));
			} 
        }

        public MainViewModel()
        {
            Pastas = new ObservableCollection<Base>();
			Pastas.Clear();
			MapearPastaEstruturaParaTreeView(CaminhoRaiz, Pastas);

		}
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AdicionaNovaPasta()
        {
            string nomeUnico = ObterNomeUnico("Nova Pasta", Pastas);
            var pasta = new Pasta(nomeUnico);
            Pastas.Add(pasta);
        }

		public void AdicionaSubPasta(Pasta pastaSelecionada)
		{
            if (pastaSelecionada == null) return;
            string nomeUnico = ObterNomeUnico("NovaPasta", pastaSelecionada.SubPastas);
            pastaSelecionada.SubPastas.Add(new Pasta(nomeUnico));
		}

        private string ObterNomeUnico(string nomeBase,ObservableCollection<Base> colecao)
        {
            string nomeFinal = nomeBase;
            int contador = 1;

            while (colecao.Any(item => item.Nome == nomeFinal))
            {
                nomeFinal = $"{nomeBase} ({contador++})";
            }

            return nomeFinal;
        }

		public void AdicionaNovaAnotacao()
        {
            string nomeUnico = ObterNomeUnico("Nova Anotação", Pastas);
            var anotacao = new Anotacao(nomeUnico);
            Pastas.Add(anotacao);
        }
        public void AdicionaNovaAnotacaoEmSubPasta(Pasta pastaSelecionada)
        {
            if (pastaSelecionada == null) return;
            string nomeUnico = ObterNomeUnico("Nova Anotação", pastaSelecionada.SubPastas);
            pastaSelecionada.SubPastas.Add(new Anotacao(nomeUnico));
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
                    string textoParaSalvar = Converter.FlowDocumentToString.Converte(anotacao.Conteudo);
                    File.WriteAllText(caminhoArquivo, textoParaSalvar);
                }
            }
        }
        public void MapearPastaEstruturaParaTreeView(string caminhoRaiz, ObservableCollection<Base> colecao)
        {
            try
            {
				foreach (string diretorio in Directory.GetDirectories(caminhoRaiz))
				{
                    var nomePasta = Path.GetFileName(diretorio);
                    var pasta = new Pasta(nomePasta);

                    colecao.Add(pasta);

                    MapearPastaEstruturaParaTreeView(diretorio, pasta.SubPastas);
                }
                foreach (string arquivos in Directory.GetFiles(caminhoRaiz, "*.md"))
                {
                    var nomeArquivo = Path.GetFileNameWithoutExtension(arquivos);
                    var textoLido = File.ReadAllText(arquivos);

                    var conteudoDocumento = Converter.FlowDocumentToString.ConverteDeVolta(textoLido);

                    var anotacao = new Anotacao(nomeArquivo)
                    {
                        Conteudo = conteudoDocumento
                    };

                    colecao.Add(anotacao);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
            }
            
        }


    }
}
