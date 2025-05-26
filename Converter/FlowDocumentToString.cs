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
                    foreach (var inline in paragraph.Inlines)
                    {
                        if (inline is Run run)
                        {
							string conteudo = run.Text;
							bool ehUnderline = run.TextDecorations.Contains(TextDecorations.Underline[0]);
							bool ehNegrito = run.FontWeight == FontWeights.Bold;
							bool ehItalico = run.FontStyle == FontStyles.Italic;

							if (ehUnderline) conteudo = $"<u>{conteudo}</u>";

							if (ehNegrito && ehItalico) conteudo = $"***{conteudo}***";

							else if (ehNegrito)
							{
								conteudo = $"**{conteudo}**";
							}

							else if (ehItalico)
                            {
								conteudo = $"*{conteudo}*";
                            }


                            markdownBuilder.Append(conteudo);
						}
						else if(inline is LineBreak)
						{
							markdownBuilder.AppendLine(" \n");
						}
                    }
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


            foreach (string linhaContent in linhas)
            {
				Paragraph paragrafo = new Paragraph();
				int lastIndex = 0;

				Regex inlineRegex = new Regex(@"(\*\*\*(.*?)\*\*\*|\*\*(.*?)\*\*|\*(.*?)\*|<u>(.*?)</u>)", RegexOptions.Multiline);
				Match m = inlineRegex.Match(linhaContent);

                while (m.Success)
                {
                    if (m.Index > lastIndex)
                    {
						paragrafo.Inlines.Add(new Run(linhaContent.Substring(lastIndex, m.Index - lastIndex)));
                    }

					Run formatedRun = new Run();
					string valorCapturado;

                    if (m.Groups[1].Success)
                    {
						valorCapturado = m.Groups[2].Value;
						formatedRun.Text = valorCapturado;
						formatedRun.FontWeight = FontWeights.Bold;
						formatedRun.FontStyle = FontStyles.Italic;
                    }
					else if (m.Groups[3].Success)
                    {
						valorCapturado = m.Groups[4].Value;
						formatedRun.Text = valorCapturado;
						formatedRun.FontWeight = FontWeights.Bold;
                    }
					else if (m.Groups[5].Success)
                    {
						valorCapturado = m.Groups[6].Value;
						formatedRun.Text = valorCapturado;
						formatedRun.FontStyle = FontStyles.Italic;
                    }
                    else if (m.Groups[7].Success)
                    {
                        valorCapturado = m.Groups[8].Value;
						formatedRun.Text = valorCapturado;
						TextDecorationCollection formatoSublinhado = new TextDecorationCollection();
						formatoSublinhado.Add(TextDecorations.Underline[0]);
						formatedRun.TextDecorations = formatoSublinhado;

                    }
					paragrafo.Inlines.Add(formatedRun);
					lastIndex = m.Index + m.Length;
					m = m.NextMatch();
                }
				if(lastIndex < linhaContent.Length)
				{
					paragrafo.Inlines.Add(new Run(linhaContent.Substring(lastIndex)));
				}
				documento.Blocks.Add(paragrafo);
            }
            if (documento.Blocks.Count == 0)
            {
				documento.Blocks.Add(new Paragraph());
            }
			return documento;
        }
	}
}
