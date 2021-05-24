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
        public static List<Mathematik> mathematikZwei = new List<Mathematik>();
        public static void readJson()
        {
            string path = "../../../Latex.json";
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                mathematikZwei = JsonConvert.DeserializeObject<List<Mathematik>>(json);
            }
        }
        public static void ChangeDatabaseEntry()
        {
            string[] uebungsart = new string[] { "P", "H", "T" };
            List<Uebungen> uebungen = new List<Uebungen>();
            for (int m = 0; m < 2; m++)
            {
                for (int j = 0; j < uebungsart.Length; j++)
                {
                    int zeigen = mathematikZwei[0].getStudiengang()[m].getListAufgaben()[0].getT().Count;
                    Console.WriteLine(zeigen);
                    switch (uebungsart[j])
                    {
                        case "P":
                            for (int i = 0; i < mathematikZwei[0].getStudiengang()[m].getListAufgaben()[0].getP().Count; i++)
                                uebungen.Add(mathematikZwei[0].getStudiengang()[m].getListAufgaben()[0].getP()[i]);
                            break;
                        case "H":
                            for (int i = 0; i < mathematikZwei[0].getStudiengang()[m].getListAufgaben()[0].getH().Count; i++)
                                uebungen.Add(mathematikZwei[0].getStudiengang()[m].getListAufgaben()[0].getH()[i]);
                            break;
                        case "T":
                            bool check = false;
                            if ((mathematikZwei[0].getStudiengang()[m].getListAufgaben()[0].getT().Count != 0))
                                check = true;
                            if (check)
                            {
                                for (int i = 0; i < mathematikZwei[0].getStudiengang()[m].getListAufgaben()[0].getT().Count; i++)
                                    uebungen.Add(mathematikZwei[0].getStudiengang()[m].getListAufgaben()[0].getT()[i]);
                            }
                            break;
                    }
                    foreach (Uebungen uebung in uebungen)
                    {
                        string nameDerAufgabe = uebung.getName();
                        nameDerAufgabe = Functions.ReplaceStringToDB(nameDerAufgabe);
                        int uebungseinheit = mathematikZwei[0].getStudiengang()[m].getUebungseinheit();
                        int aufgabennummer = uebung.getAufgabennummer();
                        string sql = ("update MKB set NameDerAufgabe='" + nameDerAufgabe + "' where Uebungseinheit=" + uebungseinheit + " AND Uebungsart='" + uebungsart[j] + "' AND Uebungsnummer=" + aufgabennummer + "");
                        Functions.sqlStatement(sql);
                    }
                    uebungen.Clear();
                }

            }
        }
    }
}
