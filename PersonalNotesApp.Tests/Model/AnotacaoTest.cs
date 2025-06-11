using PersonalNotesApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Xunit;

namespace PersonalNotesApp.Tests.Model
{
	public class AnotacaoTest
	{
		[Fact]
		public void Construtor_DeveInicializarPropriedadesCorretamente()
		{
			//Arrange
			string nomeEsperado = "Meu primeiro teste";

			//Act
			var anotacao = new Anotacao(nomeEsperado);

			//Assert
			Assert.Equal(nomeEsperado, anotacao.Nome);
			Assert.NotNull(anotacao.Conteudo);
			Assert.IsType<FlowDocument>(anotacao.Conteudo);	
		}
	}
}
