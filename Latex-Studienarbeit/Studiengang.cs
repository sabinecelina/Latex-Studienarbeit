using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latex_Studienarbeit
{
    class Studiengang
    {
        public int uebungseinheit;
        public List<Aufgaben> aufgaben;
        public Studiengang(int uebungseinheit, List<Aufgaben> aufgaben)
        {
            this.uebungseinheit = uebungseinheit;
            this.aufgaben = aufgaben;
        }
        public int getUebungseinheit()
        {
            return this.uebungseinheit;
        }
        public List<Aufgaben> getListAufgaben()
        {
            return this.aufgaben;
        }
    }
}
