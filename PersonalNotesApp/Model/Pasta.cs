using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalNotesApp.Model
{
    public class Pasta : Base
    {
		public ObservableCollection<Base> SubPastas { get; set; }

		public Pasta(string nome)
		{
            if (nome == null)
            {
                throw new ArgumentNullException(nameof(nome), "O nome da pasta não deve ser nulo");
            }
            Nome = nome;
			SubPastas = new ObservableCollection<Base>();
		}

	}
}
