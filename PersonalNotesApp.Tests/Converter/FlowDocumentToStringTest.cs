using PersonalNotesApp.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace PersonalNotesApp.Tests.Converter
{
	public class FlowDocumentToStringTest
	{
		[Fact]
		public void Converte_TextoComEstiloRegular_RetornaString()
		{
			var flowDocument = new FlowDocument(new Paragraph(new Run("Isto é um teste!")));

			var textoConvertido = FlowDocumentToString.Converte(flowDocument);

			Assert.NotNull(textoConvertido);
			Assert.IsType<string>(textoConvertido);
			Assert.Contains("Isto é um teste!", textoConvertido);
			Assert.False(VerificaFormatacaoItalico(textoConvertido));
			Assert.False(VerificaFormatacaoNegrito(textoConvertido));
			Assert.False(VerificaFormatacaoSublinhado(textoConvertido));

			
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
			Assert.False(VerificaFormatacaoItalico(textoEstilizado));
			Assert.False(VerificaFormatacaoSublinhado(textoEstilizado));
		}

		[Fact]
		public void Converte_TextoEmItalico_RetornaTextoEmItalico()
		{
			
			var flowDocument = new FlowDocument(new Paragraph(new Run("Texto em itálico")));
			flowDocument.FontStyle = FontStyles.Italic;

			
			var textoEstilizado = FlowDocumentToString.Converte(flowDocument);

			
			Assert.Contains("*Texto em itálico*", textoEstilizado);
			Assert.False(VerificaFormatacaoNegrito(textoEstilizado));
			Assert.False(VerificaFormatacaoSublinhado(textoEstilizado));

		}

		[Fact]
		public void Converte_TextoSublinhado_RetornaTextoSublinhado()
		{

			var flowDocument = new FlowDocument(new Paragraph(new Run("Texto sublinhado")
			{
				TextDecorations = TextDecorations.Underline
			}));

			var textoEstilizado = FlowDocumentToString.Converte(flowDocument);

			Assert.Contains("<u>Texto sublinhado</u>", textoEstilizado);
			Assert.False(VerificaFormatacaoNegritoItalico(textoEstilizado));

		}

		[Fact]
		public void Converte_CombinacaoDeNegritoItalico_RetornaTextoComNegritoItalico()
		{
			var flowDocument = new FlowDocument(new Paragraph(new Run("Texto em negrito-itálico")));
			flowDocument.FontWeight = FontWeights.Bold;
			flowDocument.FontStyle = FontStyles.Italic;

			var textoEstilizado = FlowDocumentToString.Converte(flowDocument);

			Assert.Contains("***Texto em negrito-itálico***", textoEstilizado);
			Assert.False(VerificaFormatacaoSublinhado(textoEstilizado));
		}
		
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
			Assert.False(VerificaFormatacaoItalico(textoEstilizado));
		}
		
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
			Assert.False(VerificaFormatacaoNegrito(textoEstilizado));
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
		[InlineData("**Texto em negrito**")]
		[InlineData("*Texto em itálico*")]
		[InlineData("<u>Texto sublinhado</u>")]
		public void ConverteReconverte_TesteDeIdaEVolta_DeveManterFormatacao(string textoMarkdown)
		{
			//Arrange
			var flowDocument = FlowDocumentToString.ConverteDeVolta(textoMarkdown);


			//Act
			var textoConvertidoEmString = FlowDocumentToString.Converte(flowDocument);
			var textoReconvertidoParaFlowDocument = FlowDocumentToString.ConverteDeVolta(textoConvertidoEmString);
			

			//Assert
			var textoFinal = FlowDocumentToString.Converte(textoReconvertidoParaFlowDocument);

			var textoConvertidoLimpo = textoConvertidoEmString.Trim();
			var textoFinalLimpo = textoFinal.Trim();

			Assert.Equal(textoConvertidoLimpo, textoFinalLimpo);

		}

		[Fact]
		public void ConverteDeVolta_StringVazia_DeveRetornarFlowDocumentVazio()
		{
			string texto = string.Empty;

			FlowDocument flowDocument = FlowDocumentToString.ConverteDeVolta(texto);
			Paragraph paragraph = flowDocument.Blocks.FirstOrDefault() as Paragraph;

			Assert.NotNull(flowDocument);
			Assert.Single(flowDocument.Blocks);
		}

		[Fact]
		public void ConverteDeVolta_TextoEmNegrito()
		{
			string texto = "**Texto em negrito**";

			var flowDocument = FlowDocumentToString.ConverteDeVolta(texto);
			var paragrafo = flowDocument.Blocks.FirstOrDefault() as Paragraph;
			var run = paragrafo.Inlines.FirstOrDefault() as Run;

			Assert.NotNull(run);
			Assert.Equal("Texto em negrito", run.Text);
			Assert.Equal(FontWeights.Bold, run.FontWeight);
			Assert.Equal(FontStyles.Normal, run.FontStyle);
			Assert.Empty(run.TextDecorations);
		}

		[Fact]
		public void ConverteDeVolta_TextoEmItalic()
		{
			string texto = "*Texto em itálico*";

			var flowDocument = FlowDocumentToString.ConverteDeVolta(texto);
			var paragrafo = flowDocument.Blocks.FirstOrDefault() as Paragraph;
			var run = paragrafo.Inlines.FirstOrDefault() as Run;

			Assert.NotNull(run);
			Assert.Equal("Texto em itálico", run.Text);
			Assert.Equal(FontStyles.Italic, run.FontStyle);
			Assert.Equal(FontWeights.Normal, run.FontWeight);
			Assert.Empty(run.TextDecorations);
		}
		[Fact]
		public void ConverteDeVolta_TextoSublinhado()
		{
			string texto = "<u>Texto sublinhado</u>";

			var flowDocument = FlowDocumentToString.ConverteDeVolta(texto);
			var paragrafo = flowDocument.Blocks.FirstOrDefault() as Paragraph;
			var run = paragrafo.Inlines.FirstOrDefault() as Run;

			Assert.NotNull(run);
			Assert.Equal("Texto sublinhado", run.Text);
			Assert.Equal(FontWeights.Normal, run.FontWeight);
			Assert.Equal(FontStyles.Normal, run.FontStyle);
			Assert.Equal(TextDecorations.Underline, run.TextDecorations);
		}

		[Fact]
		public void ConverteDeVolta_TextoNegritoItalico()
		{
			string texto = "***Texto negrito & itálico***";

			var flowDocument = FlowDocumentToString.ConverteDeVolta(texto);
			var paragrafo = flowDocument.Blocks.FirstOrDefault() as Paragraph;
			var run = paragrafo.Inlines.FirstOrDefault() as Run;

			Assert.NotNull(run);
			Assert.Equal("Texto negrito & itálico", run.Text);
			Assert.Equal(FontWeights.Bold, run.FontWeight);
			Assert.Equal(FontStyles.Italic, run.FontStyle);
			Assert.Empty(run.TextDecorations);

			
		}

		[Fact]
		public void ConverteDeVolta_TextoNegritoSublinhado()
		{
			string texto = "**<u>Texto negrito & sublinhado</u>**";

			var flowDocument = FlowDocumentToString.ConverteDeVolta(texto);
			var paragrafo = flowDocument.Blocks.FirstOrDefault() as Paragraph;
			var run = paragrafo.Inlines.FirstOrDefault() as Run;

			Assert.NotNull(run);
			Assert.Equal("Texto negrito & sublinhado", run.Text);
			Assert.Equal(FontWeights.Bold, run.FontWeight);
			Assert.Equal(TextDecorations.Underline, run.TextDecorations);
		}

		[Fact]
		public void ConverteDeVolta_TextoItalicoSublinhado()
		{
			string texto = "*<u>Texto itálico & sublinhado</u>*";

			var flowDocument = FlowDocumentToString.ConverteDeVolta(texto);
			var paragrafo = flowDocument.Blocks.FirstOrDefault() as Paragraph;
			var run = paragrafo.Inlines.FirstOrDefault() as Run;

			Assert.NotNull(run);
			Assert.Equal("Texto itálico & sublinhado", run.Text);
			Assert.Equal(FontStyles.Italic, run.FontStyle);
			Assert.Equal(TextDecorations.Underline, run.TextDecorations);
		}

		[Fact]
		public void ConverteDeVolta_CombinacaoTodosOsEstilos_RetornaTodosOsEstilos()
		{
			string texto = "***<u>Texto com todos os estilos</u>***";

			var flowDocument = FlowDocumentToString.ConverteDeVolta(texto);
			var paragrafo = flowDocument.Blocks.FirstOrDefault() as Paragraph;
			var run = paragrafo.Inlines.FirstOrDefault() as Run;

			Assert.NotNull(run);
			Assert.Equal("Texto com todos os estilos", run.Text);
			Assert.Equal(FontWeights.Bold, run.FontWeight);
			Assert.Equal(FontStyles.Italic, run.FontStyle);
			Assert.Equal(TextDecorations.Underline, run.TextDecorations);
		}


		////MÉTODOS AUXILIARES - COMEÇO

		public bool VerificaFormatacaoItalico(string texto)
		{
			var regexItalico = new Regex(@"(?<!\*)\*(?!\*).*?(?<!\*)\*(?!\*)");
			return regexItalico.IsMatch(texto);
		}

		public bool VerificaFormatacaoNegrito(string texto)
		{
			var regexNegrito = new Regex(@"(?<!\*)\*\*(?!\*).*?(?<!\*)\*\*(?!\*)");
			return regexNegrito.IsMatch(texto);
		}

		public bool VerificaFormatacaoSublinhado(string texto)
		{
			return texto.Contains("<u>") && texto.Contains("</u>");
		}

		public bool VerificaFormatacaoNegritoItalico(string texto)
		{
			var regexNegritoItalico = new Regex(@"\*\*\*.*?\*\*\*");
			return regexNegritoItalico.IsMatch(texto);
		}


		////MÉTODOS AUXILIARES - FIM
	}
}
