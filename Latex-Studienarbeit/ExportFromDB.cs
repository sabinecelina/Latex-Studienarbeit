using System;
using System.Data.SQLite;


namespace Latex_Studienarbeit
{
    class ExportFromDB
    {
        public static string ExportAufgaben(SQLiteDataReader reader)
        {
            string aufgabe = "";
            aufgabe = reader["Uebungsaufgabe"].ToString();
            aufgabe = Functions.ReplaceStringToText(aufgabe);
            return aufgabe;
        }
        public static string ExportLoesungen(SQLiteDataReader reader)
        {
            string loesunsgname = reader["NameDerAufgabe"].ToString();
            string loesung = reader["Loesung"].ToString();
            string id = reader["ID"].ToString();
            loesung = Functions.ReplaceStringToText(loesung);
            loesunsgname = Functions.ReplaceStringToText(loesunsgname);
            loesung = "%ID: " + id + " -- Loesungen zu: " + loesunsgname + "\n" + loesung;
            if (loesung.Equals("NULL"))
            {
                loesung = "%zu dieser Aufgabe existiert noch keine Loesung";
            }
            return loesung;
        }
        public static void ExportLoesungenTex(SQLiteConnection m_dbConnection, int number)
        {
            string sql;
            sql = "select ID, Loesung, NameDerAufgabe from MKB where Uebungsnummer='" + number + "'";
            ExportData.CreatePath(sql, m_dbConnection, "loesungen-" + number + ".tex", 3);
            Console.WriteLine("Es wurde eine neue loesung.tex Datei erstellt");
        }
        public static string ExportNameDerAufgabe(SQLiteDataReader reader)
        {
            string aufgabe = "";
            aufgabe = reader["NameDerAufgabe"].ToString();
            aufgabe = Functions.ReplaceStringToText(aufgabe);
            return aufgabe;
        }
        public static string ExportUebungsnummer(SQLiteDataReader reader)
        {
            string nummer = "";
            nummer = reader["Uebungsnummer"].ToString();
            return nummer;
        }
        public static string ExportID(SQLiteDataReader reader)
        {
            string id = "";
            id = reader["ID"].ToString();
            return id;
        }
        public static string ExportUebungseinheit(SQLiteDataReader reader)
        {
            string id = "";
            id = reader["Uebungseinheit"].ToString();
            return id;
        }
        public static Uebungen ExportAll(SQLiteDataReader reader)
        {
            string aufgabe = ExportAufgaben(reader);
            string loesung = ExportLoesungen(reader);
            string name = ExportNameDerAufgabe(reader);
            string uebungsnummer = ExportUebungsnummer(reader);
            int nummer = Int32.Parse(uebungsnummer);
            string id = ExportID(reader);
            int idnummer = Int32.Parse(id);
            string uebungseinheit = ExportUebungseinheit(reader);
            Uebungen uebung = new Uebungen(uebungseinheit, aufgabe, loesung, name, nummer, idnummer);
            return uebung;
        }
        public static void ExportFiles(SQLiteConnection m_dbConnection, int number, int auswahl)
        {
            if (number == 1 || number == 2)
            {
                while (true)
                {
                    try
                    {
                        Functions.ConsoleWrite("Welche Uebungsaufgaben moechten Sie exportieren? Geben Sie jeweils P oder H oder T an. Trennen Sie Ihre Angaben bitte mit einem ','", ConsoleColor.DarkBlue);
                        string userInput = Console.ReadLine();
                        string[] input = userInput.Split(",");
                        if (input.Length == 1)
                        {
                            throw new Exception();
                        }
                        else
                        {
                            ExportData.ExportUebungen(userInput, m_dbConnection, auswahl, number);
                        }
                    }
                    catch (Exception e)
                    {
                        Functions.ConsoleWrite("Diese Eingabe war leider ungueltig. Bitte versuchen Sie es erneut./n ", ConsoleColor.DarkYellow);
                        continue;
                    }
                    break;
                }
            }
            else if (number == 3)
            {
                ExportFromDB.ExportLoesungenTex(m_dbConnection, auswahl);
            }
        }
    }
}
