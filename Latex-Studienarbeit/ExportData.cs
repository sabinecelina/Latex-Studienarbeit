using System;
using System.IO;
using System.Text;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Latex_Studienarbeit
{
    class ExportData
    {
        private static string[] uebungen = new string[] { "preasuebg-", "hausuebg-.tex", "tutorium-.tex" };
        private static string[] uebungsart = new string[] { "P", "H", "T" };
        public static string ExportAufgaben(string aufgabe, SQLiteDataReader reader)
        {
            aufgabe = reader["Uebungsaufgabe"].ToString();
            aufgabe = aufgabe.Replace("slash", @"\").Replace("anfuerungszeichen", "\"")
                .Replace("replacedonesign", "\'").Replace("dollar", @"$");
            return aufgabe;
        }
        public static string ExportLoesungen(string loesung, SQLiteDataReader reader)
        {
            loesung = reader["Loesung"].ToString();
            loesung = loesung.Replace("slash", @"\").Replace("anfuerungszeichen", "\"")
                .Replace("replacedonesign", "\'").Replace("dollar", @"$");
            return loesung;
        }
        public static void ExportUebungenAndLoesungen(string userInput, SQLiteConnection m_dbConnection, int number)
        {
            uebungen[0] = uebungen[0] + number + ".tex";
            uebungen[1] = uebungen[1] + number + ".tex";
            string sql;
            if (userInput.Equals("P") || userInput.Equals("p"))
            {
                sql = "select Uebungsaufgabe, Loesung from MKB where Uebungsart='P' AND Uebungseinheit='"+number+"'";
                CreatePath(sql, m_dbConnection, uebungen[0], 3);
            }
            else if (userInput.Equals("H") || userInput.Equals("h"))
            {
                sql = "select Uebungsaufgabe, Loesung from MKB where Uebungsart='H' AND Uebungseinheit='" + number + "'";
                CreatePath(sql, m_dbConnection, uebungen[1], 3);
            }
            else if (userInput.Equals("PH") || userInput.Equals("ph"))
            {
                sql = "select Uebungsaufgabe, Loesung from MKB where Uebungsart='P' AND Uebungseinheit='" + number + "'";
                CreatePath(sql, m_dbConnection, uebungen[0], 3);
                sql = "select Uebungsaufgabe, Loesung from MKB where Uebungsart='H' AND Uebungseinheit='" + number + "'";
                CreatePath(sql, m_dbConnection, uebungen[1], 3);
            }
        }
        public static void ExportUebungen(string userInput2, SQLiteConnection m_dbConnection)
        {
            string sql;
            if (userInput2.Equals("P"))
            {
                sql = "select Uebungsaufgabe from MKB where Uebungsart='P'";
                CreatePath(sql, m_dbConnection, uebungen[0], 1);
            }
            else if (userInput2.Equals("H"))
            {
                sql = "select Uebungsaufgabe from MKB where Uebungsart='H'";
                CreatePath(sql, m_dbConnection, uebungen[1], 1);
            }
            else if (userInput2.Equals("PH"))
            {
                sql = "select Uebungsaufgabe from MKB where Uebungsart='P'";
                CreatePath(sql, m_dbConnection, uebungen[0], 1);
                sql = "select Uebungsaufgabe from MKB where Uebungsart='H'";
                CreatePath(sql, m_dbConnection, uebungen[1], 1);
            }
        }
        public static void ExportLoesungenTex(SQLiteConnection m_dbConnection)
        {
            string sql;
            sql = "select Loesung from MKB";
            CreatePath(sql, m_dbConnection, "loesungen.tex", 2);
            Console.WriteLine("Es wurde eine neue loesung.tex Datei erstellt");
        }
        public static void CreatePath(string sql, SQLiteConnection m_dbConnection, string filepath, int exportArt)
        {
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            string aufgabe = "";
            string loesung = "";

            var aufgaben = new List<string>();
            while (reader.Read())
            {
                //schaut, was alles exportiert werden muss: Loesungen, Uebungen oder Uebungen mit Loesungen
                switch (exportArt)
                {
                    case 1:
                        aufgabe = ExportData.ExportAufgaben(aufgabe, reader);
                        aufgaben.Add(aufgabe);
                        break;
                    case 2:
                        loesung = ExportData.ExportLoesungen(loesung, reader);
                        aufgaben.Add(loesung);
                        break;
                    case 3:
                        aufgabe = ExportData.ExportAufgaben(aufgabe, reader);
                        loesung = ExportData.ExportLoesungen(loesung, reader);
                        aufgaben.Add(aufgabe + loesung);
                        break;
                }
            }
            try
            {
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(@"..\..\..\..\" + filepath))
                {
                    for (int j = 0; j < aufgaben.Count; j++)
                    {
                        byte[] info = new UTF8Encoding(true).GetBytes(aufgaben[j]);
                        // Add some information to the file.
                        fs.Write(info, 0, info.Length);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
