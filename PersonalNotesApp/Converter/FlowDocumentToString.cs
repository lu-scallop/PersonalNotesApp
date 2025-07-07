using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace PersonalNotesApp.Converter
{
	public class FlowDocumentToString
	{
		public static string Converte(FlowDocument documento)
		{
			var markdownBuilder = new StringBuilder();

			foreach (var block in documento.Blocks)
			{
                if (block is Paragraph paragraph)
                {
					bool temAlinhamento = paragraph.TextAlignment != TextAlignment.Left;
					string tagAbertura = "";
					string tagFechamento = "";

					if(temAlinhamento)
					{
						switch (paragraph.TextAlignment)
						{
							case TextAlignment.Right:
								tagAbertura = "<div align=\"right\">";
								tagFechamento = "</div>";
								break;
							case TextAlignment.Center:
								tagAbertura = "<div align=\"center\">";
								tagFechamento = "</div>";
								break;
							case TextAlignment.Justify:
								tagAbertura = "<div align=\"justify\">";
								tagFechamento = "</div>";
								break;
						}
					}

					if (temAlinhamento) markdownBuilder.Append(tagAbertura); 


                    foreach (var inline in paragraph.Inlines)
                    {
                        if (inline is Run run)
                        {
							string conteudo = run.Text;
							bool ehUnderline = run.TextDecorations.Contains(TextDecorations.Underline[0]);
							bool ehNegrito = run.FontWeight == FontWeights.Bold;
							bool ehItalico = run.FontStyle == FontStyles.Italic;


							if (ehNegrito && ehItalico && ehUnderline) 
							{ 
								conteudo = $"***<u>{conteudo}</u>***"; 
							}

							else if (ehUnderline && ehNegrito)
							{
								conteudo = $"**<u>{conteudo}</u>**";
							}

							else if (ehItalico && ehUnderline)
							{
								conteudo = $"*<u>{conteudo}</u>*";
							}

							else if (ehNegrito && ehItalico)
							{
								conteudo = $"***{conteudo}***";
							}

							else if (ehNegrito)
							{
								conteudo = $"**{conteudo}**";
							}

							else if (ehItalico)
							{
								conteudo = $"*{conteudo}*";
							}

							else if (ehUnderline)
							{
								conteudo = $"<u>{conteudo}</u>";
							}

                            markdownBuilder.Append(conteudo);
						}
						else if(inline is LineBreak)
						{
							markdownBuilder.AppendLine(" \n");
						}
                    }
					if (temAlinhamento) markdownBuilder.Append(tagFechamento);
                }
				markdownBuilder.AppendLine().ToString().TrimEnd(Environment.NewLine.ToCharArray());
			}

			return markdownBuilder.ToString();
		}

		public static FlowDocument ConverteDeVolta(string textoMarkdown)
		{
			FlowDocument documento = new FlowDocument();

            if (string.IsNullOrEmpty(textoMarkdown))
            {
				documento.Blocks.Add(new Paragraph());
				return documento;
            }
			
			string[] linhas = textoMarkdown.Split(new[] {Environment.NewLine}, StringSplitOptions.None);


            foreach (string conteudoLinha in linhas)
            {
				Paragraph paragrafo = new Paragraph();
				string conteudoAlinhamento = conteudoLinha;

				TextAlignment alinhamento = TextAlignment.Left;

				if (conteudoAlinhamento.StartsWith("<div align=\"right\">") && conteudoAlinhamento.EndsWith("</div>"))
				{
					alinhamento = TextAlignment.Right;
					string tagAbertura = "<div align=\"right\">";
					string tagFechamento = "</div>";
					conteudoAlinhamento = conteudoAlinhamento.Substring(tagAbertura.Length,
						conteudoAlinhamento.Length - tagAbertura.Length - tagFechamento.Length);
				}
				else if (conteudoAlinhamento.StartsWith("<div align=\"center\">") && conteudoAlinhamento.EndsWith("</div>"))
                {
                    alinhamento = TextAlignment.Center;
					string tagAbertura = "<div align=\"center\">";
					string tagFechamento = "</div>";
					conteudoAlinhamento = conteudoAlinhamento.Substring(tagAbertura.Length,
						conteudoAlinhamento.Length - tagAbertura.Length - tagFechamento.Length);
                }
				else if (conteudoAlinhamento.StartsWith("<div align=\"justify\">") && conteudoAlinhamento.EndsWith("</div>"))
				{
					alinhamento = TextAlignment.Justify;
					string tagAbertura = "<div align=\"justify\">";
					string tagFechamento = "</div>";
					conteudoAlinhamento = conteudoAlinhamento.Substring(tagAbertura.Length,
						conteudoAlinhamento.Length - tagAbertura.Length - tagFechamento.Length);
				}

				paragrafo.TextAlignment = alinhamento;

                int lastIndex = 0;

				Regex inlineRegex = new Regex(@"(\*\*\*<u>(.*?)</u>\*\*\*)|(\*\*<u>(.*?)</u>\*\*)|(\*<u>(.*?)</u>\*)|(\*\*\*(.*?)\*\*\*)|(\*\*(.*?)\*\*)|(\*(.*?)\*)|(<u>(.*?)</u>)", RegexOptions.Multiline);
				Match m = inlineRegex.Match(conteudoAlinhamento);

                while (m.Success)
                {
                    if (m.Index > lastIndex)
                    {
						paragrafo.Inlines.Add(new Run(conteudoAlinhamento.Substring(lastIndex, m.Index - lastIndex)));
                    }

					Run formatedRun = new Run();
					string valorCapturado;

                    if (m.Groups[1].Success)
                    {
						valorCapturado = m.Groups[2].Value;
						formatedRun.Text = valorCapturado;
						formatedRun.FontWeight = FontWeights.Bold;
						formatedRun.FontStyle = FontStyles.Italic;
						formatedRun.TextDecorations = TextDecorations.Underline;
                    }
					else if (m.Groups[3].Success)
                    {
						valorCapturado = m.Groups[4].Value;
						formatedRun.Text = valorCapturado;
						formatedRun.TextDecorations = TextDecorations.Underline;
						formatedRun.FontWeight = FontWeights.Bold;
						
					}
					else if (m.Groups[5].Success)
                    {
						valorCapturado = m.Groups[6].Value;
						formatedRun.Text = valorCapturado;
						formatedRun.TextDecorations = TextDecorations.Underline;
						formatedRun.FontStyle = FontStyles.Italic;
						
					}
					else if (m.Groups[7].Success)
					{
						valorCapturado = m.Groups[8].Value;
						formatedRun.Text = valorCapturado;
						formatedRun.FontWeight = FontWeights.Bold;
						formatedRun.FontStyle = FontStyles.Italic;
					}
					else if (m.Groups[9].Success)
                    {
                        valorCapturado = m.Groups[10].Value;
						formatedRun.Text = valorCapturado;
						formatedRun.FontWeight = FontWeights.Bold;
					}
					else if (m.Groups[11].Success)
					{
						valorCapturado = m.Groups[12].Value;
						formatedRun.Text = valorCapturado;
						formatedRun.FontStyle = FontStyles.Italic;
					}
					else if (m.Groups[13].Success)
					{
						valorCapturado = m.Groups[14].Value;
						formatedRun.Text = valorCapturado;
						formatedRun.TextDecorations = TextDecorations.Underline;
					}
					paragrafo.Inlines.Add(formatedRun);
					lastIndex = m.Index + m.Length;
					m = m.NextMatch();
                }
				if(lastIndex < conteudoLinha.Length)
				{
					paragrafo.Inlines.Add(new Run(conteudoAlinhamento.Substring(lastIndex)));
				}
				documento.Blocks.Add(paragrafo);
            }
			return documento;
        }
	}
}
