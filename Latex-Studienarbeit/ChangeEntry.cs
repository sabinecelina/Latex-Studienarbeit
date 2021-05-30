using System;
using System.Data.SQLite;

namespace Latex_Studienarbeit
{
    class ChangeEntry
    {
        public static SQLiteConnection m_dbConnection = new SQLiteConnection(@"Data Source=..\..\..\..\MKB.sqlite;Version=3;");
        public static void UpdateTexEntry()
        {
            m_dbConnection.Open();
            Functions.AllUebungenFromNumner();
            Functions.ConsoleWrite("Welche Aufgabe moechten Sie ändern? Tippen Sie die ID ein: ", ConsoleColor.DarkBlue);
            string getUserInput = Console.ReadLine();
            int uebungseinheit = Int32.Parse(getUserInput);
            string sql = "select Uebungsaufgabe from MKB where ID=" + uebungseinheit + "";
            string entryPath = "uebungsaufgabe.tex";
            ExportData.CreatePath(sql, m_dbConnection, entryPath, 2);
            Console.WriteLine("Tippen Sie 'weiter' sobald Sie die Übungsaufgabe geändert haben.");
            getUserInput = Console.ReadLine();
            if (getUserInput.Equals("weiter"))
            {
                string line = "";
                string uebungsaufgabe = "";
                System.IO.StreamReader file =
                    new System.IO.StreamReader(@"..\..\..\..\" + entryPath);
                while ((line = file.ReadLine()) != null)
                {
                    uebungsaufgabe += "\n" + line;
                }
                uebungsaufgabe = Functions.ReplaceStringToDB(uebungsaufgabe);
                sql = "update MKB set Uebungsaufgabe='" + uebungsaufgabe + "' where ID='" + getUserInput + "'";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
            }
            m_dbConnection.Close();
        }
        public static void ChangeOrderinDatabase()
        {
            Functions.AllUebungenFromNumner();
            Functions.ConsoleWrite("Welche Aufgaben möchten sie tauschen? Bitte geben Sie die IDs an [1,2]", ConsoleColor.DarkBlue);
            string getUserInput = Console.ReadLine();
            string[] userInputArray = getUserInput.Split(',');
            UpdateDB(userInputArray[0], userInputArray[1]);
            UpdateDB(userInputArray[1], userInputArray[0]);
        }
        public static void UpdateDB(string userInput, string user)
        {
            m_dbConnection.Open();
            int id = Int32.Parse(userInput);
            string sql = "select * from MKB where ID=" + id + "";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            string aufgabe = "";
            Uebungen uebung = new Uebungen();
            while (reader.Read())
            {
                uebung = ExportFromDB.ExportAll(reader);
            }
            sql = "update MKB set NamederAufgabe='" + Functions.ReplaceStringToDB(uebung.GetName()) + "', Uebungsaufgabe='" + Functions.ReplaceStringToDB(uebung.GetAufgabe()) + "', Loesung='" + Functions.ReplaceStringToDB(uebung.GetLoesung()) + "'  where ID='" + user + "' ";
            Functions.sqlStatement(sql);
            m_dbConnection.Close();
        }
    }
}
