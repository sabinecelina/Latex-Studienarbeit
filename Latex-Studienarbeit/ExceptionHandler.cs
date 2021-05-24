using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latex_Studienarbeit
{
	public class ExceptionHandler: Exception {
	protected int fall;

	public ExceptionHandler(int fall)
	{
		this.fall = fall;
	}

	public ExceptionHandler(String text)
	{

		this.fall = 0;
	}

	public Exception notice()
	{
		Exception e = new ExceptionHandler("Error!");

		if (fall == 1)
		{
			e = new ExceptionHandler("Error! Nicht zulässige Zeichen");
		}
		if (fall == 2)
		{
			e = new ExceptionHandler("Error! Mehrere Operatoren hintereinander");
		}
		if (fall == 3)
		{
			e = new ExceptionHandler("Error! Zeichenkette endet mit Operator");
		}
		return e;
	}

}
}
