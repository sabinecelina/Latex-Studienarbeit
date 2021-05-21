using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Latex_Studienarbeit
{
    class ChangeEntry
    {
        public static SQLiteConnection m_dbConnection = new SQLiteConnection(@"Data Source=..\..\..\..\MKB.sqlite;Version=3;");
        public static void UpdateTexEntry()
        {
            ReadFromDatabase.ConsoleWrite("Welche Aufgabe moechten Sie aendern? Tippen Sie wie folgt ein: 1,P,8", ConsoleColor.DarkBlue);
            string getUserInput = Console.ReadLine();
            string[] userInput = getUserInput.Split(",");
            while (userInput.Length != 3)
            {
                ReadFromDatabase.ConsoleWrite("Sie haben die Aufgabe nicht richtig eingeschrieben. Versuchen Sie es erneut: 1,P,8", ConsoleColor.DarkBlue);
                getUserInput = Console.ReadLine();
                userInput = getUserInput.Split(",");
            }
            m_dbConnection.Open();
            string sql;
            sql = "select Uebungsaufgabe from MKB where Uebungseinheit='" + userInput[0] + "' AND Uebungsart='" + userInput[1] + "' AND Uebungsnummer='" + userInput[2] + "'";
            string changeEntryPath = "uebungsaufgabe.tex";
            ExportData.CreatePath(sql, m_dbConnection, changeEntryPath, 1);
            Console.WriteLine("Tippen Sie 'weiter' sobal Sie die Uebungsaufgabe geaendert haben.");
            getUserInput = Console.ReadLine();
            if (getUserInput.Equals("weiter"))
            {
                string filename = @"..\..\..\..\" + changeEntryPath;
                string line = "";
                string uebungsaufgabe = "";
                System.IO.StreamReader file =
                    new System.IO.StreamReader(filename);
                while ((line = file.ReadLine()) != null)
                {
                    uebungsaufgabe += "\n" + line;
                }
                uebungsaufgabe = uebungsaufgabe.Replace("slash", @"\").Replace("anfuerungszeichen", "\"").Replace("replacedonesign", "\'").Replace("dollar", @"$");
                sql = "update MKB set Uebungsaufgabe='" + uebungsaufgabe + "' where Uebungseinheit='" + userInput[0] + "' AND Uebungsart='" + userInput[1] + "' AND Uebungsnummer='" + userInput[2] + "' ";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
            }
            m_dbConnection.Close();
        }
        public static void ChangeOrderinDatabase()
        {
            m_dbConnection.Open();
            ReadFromDatabase.ConsoleWrite("In welcher Uebungseinheit und Uebungsart moechten Sie die Reihenfolge aendern 1,P?", ConsoleColor.DarkBlue);
            string getUserInput = Console.ReadLine();
            string[] userInputArray = getUserInput.Split(",");
            while(userInputArray.Length != 2)
            {
                ReadFromDatabase.ConsoleWrite("Das Eingabeformat war leider falsch, bitte versuchen Sie es erneut.", ConsoleColor.DarkRed);
                getUserInput = Console.ReadLine();
                userInputArray = getUserInput.Split(",");
            }
            ReadFromDatabase.ConsoleWrite("Sie haben folgende Uebungen, deren Reihenfolge Sie aendern koennen.", ConsoleColor.DarkBlue);
            string sql = "select NameDerAufgabe, Uebungsnummer from MKB where Uebungseinheit='" + userInputArray[0] + "' and Uebungsart='" + userInputArray[1] + "'";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            var aufgaben = new List<string>();
            var aufgabennummern = new List<string>();
            while (reader.Read())
            {
                string aufgabename = ExportData.ExportNameDerAufgabe(reader);
                aufgabename = aufgabename.Replace("slash", @"\").Replace("anfuerungszeichen", "\"")
                .Replace("replacedonesign", "\'").Replace("dollar", @"$");
                aufgaben.Add(aufgabename);
                string aufgabennummer = ExportData.ExportUebungsnummer(reader);
                aufgabennummern.Add(aufgabennummer);
            }
            for(int i = 0; i<aufgaben.Count; i++)
            {
                ReadFromDatabase.ConsoleWrite("\n" + aufgabennummern[i] + " || " + aufgaben[i], ConsoleColor.DarkRed);
            }
            List <Uebungen> uebungen = new List<Uebungen>();
            for(int j = 0; j<aufgaben.Count; j++)
            {
                uebungen[j].setAufgabennummer(Int32.Parse(aufgabennummern[j]));
                uebungen[j].setName(aufgaben[j]);
            }
            ReadFromDatabase.ConsoleWrite("Welche Aufgaben moechten sie tauschen? [1,2]", ConsoleColor.DarkBlue);
            getUserInput = Console.ReadLine();
            userInputArray = getUserInput.Split(",");
            for(int i = 0; i< uebungen.Count; i++)
            {
            sql = "update MKB set Uebungsnummer='" + userInputArray[0] + "' where Uebungsnummer='" + userInputArray[1] +  "' ";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            sql = "update MKB set Uebungsnummer='" + userInputArray[1] + "' where Uebungsnummer='" + userInputArray[0] + "' ";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            }
            m_dbConnection.Close();
        }
    }
}
