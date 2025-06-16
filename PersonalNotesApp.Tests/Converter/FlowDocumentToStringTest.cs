using PersonalNotesApp.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
			Assert.Contains("Isto é um teste!", textoConvertido);
			
		}

		[Fact]
		public void Converte_FlowDocumentParaStringVazia_RetornaStringVazia()
		{
			var flowDocument = new FlowDocument();

			var textoVazio = FlowDocumentToString.Converte(flowDocument);

			Assert.NotNull(textoVazio);
			Assert.Contains(string.Empty, textoVazio.TrimEnd());
		}

		[Fact]
		public void Converte_FlowDocumentRetornaNulo_LancaExcecao()
		{
			
			FlowDocument flowDocument = null;

			
			Assert.Throws<NullReferenceException>(() => FlowDocumentToString.Converte(flowDocument));
			
		}

		[Fact]
		public void Converte_TextoEmNegrito_RetornaTextoEmNegrito()
		{
			
			var flowDocument = new FlowDocument(new Paragraph(new Run("Texto em negrito")));
			flowDocument.FontWeight = FontWeights.Bold;

			
			var textoEstilizado = FlowDocumentToString.Converte(flowDocument);

			
			Assert.Contains("**Texto em negrito**", textoEstilizado);
			Assert.DoesNotContain("***", textoEstilizado);
			Assert.DoesNotContain("<u>", textoEstilizado);
		}

		[Fact]
		public void Converte_TextoEmItalico_RetornaTextoEmItalico()
		{
			
			var flowDocument = new FlowDocument(new Paragraph(new Run("Texto em itálico")));
			flowDocument.FontStyle = FontStyles.Italic;

			
			var textoEstilizado = FlowDocumentToString.Converte(flowDocument);

			
			Assert.Contains("*Texto em itálico*", textoEstilizado);
			Assert.DoesNotContain("**", textoEstilizado);
			Assert.DoesNotContain("<u>", textoEstilizado);

		}

		[Fact]
		public void Converte_TextoEmItalico_RetornaTextoSublinhado()
		{

			var flowDocument = new FlowDocument(new Paragraph(new Run("Texto sublinhado")
			{
				TextDecorations = TextDecorations.Underline
			}));

			var textoEstilizado = FlowDocumentToString.Converte(flowDocument);

			Assert.Contains("<u>Texto sublinhado</u>", textoEstilizado);
			Assert.DoesNotContain("**", textoEstilizado);
			Assert.DoesNotContain("*", textoEstilizado);

		}

		[Fact]
		public void Converte_CombinacaoDeNegritoItalico_RetornaTextoComNegritoItalico()
		{
			var flowDocument = new FlowDocument(new Paragraph(new Run("Texto em negrito-itálico")));
			flowDocument.FontWeight = FontWeights.Bold;
			flowDocument.FontStyle = FontStyles.Italic;

			var textoEstilizado = FlowDocumentToString.Converte(flowDocument);

			Assert.Contains("***Texto em negrito-itálico***", textoEstilizado);
			Assert.DoesNotContain("<u>", textoEstilizado);
		}
		/*
		[Fact]
		public void Converte_CombinacaoDeNegritoSublinhado_RetornaTextoComNegritoSublinhado()
		{
			var flowDocument = new FlowDocument(new Paragraph(new Run("Texto negrito e sublinhado")
			{
				FontWeight = FontWeights.Bold,
				TextDecorations = TextDecorations.Underline
			}));
			

			var textoEstilizado = FlowDocumentToString.Converte(flowDocument);

			Assert.Contains("**<u>Texto negrito e sublinhado</u>**", textoEstilizado);
			Assert.DoesNotContain($"*", textoEstilizado);
		}
		*/
		[Fact]
		public void Converte_CombinacaoDeItalicoSublinhado_RetornaTextoComItalicoSublinhado()
		{
			var flowDocument = new FlowDocument(new Paragraph(new Run("Texto itálico e sublinhado")
			{
				FontStyle = FontStyles.Italic,
				TextDecorations = TextDecorations.Underline
			}));

			var textoEstilizado = FlowDocumentToString.Converte(flowDocument);

			Assert.Contains("*<u>Texto itálico e sublinhado</u>*", textoEstilizado);
			Assert.DoesNotContain("**", textoEstilizado);
		}

		[Fact]
		public void Converte_CombinacaoTodosOsEstilos_RetornaTodosOsEstilos()
		{
			var flowDocument = new FlowDocument(new Paragraph(new Run("Texto com todos os estilos")
			{
				FontWeight = FontWeights.Bold,
				FontStyle = FontStyles.Italic,
				TextDecorations = TextDecorations.Underline
			}));

			var textoEstilizado = FlowDocumentToString.Converte(flowDocument);

			Assert.Contains("***<u>Texto com todos os estilos</u>***", textoEstilizado);
		}
		
		[Theory]
		[InlineData("Texto sem formatação")]
		[InlineData("***<u>Texto em itálico, negrito e sublinhado</u>***")]
		[InlineData("**<u>Texto em negrito e sublinhado</u>**")]
		[InlineData("*<u>Texto em itálico e sublinhado</u>*")]
		[InlineData("***Texto em itálico e negrito***")]
		[InlineData("**Texto em negrito**")]
		[InlineData("*Texto em itálico*")]
		public void Converte_VariasEstilizacoesDeTexto_AplicaEstiloNosTextos(string texto)
		{
			//Arrange
			var flowDocument = new FlowDocument(new Paragraph(new Run(texto)));

			//Act
			var textoEstilizado = FlowDocumentToString.Converte(flowDocument);

			//Assert
			Assert.Contains(texto,textoEstilizado);


		}
		
	}
}
