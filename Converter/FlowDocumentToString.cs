using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace PersonalNotesApp.Converter
{
	public class FlowDocumentToString
	{
		public static string Converte(FlowDocument documento)
		{
			//return new TextRange(documento.ContentStart, documento.ContentEnd).Text;

			var markdownBuilder = new StringBuilder();

			foreach (var block in documento.Blocks)
			{
                if (block is Paragraph paragraph)
                {
                    foreach (var inline in paragraph.Inlines)
                    {
                        if (inline is Run run)
                        {
							string text = run.Text;
							if (run.FontWeight == FontWeights.Bold) text = $"**{text}**";
							if (run.FontStyle == FontStyles.Italic) text = $"*{text}*";
							if (run.TextDecorations.Contains(TextDecorations.Underline[0])) text = $"<u>text</u>";

							markdownBuilder.Append(text);
						}
						else if(inline is LineBreak)
						{
							markdownBuilder.AppendLine();
						}
                    }
                }
				markdownBuilder.AppendLine();
            }

			return markdownBuilder.ToString();
		}

		public static FlowDocument ConverteDeVolta(string texto)
		{
			FlowDocument documento = new FlowDocument();
			documento.Blocks.Add(new Paragraph(new Run(texto)));
			return documento;
		}
	}
}
