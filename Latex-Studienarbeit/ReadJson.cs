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
        public static void ChangeDatabaseEntry()
        {
            SQLiteConnection m_dbConnection = new SQLiteConnection(@"Data Source=..\..\..\..\MKB.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "";
            string[] uebungsart = new string[] { "P", "H", "T" };
            List<Uebungen> uebungen = new List<Uebungen>();
            for (int j = 0; j < uebungsart.Length; j++)
            {
                switch (uebungsart[j])
                {
                    case "P":
                        for (int i = 0; i < mathematikZwei[0].getStudiengang()[0].getListAufgaben()[0].getP().Count; i++)
                            uebungen.Add(mathematikZwei[0].getStudiengang()[0].getListAufgaben()[0].getP()[i]);
                        break;
                    case "H":
                        for (int i = 0; i < mathematikZwei[0].getStudiengang()[0].getListAufgaben()[0].getH().Count; i++)
                            uebungen.Add(mathematikZwei[0].getStudiengang()[0].getListAufgaben()[0].getH()[i]);
                        break;
                    case "T":
                        bool check = false;
                        if ((mathematikZwei[0].getStudiengang()[0].getListAufgaben()[0].getT() == null))
                            check = true;
                        if (check)
                        {
                            for (int i = 0; i < mathematikZwei[0].getStudiengang()[0].getListAufgaben().Count; i++)
                                uebungen.Add(mathematikZwei[0].getStudiengang()[0].getListAufgaben()[0].getT()[i]);
                        }
                        break;
                }
                foreach (Uebungen uebung in uebungen)
                {
                    string nameDerAufgabe = uebung.getName();
                    nameDerAufgabe = nameDerAufgabe.Replace(@"\", "slash").Replace("\"", "anfuerungszeichen")
                                                    .Replace("\'", "replacedonesign").Replace(@"$", "dollar");
                    int uebungseinheit = mathematikZwei[0].getStudiengang()[0].getUebungseinheit();
                    int aufgabennummer = uebung.getAufgabennummer();
                    sql = ("update MKB set NameDerAufgabe='" + nameDerAufgabe + "' where Uebungseinheit=" + uebungseinheit + " AND Uebungsart='" + uebungsart[j] + "' AND Uebungsnummer=" + aufgabennummer + "");
                    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();
                }
                uebungen.Clear();
            }
        }
    }
}
