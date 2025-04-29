using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace PersonalNotesApp.Model
{
    public class Anotacao : Base
    {
        private FlowDocument _conteudo;
        public FlowDocument Conteudo 
        {
            get => _conteudo;
            set
            {
                _conteudo = value;
                OnPropertyChanged(nameof(Conteudo));
            }
        }
        public Anotacao(string nome)
        {
            Nome = nome;
            Conteudo = new FlowDocument(new Paragraph(new Run("Olá Mundo")));
        }
    }
}
