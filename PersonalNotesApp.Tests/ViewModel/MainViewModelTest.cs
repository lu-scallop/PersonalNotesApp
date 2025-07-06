using PersonalNotesApp.Model;
using PersonalNotesApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalNotesApp.Tests.ViewModel
{
	public class MainViewModelTest
	{
		[Fact]
		public void AdicionaNovaPasta_PastaAdicionadaNaColecao_DeveSerAdicionadoNaColecao()
		{
			//Arrange
			MainViewModel mainViewModel = new MainViewModel();

			//Act
			mainViewModel.AdicionaNovaPasta();

			//Assert
			Assert.NotEmpty(mainViewModel.Pastas);
		}

		[Fact]
		public void AdicionaNovaPasta_ComPastasExistentes_DeveGerarNomeUnico()
		{
			//Arrange
			MainViewModel mainViewModel = new MainViewModel();

			//Act
			mainViewModel.AdicionaNovaPasta();
			mainViewModel.AdicionaNovaPasta();
			mainViewModel.AdicionaNovaPasta();

			//Assert
			var nomes = mainViewModel.Pastas.Cast<Pasta>().Select(p => p.Nome).ToList();
			Assert.Contains("Nova Pasta", nomes);
			Assert.Contains("Nova Pasta (1)", nomes);
			Assert.Contains("Nova Pasta (2)", nomes);
		}

		[Fact]
		public void AdicionaSubPasta_SubPastaAdicionadaNaColecao_DeveSerAdicionadoNaColecao()
		{
			MainViewModel mainViewModel = new MainViewModel();
			Pasta pasta = new Pasta("Pasta");

			mainViewModel.AdicionaSubPasta(pasta);

			Assert.NotEmpty(pasta.SubPastas);
			Assert.Single(pasta.SubPastas);
		}

		[Fact]
		public void AdicionaSubPasta_ComPastaNula_LancaExcecao()
		{
			MainViewModel mainViewModel = new MainViewModel();

			Assert.Throws<ArgumentNullException>(() => mainViewModel.AdicionaSubPasta(null));
		}


		[Fact]
		public void AdicionaNovaAnotacao_AnotacaoAdicionadaNaColecao_DeveSerAdicionadoNaColecao()
		{
			MainViewModel mainViewModel = new MainViewModel();

			mainViewModel.AdicionaNovaAnotacao();

			Assert.NotEmpty(mainViewModel.Pastas);
			var novaAnotacao = mainViewModel.Pastas.Last() as Anotacao;
			Assert.NotNull(novaAnotacao);
			Assert.StartsWith("Nova Anotação", novaAnotacao.Nome);
		}

		[Fact]
		public void AdicionaNovaAnotacaoEmSubPasta_AnotacaoAdicionadaNaSubPasta_AnotacaoDeveEstarNaColecao()
		{
			MainViewModel mainViewModel = new MainViewModel();
			Pasta pasta = new Pasta("Pasta");
			Pasta subPasta = new Pasta("Sub Pasta");
			mainViewModel.Pastas.Add(pasta);
			pasta.SubPastas.Add(subPasta);

			mainViewModel.AdicionaNovaAnotacaoEmSubPasta(subPasta);

			Assert.NotEmpty(subPasta.SubPastas);


		}

		[Fact]
		public void ExcluirItem_ExcluiPastaNaColecao_ExcluiPasta()
		{
			
			MainViewModel mainViewModel = new MainViewModel();
			mainViewModel.Pastas.Clear();
			Pasta pasta = new Pasta("Nova Pasta");

			mainViewModel.Pastas.Add(pasta);

			mainViewModel.ExcluirItem(pasta);

			Assert.DoesNotContain(pasta, mainViewModel.Pastas);
			Assert.Empty(mainViewModel.Pastas);
			LimparDiretorio(RetornaCaminhoDosDiretoriosDeTestes());
			mainViewModel.Pastas.Clear();
		}

		[Fact]
		public void ExcluirItem_ExcluiSubPasta_ExcluiSubPasta()
		{
			MainViewModel mainViewModel = new MainViewModel();
			Pasta pasta = new Pasta("Pasta 1");
			Pasta subPasta = new Pasta("Pasta 1.1");

			mainViewModel.Pastas.Add(pasta);
			pasta.SubPastas.Add(subPasta);

			mainViewModel.ExcluirItem(subPasta);

			Assert.DoesNotContain(subPasta, pasta.SubPastas);
			Assert.Empty(pasta.SubPastas);
		}
		
		[Fact]
		public void ExcluirItem_ExcluirSubPastaEmHierarquiaProfunda_RemoveSubPasta()
		{
			MainViewModel mainViewModel = new MainViewModel();	
			Pasta pastaPrincipal = new Pasta("Pasta 1");
			var nivel1 = new Pasta("Pasta 1.1");
			var nivel2 = new Pasta("Pasta 1.2");
			var nivel3 = new Pasta("Pasta 1.3");
			var nivel4 = new Pasta("Pasta 1.4");

			mainViewModel.Pastas.Add(pastaPrincipal);
			pastaPrincipal.SubPastas.Add(nivel1);
			nivel1.SubPastas.Add(nivel2);
			nivel2.SubPastas.Add(nivel3);
			nivel3.SubPastas.Add(nivel4);

			mainViewModel.ExcluirItem(nivel4);

			Assert.DoesNotContain(nivel4, nivel3.SubPastas);
			Assert.Empty(nivel3.SubPastas);
			Assert.Contains(pastaPrincipal, mainViewModel.Pastas);
			Assert.Contains(nivel1, pastaPrincipal.SubPastas);
			Assert.Contains(nivel2, nivel1.SubPastas);
			Assert.Contains(nivel3, nivel2.SubPastas);

		}

		[Fact]
		public void ExcluirItem_ItemNulo_NaoLancaExcecao()
		{
			MainViewModel mainViewModel = new MainViewModel();

			var exception = Record.Exception(() => mainViewModel.ExcluirItem(null));
			Assert.Null(exception);
		}

		[Fact]
		public void ExcluirItem_ItemNaoExistente_NaoAlteraColecao()
		{
			MainViewModel mainViewModel = new MainViewModel();
			var pastaExistente = new Pasta("Pasta Existente");
			var pastaNaoExistente = new Pasta("Pasta Não Existente");

			mainViewModel.Pastas.Add(pastaExistente);
			int countAntes = mainViewModel.Pastas.Count;

			mainViewModel.ExcluirItem(pastaNaoExistente);

			Assert.Equal(countAntes, mainViewModel.Pastas.Count);
			Assert.True(mainViewModel.Pastas.Contains(pastaExistente));
		}

		[Fact]
		public void Salvar_CriaDiretorio_DeveSalvarPastaComoDiretorio()
		{
			MainViewModel mainViewModel = new MainViewModel();
			mainViewModel.Pastas.Clear();
			string caminho = RetornaCaminhoDosDiretoriosDeTestes();
			Pasta pasta = new Pasta("Pasta");
			mainViewModel.Pastas.Add(pasta);

			mainViewModel.Salvar(caminho, mainViewModel.Pastas);

			Assert.True(Directory.Exists(caminho));

			LimparDiretorio(caminho);
			mainViewModel.Pastas.Clear();
		}

		[Fact]
		public void MapearPastaEstruturaParaTreeView_RetornaAsPastasParaTreeView()
		{
			MainViewModel mainViewModel = new MainViewModel();
			mainViewModel.Pastas.Clear();
			string caminho = RetornaCaminhoDosDiretoriosDeTestes();
			Pasta pasta = new Pasta("PastaTeste");
			Pasta pasta2 = new Pasta("PastaTeste2");
			Anotacao anotacao = new Anotacao("Anotacao");
			Directory.CreateDirectory(caminho+@$"\{pasta}");
			Directory.CreateDirectory(caminho+@$"\{pasta2}");
			File.Create(caminho+@$"\{anotacao}.md");
			

			mainViewModel.MapearPastaEstruturaParaTreeView(caminho, mainViewModel.Pastas);

			Assert.True(Directory.Exists(caminho));
			LimparDiretorio(caminho);
			mainViewModel.Pastas.Clear();

		}

		[Fact]
		public void ItemSelecionado_AlterarValor_DeveNotificarPropertyChanged()
		{
			MainViewModel mainViewModel = new MainViewModel();
			var pasta = new Pasta("Teste");
			bool propertyChangedFired = false;

			mainViewModel.PropertyChanged += (sender, e) =>
			{
				if(e.PropertyName == nameof(MainViewModel.ItemSelecionado))
					propertyChangedFired = true;
			};

			mainViewModel.ItemSelecionado = pasta;

			Assert.True(propertyChangedFired);
			Assert.Equal(pasta, mainViewModel.ItemSelecionado);
		}

		[Fact]
		public void DocumentoSelecionado_ComAnotacaoSelecionado_DeveRetornarConteudo()
		{
			MainViewModel mainViewModel = new MainViewModel();
			var anotacao = new Anotacao("Teste");

			mainViewModel.ItemSelecionado = anotacao;

			Assert.NotNull(mainViewModel.DocumentoSelecionado);
			Assert.Equal(anotacao.Conteudo, mainViewModel.DocumentoSelecionado);
		}

		[Fact]
		public void DocumentoSelecionado_ComPastaSelecionada_DeveRetornarNulo()
		{
			MainViewModel viewModel = new MainViewModel();
			var pasta = new Pasta("Teste");

			viewModel.ItemSelecionado = pasta;

			Assert.Null(viewModel.DocumentoSelecionado);
		}

		////MÉTODOS AUXILIARES - INÍCIO
		public void LimparDiretorio(string caminho)
		{
			MainViewModel mainViewModel = new MainViewModel();
			
			string[] conteudo = Directory.GetDirectories(caminho);

            if (Directory.Exists(caminho))
            {
				try
				{
					foreach (string pastas in conteudo)
					{
						Directory.Delete(pastas, true);
						mainViewModel.Pastas.Clear();
					}
				}
				catch (IOException ex) 
				{
					Console.WriteLine(ex.Message);
				}
				    
            }
        }

		public string RetornaCaminhoDosDiretoriosDeTestes()
		{
			return @"C:\ProjetosDev\ProjetosCsharp\PersonalNotesApp\PersonalNotesApp.Tests\DiretorioParaTestes";
		}

		////MÉTODO AUXILIARES - FIM
	}
}
