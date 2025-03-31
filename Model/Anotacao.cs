using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalNotesApp.Model
{
    public class Anotacao : Base
    {
        public string Texto { get; set; }
        public Anotacao(string nome)
        {
            Nome = nome;
            Texto = "Olá Mundo!";
        }
    }
}
