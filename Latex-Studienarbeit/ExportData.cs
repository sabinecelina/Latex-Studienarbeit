using System;
using System.IO;
using System.Text;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Latex_Studienarbeit
{
    class ExportData
    {
        private static string[] uebungen = new string[] { "preasuebg-", "hausuebg-", "tutorium-" };
        private static string[] uebungsart = new string[] { "P", "H", "T" };
        public static void SetUebungseinheitNumber(int number)
        {
            uebungen[0] = uebungen[0] + number + ".tex";
            uebungen[1] = uebungen[1] + number + ".tex";
            uebungen[2] = uebungen[2] + number + ".tex";
        }
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
            loesung = "%ID: " + id + " -- Loesungen zu: " + loesunsgname  + "\n" + loesung;
            if (loesung.Equals("NULL"))
            {
                loesung = "%zu dieser Aufgabe existiert noch keine Loesung";
            }
            return loesung;
        }
        public static void ExportUebungen(string userInput, SQLiteConnection m_dbConnection, int number, int auswahl)
        {
            SetUebungseinheitNumber(number);
            string sql;
            string[] input = userInput.Split(",");
            int caseNumber = 0;
            string uebungsart = "";
            string moeglichkeit = "";
            switch (auswahl)
            {
                case 1:
                    moeglichkeit = "Uebungsaufgabe, Loesung";
                    caseNumber = 1;
                    break;
                case 2:
                    moeglichkeit = "Uebungsaufgabe";
                    caseNumber = 2;
                    break;
                case 3:
                    moeglichkeit = "Loesung";
                    caseNumber = 3;
                    break;
            }
            if (input.Length == 1)
            {
                uebungsart = ReturnUebungsart(input[0]);
                sql = "select "+ moeglichkeit + " from MKB where Uebungsart='" +  input[0] + "' AND Uebungseinheit='" + number + "'";
                CreatePath(sql, m_dbConnection, uebungsart, caseNumber);
            }
            else if(input.Length == 2)
            {
                uebungsart = ReturnUebungsart(input[0]);
                sql = "select " + moeglichkeit + " from MKB where Uebungsart='" + input[0] + "' AND Uebungseinheit='" + number + "'";
                CreatePath(sql, m_dbConnection, uebungsart, caseNumber);
                uebungsart = ReturnUebungsart(input[1]);
                sql = "select " + moeglichkeit + " from MKB where Uebungsart='" + input[1] +"' AND Uebungseinheit='" + number + "'";
                CreatePath(sql, m_dbConnection, uebungsart, caseNumber);
            }
            else if(input.Length == 3)
            {
                for(int i = 0; i<input.Length; i++)
                {
                    uebungsart = ReturnUebungsart(input[i]);
                    sql = "select " + moeglichkeit + " from MKB where Uebungsart='" + input[i] + "' AND Uebungseinheit='" + number + "'";
                    CreatePath(sql, m_dbConnection, uebungsart, caseNumber);
                }
            }
            else if(input.Length > 3 || input.Length < 0)
            {
                Functions.ConsoleWrite("Die Eingabe war leider ungueltig.", ConsoleColor.DarkYellow);
            }
        }
        public static void ExportLoesungenTex(SQLiteConnection m_dbConnection, int number)
        {
            string sql;
            sql = "select ID, Loesung, NameDerAufgabe from MKB where Uebungsnummer='"+number+"'";
            CreatePath(sql, m_dbConnection, "loesungen-" + number + ".tex", 3);
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
        public static string ReturnUebungsart(string uebungsart)
        {
            switch (uebungsart)
            {
                case "P":
                    uebungsart = uebungen[0];
                    break;
                case "H":
                    uebungsart = uebungen[1];
                    break;
                case "T":
                    uebungsart = uebungen[2];
                    break;
            }
            return uebungsart;
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
                        aufgabe = ExportData.ExportAufgaben(reader);
                        loesung = ExportData.ExportLoesungen(reader);
                        aufgaben.Add(aufgabe + loesung);
                        break;
                    case 2:
                        aufgabe = ExportData.ExportAufgaben(reader);
                        aufgaben.Add(aufgabe);
                        break;
                    case 3:
                        loesung = ExportData.ExportLoesungen(reader);
                        aufgaben.Add(loesung);
                        aufgaben.Add("\n");
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
