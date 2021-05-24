using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latex_Studienarbeit
{
    class Mathematik
    {
        public List<Studiengang> studiengang;

        public Mathematik(List<Studiengang> studiengang)
        {
            this.studiengang = studiengang;
        }
        public List<Studiengang> getStudiengang()
        {
            return this.studiengang;
        }
    }
}
