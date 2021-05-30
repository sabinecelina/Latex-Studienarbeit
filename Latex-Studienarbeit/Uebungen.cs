using Newtonsoft.Json;
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
        private string loesung;
        private int id;
        private string uebungseinheit;
        public Uebungen()
        {
        }

        [JsonConstructor]
        public Uebungen(string name, int aufgabennummer)
        {
            this.name = name;
            this.aufgabennummer = aufgabennummer;
        }
        public Uebungen(string name, int aufgabennummer, int id) {
            this.name = name;
            this.aufgabennummer = aufgabennummer;
            this.id = id;
        }
        public Uebungen(string uebungseinheit, string aufgabe, string loesung, string name, int aufgabennummer, int id)
        {
            this.aufgabe = aufgabe;
            this.loesung = loesung;
            this.name = name;
            this.aufgabennummer = aufgabennummer;
            this.id = id;
            this.uebungseinheit = uebungseinheit;
        }
        public string GetAufgabe()
        {
            return this.aufgabe;
        }
        public string GetLoesung()
        {
            return this.loesung;
        }
        public string GetName()
        {
            return this.name;
        }
        public int GetAufgabennummer()
        {
            return this.aufgabennummer;
        }
        public int GetId()
        {
            return this.id;
        }
        public void SetAufgabe(string aufgabe)
        {
             this.aufgabe = aufgabe;
        }
        public void SetLoesung(string loesung)
        {
             this.loesung = loesung;
        }
        public void SetName(string name)
        {
            this.name = name;
        }
        public void SetAufgabennummer(int aufgabennummer)
        {
            this.aufgabennummer = aufgabennummer;
        }
        public void SetId(int id)
        {
            this.id = id;
        }
    }
}
