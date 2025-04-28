using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalNotesApp.Model
{
    public class Anotacao : Base
    {
        private string _texto;
        public string Texto 
        {
            get => _texto;
            set
            {
                _texto = value;
                OnPropertyChanged(nameof(Texto));
            }
        }
        public Anotacao(string nome)
        {
            Nome = nome;
            Texto = "Olá Mundo!";
        }
    }
}
