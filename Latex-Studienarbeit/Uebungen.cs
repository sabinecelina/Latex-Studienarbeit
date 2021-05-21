using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latex_Studienarbeit
{
    class Uebungen
    {
        public string name;
        public int aufgabennummer;
        public Uebungen(string name, int aufgabennummer) {
            this.name = name;
            this.aufgabennummer = aufgabennummer;
        }
        public string getName()
        {
            return this.name;
        }
        public int getAufgabennummer()
        {
            return this.aufgabennummer;
        }
    }
}
