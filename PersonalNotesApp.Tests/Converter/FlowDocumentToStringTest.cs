using PersonalNotesApp.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace PersonalNotesApp.Tests.Converter
{
	public class FlowDocumentToStringTest
	{
		[Fact]
		public void Converte_FlowDocumentParaString_RetornaString()
		{
			//Arrange
			var flowDocument = new FlowDocument(new Paragraph(new Run("Isto é um teste!")));

			//Act
			var textoConvertido = FlowDocumentToString.Converte(flowDocument);

			//Assert
			Assert.NotNull(textoConvertido);
			Assert.IsType<string>(textoConvertido);
			Assert.Contains("Isso é um teste!", textoConvertido);
			
		}
	}
}
