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


            var parts = Regex.Split(textoMarkdown, @"(\*\*.*?\*\*|\*.*?\*|<u>.*?</u>)");

			foreach (var part in parts)
			{
                if (Regex.IsMatch(part, @"\*\*(.*?)\*\*"))
                {
					paragraph.Inlines.Add(new Run(part.Trim('*')) {FontWeight=FontWeights.Bold});
                }
                else if (Regex.IsMatch(part, @"\*(.*?)\*"))
                {
					paragraph.Inlines.Add(new Run(part.Trim('*')) { FontStyle = FontStyles.Italic});
                }
				else if(Regex.IsMatch(part, @"<u>.*?</u>"))
				{
					paragraph.Inlines.Add(new Run(Regex.Match(part, @"<u>(.*?)</u>").Groups[1].Value)
					{
						TextDecorations = TextDecorations.Underline
					});
				}
				else
				{
					paragraph.Inlines.Add(new Run(part));
				}
            }
			documento.Blocks.Add(paragraph);
			return documento;
		}
	}
}
