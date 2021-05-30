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
    }
}
