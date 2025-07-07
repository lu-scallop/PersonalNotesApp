using PersonalNotesApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PersonalNotesApp.Tests.Model
{
	public class PastaTest
	{
		[Fact]
		public void Construtor_DeveInicializarPropriedadesCorretamente()
		{
			//Arrange - Define classes, métodos, variáveis
			string nomeEsperado = "Pasta Teste";


			//Act - Executa a função
			var pasta = new Pasta(nomeEsperado);


			//Assert - Verifica resultado esperado
			Assert.Equal(nomeEsperado, pasta.Nome);
			Assert.NotNull(pasta.Nome);
		}
	}
}
