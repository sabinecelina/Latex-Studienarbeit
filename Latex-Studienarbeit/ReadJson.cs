using System.Diagnostics;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Linq;
using System.Data.SQLite;

namespace Latex_Studienarbeit
{
    class ReadJson
    {
        public static List<Mathe2> mathematikZwei = new List<Mathe2>();
        public static void readJson()
        {
            string path = "../../../Latex.json";
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                Console.WriteLine(json);
                mathematikZwei = JsonConvert.DeserializeObject<List<Mathe2>>(json);
                Console.WriteLine(mathematikZwei[0].getStudiengang()[0].getListAufgaben()[0].getP()[0].getName());
            }
        }
    }
}
