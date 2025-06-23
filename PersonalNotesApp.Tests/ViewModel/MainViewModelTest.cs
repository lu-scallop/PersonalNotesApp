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
			ObservableCollection<Base> Pastas = new ObservableCollection<Base>();
			string pastaTeste = "Nova Pasta";
			Pasta pasta = new Pasta(pastaTeste);

			//Act
			Pastas.Add(pasta);

			//Assert
			Assert.True(Pastas.Contains(pasta));
		}

		[Fact]
		public void AdicionaNovaPasta_NomeDaPastaVazio_DeveAdicionarPastaSemNome()
		{
			ObservableCollection<Base> Pastas = new ObservableCollection<Base>();
			string pastaTeste = string.Empty;
			Pasta pasta = new Pasta(pastaTeste);

			//Act
			Pastas.Add(pasta);

			//Assert
			Assert.True(Pastas.Contains(pasta));
			Assert.Contains(string.Empty, pastaTeste);
		}

		[Fact]
		public void AdicionaNovaPasta_NomeDaPastaComValorNulo_LancaExcecao()
		{
			string pastaTeste = null;

			Assert.Throws<ArgumentNullException>(() => new Pasta(pastaTeste));
		}

		[Fact]
		public void AdicionaSubPasta_SubPastaAdicionadaNaColecao_DeveSerAdicionadoNaColecao()
		{
			Pasta pasta = new Pasta("Pasta principal");
			string nomeSubPasta = "Nova Sub Pasta";
			Pasta subPasta = new Pasta(nomeSubPasta);

			pasta.SubPastas.Add(subPasta);

			Assert.True(pasta.SubPastas.Contains(subPasta));

		}

		[Fact]
		public void AdicionaNovaAnotacao_AnotacaoAdicionadaNaColecao_AnotacaoDeveEstarNaColecao()
		{
			ObservableCollection<Base> colecao = new ObservableCollection<Base>();
			Anotacao anotacaoTeste = new Anotacao("Nova anotacao");

			colecao.Add(anotacaoTeste);

			Assert.True(colecao.Contains(anotacaoTeste));
		}

		[Fact]
		public void AdicionaNovaAnotacao_NomeAnotacaoNulaAdicionadaNaColecao_LancaExcecao()
		{

			string nomeAnotacao = null;

			Assert.Throws<ArgumentNullException>(() => new Anotacao(nomeAnotacao));
		}

		[Fact]
		public void AdicionaNovaAnotacaoEmSubPasta_AnotacaoAdicionadaNaSubPasta_AnotacaoDeveEstarNaColecao()
		{
			Pasta pasta = new Pasta("Pasta principal");
			Anotacao anotacao = new Anotacao("Nova Anotação");

			pasta.SubPastas.Add(anotacao);

			Assert.True(pasta.SubPastas.Contains(anotacao));
		}

		[Fact]
		public void ExcluirItem_ExcluiPastaNaColecao_RemovePasta()
		{
			MainViewModel mainViewModel = new MainViewModel();
			Pasta pasta = new Pasta("Pasta");

			mainViewModel.Pastas.Add(pasta);
			mainViewModel.ExcluirItem(pasta);

			Assert.Empty(mainViewModel.Pastas);
			Assert.DoesNotContain(pasta, mainViewModel.Pastas);
		}

		[Fact]
		public void ExcluirItem_ExcluiSubPasta_RemoveSubPasta()
		{
			MainViewModel mainViewModel = new MainViewModel();
			Pasta pastaPrincipal = new Pasta("Pasta 1");
			Pasta subPasta = new Pasta("Pasta 1.1");

			mainViewModel.Pastas.Add(pastaPrincipal);
			pastaPrincipal.SubPastas.Add(subPasta);

			mainViewModel.ExcluirItem(subPasta);

			Assert.False(pastaPrincipal.SubPastas.Contains(subPasta));
			Assert.Empty(pastaPrincipal.SubPastas);
			Assert.True(mainViewModel.Pastas.Contains(pastaPrincipal));

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

			Assert.False(nivel3.SubPastas.Contains(nivel4));
			Assert.Empty(nivel3.SubPastas);

			Assert.True(mainViewModel.Pastas.Contains(pastaPrincipal));
			Assert.True(pastaPrincipal.SubPastas.Contains(nivel1));
			Assert.True(nivel1.SubPastas.Contains(nivel2));
			Assert.True(nivel2.SubPastas.Contains(nivel3));

		}

	}
}
