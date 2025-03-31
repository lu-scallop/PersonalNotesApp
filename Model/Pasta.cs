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

		
		public ObservableCollection<Pasta> SubPastas { get; set; }

		public Pasta(string nome)
		{
			Nome = nome;
			SubPastas = new ObservableCollection<Pasta>();
		}

	}
}
