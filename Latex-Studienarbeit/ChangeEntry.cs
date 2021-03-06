using System;
using System.Data.SQLite;
using System.Collections.Generic;
namespace Latex_Studienarbeit
{
    class ChangeEntry
    {
        public static SQLiteConnection m_dbConnection = new SQLiteConnection(@"Data Source=..\..\..\..\MKB.sqlite;Version=3;");
        public static void UpdateTexEntry()
        {
            Functions.ConsoleWrite("Tippen Sie die Übungseinheit ein, bei der Sie die Aufgabe ändern wollen: ", ConsoleColor.DarkBlue);
            string getInput = Console.ReadLine();
            string[] allInput = getInput.Split(',');
            try
            {
                int[] splitInput = new int[allInput.Length];
                for(int m = 0; m<allInput.Length; m++)
                {
                    splitInput[m] = Int32.Parse(allInput[m]);
                }
            } catch(FormatException e)
            {
                Functions.ConsoleWrite("\n Diese Eingabe war leider ungueltig. \n", ConsoleColor.DarkRed);
                UpdateTexEntry();
            }
            Functions.AllUebungenFromNumner(allInput);
            Functions.ConsoleWrite("Welche Aufgabe möchten Sie ändern? Tippen Sie die ID ein: ", ConsoleColor.DarkBlue);
            string getUserInput = Console.ReadLine();
            int uebungseinheit = Int32.Parse(getUserInput);
            m_dbConnection.Open();
            String path = Functions.GetCurrentDate();
            string sql = "select Uebungsaufgabe from MKB where ID=" + uebungseinheit + "";
            string entryPath = path + "-uebungsaufgabe.tex";
            ExportData.CreatePath(sql, m_dbConnection, entryPath, 2);
            Functions.ConsoleWrite("Im Verzeichnis befindet sich eine neue '.tex' Datei mit dem Datum und dem Text Uebungsaufgabe.", ConsoleColor.DarkGreen);

            Functions.ConsoleWrite("Tippen Sie 'weiter' sobald Sie die Übungsaufgabe geändert haben.", ConsoleColor.DarkYellow);
            string next = Console.ReadLine();
            string line = "";   
            string uebungsaufgabe = "";
            //Console.WriteLine(now);
            Console.WriteLine(@"..\..\..\..\" + entryPath);
            System.IO.StreamReader file =
                new System.IO.StreamReader(@"..\..\..\..\"  + entryPath);
            while ((line = file.ReadLine()) != null)
            {
                Console.WriteLine("while");
                uebungsaufgabe += line + "\n";
            }
            uebungsaufgabe = Functions.ReplaceStringToDB(uebungsaufgabe);
            sql = "update MKB set Uebungsaufgabe=@aufgabe where ID=" + getUserInput + "";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.Parameters.AddWithValue("@aufgabe", uebungsaufgabe);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }
        public static void ChangeOrderinDatabase()
        {
            Functions.ConsoleWrite("Tippen Sie die Übungseinheiten ein, in der Sie etwas ändern möchten, z.B. [1,2]?", ConsoleColor.DarkBlue);
            string getInput = Console.ReadLine();
            string[] allInput = getInput.Split(',');
            try
            {
                int[] splitInput = new int[allInput.Length];
                for (int m = 0; m < allInput.Length; m++)
                {
                    splitInput[m] = Int32.Parse(allInput[m]);
                }
            }
            catch (FormatException e)
            {
                Functions.ConsoleWrite("\n Diese Eingabe war leider ungueltig. \n", ConsoleColor.DarkRed);
                ChangeOrderinDatabase();

            }
            Functions.AllUebungenFromNumner(allInput);
            Functions.ConsoleWrite("Welche Aufgaben möchten sie tauschen? Bitte geben Sie die IDs an [1,2]?", ConsoleColor.DarkBlue);
            string getUserInput = Console.ReadLine();
            string[] userInputArray = getUserInput.Split(',');
            UpdateDB(userInputArray);
        }
        public static void UpdateDB(string[] userInputArray)
        {
            m_dbConnection.Open();
            string sql = "";
            List<Uebungen> uebungen = new List<Uebungen>();
            Console.WriteLine(userInputArray.Length);
            for (int i = 0; i < userInputArray.Length; i++)
            {
                int id = Int32.Parse(userInputArray[i]);
                sql = "select * from MKB where ID=" + id + "";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    uebungen.Add(ExportFromDB.ExportAll(reader));
                }
            }
            sql = "update MKB set NamederAufgabe='" + Functions.ReplaceStringToDB(uebungen[0].GetName()) + "', Uebungsaufgabe='" + Functions.ReplaceStringToDB(uebungen[0].GetAufgabe()) + "', Loesung='" + Functions.ReplaceStringToDB(uebungen[0].GetLoesung()) + "'  where ID='" + userInputArray[1] + "' ";
            Functions.sqlStatement(sql);
            sql = "update MKB set NamederAufgabe='" + Functions.ReplaceStringToDB(uebungen[1].GetName()) + "', Uebungsaufgabe='" + Functions.ReplaceStringToDB(uebungen[1].GetAufgabe()) + "', Loesung='" + Functions.ReplaceStringToDB(uebungen[1].GetLoesung()) + "'  where ID='" + userInputArray[0] + "' ";
            Functions.sqlStatement(sql);
            m_dbConnection.Close();
        }
    }
}
