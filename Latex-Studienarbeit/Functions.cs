using System;
using System.IO;
using System.Text;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Latex_Studienarbeit
{
    class Functions
    {
        private static string[] uebungsart = new string[] { "P", "H", "T" };
        private static string connectionPath = @"Data Source=..\..\..\..\MKB.sqlite;Version=3;";
        private static SQLiteConnection m_dbConnection = new SQLiteConnection(connectionPath);
        public static List<string> GetAllFiles(DirectoryInfo d)
        {
            List<string> fileName = new();
            FileInfo[] Files = d.GetFiles("*.tex"); //Getting Text files
            foreach (FileInfo file in Files)
            {
                fileName.Add(file.Name);
            }
            //sortList();
            fileName.Sort();
            return fileName;
        }
        public static void sqlStatement(string sql)
        {
            m_dbConnection.Open();
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }
        public static string ReplaceStringToDB(string text)
        {
            return text.Replace(@"\", "slash").Replace("\"", "anfuerungszeichen")
                                                .Replace("\'", "replacedonesign").Replace(@"$", "dollar").Replace("-", "minus").Replace("'", "anfuerungszeichen");
        }
        public static string ReplaceStringToText(string text)
        {
            return text.Replace("slash", @"\").Replace("anfuerungszeichen", "\"")
                .Replace("replacedonesign", "\'").Replace("dollar", @"$").Replace("minus", "-").Replace("anfuerungszeichen", "'");
        }
        public static void ConsoleWrite(string text, ConsoleColor backgroundcolor)
        {
            Console.BackgroundColor = backgroundcolor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public static void AllUebungenFromNumner(string writeLine)
        {
            m_dbConnection.Open();
            Functions.ConsoleWrite(writeLine, ConsoleColor.DarkBlue);
            string getUserInput = Console.ReadLine();
            string[] allInput = getUserInput.Split(',');
            Functions.ConsoleWrite("Sie haben folgende Übungen zur Auswahl: \n", ConsoleColor.DarkBlue);
            for(int i = 0; i< allInput.Length; i++) {
                int uebungseinheit = Int32.Parse(allInput[i]);
                string sql = "select ID, NameDerAufgabe, Uebungsnummer from MKB where Uebungseinheit=" + allInput[i] + "";
                SQLiteCommand command = new(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                List<Uebungen> uebungen = new();
                while (reader.Read())
                {
                    string aufgabename = ExportFromDB.ExportNameDerAufgabe(reader);
                    aufgabename = Functions.ReplaceStringToText(aufgabename);
                    int aufgabennummer = Int32.Parse(ExportFromDB.ExportUebungsnummer(reader));
                    string id = ExportFromDB.ExportID(reader);
                    int idNumber = Int32.Parse(id);
                    Uebungen uebung = new(aufgabename, aufgabennummer, idNumber);
                    uebungen.Add(uebung);
                }
                for (int j = 0; j < uebungen.Count; j++)
                {
                    Functions.ConsoleWrite(uebungen[j].GetName() + " || " + uebungen[j].GetAufgabennummer() + " || ID: " + uebungen[j].GetId(), ConsoleColor.DarkRed);
                }
                Console.WriteLine("\n");
            }
           
            m_dbConnection.Close();
        }
    }
}
