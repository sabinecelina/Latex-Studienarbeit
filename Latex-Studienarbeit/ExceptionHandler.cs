using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latex_Studienarbeit
{
    public class ExceptionHandler : Exception
    {
        /**
         * @param message
         */
        public ExceptionHandler(String message, ConsoleColor consoleColor)
        {
            Functions.ConsoleWrite("\n" + message + "\n", consoleColor);
        }

    }

}
