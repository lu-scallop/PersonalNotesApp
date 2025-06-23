using PersonalNotesApp.Model;
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
		public void AdicionaSubPasta_AdicionaSubPasta_DeveSerAdicionadoNaColecao()
		{
			Pasta pasta = new Pasta("Pasta principal");
			string nomeSubPasta = "Nova Sub Pasta";
			Pasta subPasta = new Pasta(nomeSubPasta);

			pasta.SubPastas.Add(subPasta);

			Assert.True(pasta.SubPastas.Contains(subPasta));

		}


	}
}
