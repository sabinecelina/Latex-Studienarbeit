using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latex_Studienarbeit
{
    class Aufgaben
    {
        public List<Uebungen> P;
        public List<Uebungen> H;
        public List<Uebungen> T;
        public Aufgaben(List<Uebungen> P, List<Uebungen> H, List<Uebungen> T)
        {
            this.P = P;
            this.H = H;
            this.T = T;
        }
        public List<Uebungen> getP()
        {
            return this.P;
        }
        public List<Uebungen> getH()
        {
            return this.H;
        }
        public List<Uebungen> getT()
        {
            return this.T;
        }
    }
}
