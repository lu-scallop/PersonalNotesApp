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
		public void AdicionaNovaAnotacaoEmSubPasta_AnotacaoAdicionadaNaSubPasta_AnotacaoDeveEstarNaColecao()
		{
			Pasta pasta = new Pasta("Pasta principal");
			Anotacao anotacao = new Anotacao("Nova Anotação");

			pasta.SubPastas.Add(anotacao);

			Assert.Contains(anotacao, pasta.SubPastas);
		}

		[Fact]
		public void ExcluirItem_ExcluiPastaNaColecao_RemovePasta()
		{
			MainViewModel mainViewModel = new MainViewModel();
			Pasta pasta = new Pasta("Nova Pasta");

			mainViewModel.AdicionaNovaPasta();
			mainViewModel.ExcluirItem(mainViewModel.Pastas.FirstOrDefault());

			Assert.Empty(mainViewModel.Pastas);
			Assert.DoesNotContain(pasta, mainViewModel.Pastas);
		}

		[Fact]
		public void ExcluirItem_ExcluiSubPasta_RemoveSubPasta()
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
	}
}
