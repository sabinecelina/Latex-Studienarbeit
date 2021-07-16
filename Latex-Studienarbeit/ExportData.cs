using System;
using System.IO;
using System.Text;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Latex_Studienarbeit
{
    class ExportData
    {
        private static string[] texname; 
        public static void ExportUebungen(string userInput, SQLiteConnection m_dbConnection, string[] tex, int number,  int auswahl)
        {
            texname = tex;
            string sql;
            string[] input = userInput.Split(",");
            for(int m = 0; m<input.Length; m++)
            {
                input[m] = input[m].ToUpper();
            }
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
       
        public static string ReturnUebungsart(string uebungsart)
        {
            switch (uebungsart)
            {
                case "P":
                    uebungsart = texname[0];
                    break;
                case "H":
                    uebungsart = texname[1];
                    break;
                case "T":
                    uebungsart = texname[2];
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
                        aufgabe = ExportFromDB.ExportAufgaben(reader);
                        loesung = ExportFromDB.ExportLoesungen(reader);
                        aufgaben.Add(aufgabe + loesung);
                        break;
                    case 2:
                        aufgabe = ExportFromDB.ExportAufgaben(reader);
                        aufgaben.Add(aufgabe);
                        break;
                    case 3:
                        loesung = ExportFromDB.ExportLoesungenForUser(reader);
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
