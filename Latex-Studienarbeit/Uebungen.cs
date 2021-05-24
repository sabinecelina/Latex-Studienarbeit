using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latex_Studienarbeit
{
    class Uebungen
    {
        private string name;
        private int aufgabennummer;
        private string aufgabe;
        private string loesungen;
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
        public void setName(string name)
        {
            this.name = name;
        }
        public void setAufgabennummer(int aufgabennummer)
        {
           this.aufgabennummer = aufgabennummer;
        }
    }
}
