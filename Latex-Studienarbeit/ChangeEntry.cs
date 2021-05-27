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
            Functions.ConsoleWrite("Welche Aufgabe moechten Sie aendern? Tippen Sie wie folgt ein: 1,P,8", ConsoleColor.DarkBlue);
            string getUserInput = Console.ReadLine();
            string[] userInput = getUserInput.Split(",");
            while (userInput.Length != 3)
            {
                Functions.ConsoleWrite("Sie haben die Aufgabe nicht richtig eingeschrieben. Versuchen Sie es erneut: 1,P,8", ConsoleColor.DarkBlue);
                getUserInput = Console.ReadLine();
                userInput = getUserInput.Split(",");
            }
            m_dbConnection.Open();
            string sql;
            sql = "select Uebungsaufgabe from MKB where Uebungseinheit='" + userInput[0] + "' AND Uebungsart='" + userInput[1] + "' AND Uebungsnummer='" + userInput[2] + "'";
            string changeEntryPath = "uebungsaufgabe.tex";
            ExportData.CreatePath(sql, m_dbConnection, changeEntryPath, 1);
            Console.WriteLine("Tippen Sie 'weiter' sobald Sie die Uebungsaufgabe geaendert haben.");
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
                uebungsaufgabe = Functions.ReplaceStringToDB(uebungsaufgabe);
                sql = "update MKB set Uebungsaufgabe='" + uebungsaufgabe + "' where Uebungseinheit='" + userInput[0] + "' AND Uebungsart='" + userInput[1] + "' AND Uebungsnummer='" + userInput[2] + "' ";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
            }
            m_dbConnection.Close();
        }
        public static void ChangeOrderinDatabase()
        {
            m_dbConnection.Open();
            Functions.ConsoleWrite("In welcher Uebungseinheit moechten Sie die Reihenfolge aendern 1?", ConsoleColor.DarkBlue);
            string getUserInput = Console.ReadLine();
            string[] userInputArray = getUserInput.Split(",");
            Functions.ConsoleWrite("Sie haben folgende Uebungen, deren Reihenfolge Sie aendern koennen.", ConsoleColor.DarkBlue);
            string sql = "select ID, NameDerAufgabe, Uebungsnummer from MKB where Uebungseinheit='" + getUserInput + "'";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<Uebungen> uebungen = new List<Uebungen>();
            while (reader.Read())
            {
                string aufgabename = ExportData.ExportNameDerAufgabe(reader);
                aufgabename = Functions.ReplaceStringToText(aufgabename);
                int aufgabennummer = Int32.Parse(ExportData.ExportUebungsnummer(reader));
                string id = ExportData.ExportID(reader);
                Uebungen uebung = new Uebungen(aufgabename, aufgabennummer, id);
                uebungen.Add(uebung);
            }
            for (int i = 0; i < uebungen.Count; i++)
            {
                Functions.ConsoleWrite("\n" + uebungen[i].getName() + " || " + uebungen[i].getAufgabennummer() + " || ID: " + uebungen[i].GetId(), ConsoleColor.DarkRed);
            }
            Functions.ConsoleWrite("Welche Aufgaben moechten sie tauschen? Bitte geben Sie die IDs an [1,2]", ConsoleColor.DarkBlue);
            getUserInput = Console.ReadLine();
            userInputArray = getUserInput.Split(",");
            for (int i = 0; i < userInputArray.Length; i++)
            {
                foreach (Uebungen uebungsaufgaben in uebungen)
                {
                    int uebungsnummer = 0;
                    if (uebungsaufgaben.GetId().Equals(userInputArray[i]))
                    {
                         uebungsnummer = uebungsaufgaben.getAufgabennummer();
                    }
                    sql = "update MKB set Uebungsnummer=" + userInputArray[i] + " where ID='" + uebungsnummer + "' ";
                    Console.WriteLine(sql);
                    Functions.sqlStatement(sql);
                }
            }
        }
    }
}
