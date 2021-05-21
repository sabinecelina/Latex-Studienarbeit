using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latex_Studienarbeit
{
    class Mathe2
    {
        public List<Studiengang> studiengang;

        public Mathe2(List<Studiengang> studiengang)
        {
            this.studiengang = studiengang;
        }
        public List<Studiengang> getStudiengang()
        {
            return this.studiengang;
        }
    }
}
