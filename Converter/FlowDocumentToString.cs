using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace PersonalNotesApp.Converter
{
	public class FlowDocumentToString
	{
		public static string Converte(FlowDocument documento)
		{
			return new TextRange(documento.ContentStart, documento.ContentEnd).Text;
		}

		public static FlowDocument ConverteDeVolta(string texto)
		{
			FlowDocument documento = new FlowDocument();
			documento.Blocks.Add(new Paragraph(new Run(texto)));
			return documento;
		}
	}
}
