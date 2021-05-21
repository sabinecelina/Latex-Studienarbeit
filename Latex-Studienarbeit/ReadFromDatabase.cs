using System;
using System.IO;
using System.Text;
using System.Data.SQLite;
using System.Collections.Generic;
namespace Latex_Studienarbeit
{
    class ReadFromDatabase

    {
        private ExportData exportData = new ExportData();
        private static List<string> uebungen;
        private static string[] uebungsart = new string[] { "P", "H", "T" };
        public static void GetAllFiles()
        {
            DirectoryInfo d = new DirectoryInfo(@"..\..\..\..\MKB-1\");//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.tex"); //Getting Text files
            foreach (FileInfo file in Files)
            {
                uebungen.Add(file.Name);
            }
        }
        public static void Read()
        {
            SQLiteConnection m_dbConnection = new SQLiteConnection(@"Data Source=..\..\..\..\MKB.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "";
            ConsoleWrite("Moechten Sie eine Aufgabe bearbeiten(1) oder Aufgaben exportieren(2)?", ConsoleColor.DarkBlue);
            int auswahl = Convert.ToInt32(Console.ReadLine());
            if(auswahl == 1)
            {
                ChangeEntry.UpdateTexEntry();
            }
            ConsoleWrite("Welche Uebungseinheit in Nummer moechten Sie exportieren?", ConsoleColor.DarkBlue);
            int numberUebungseinheitUserInput = Convert.ToInt32(Console.ReadLine());
            AskIfExist(m_dbConnection, numberUebungseinheitUserInput);

            ConsoleWrite("Moechten Sie die Uebungseinheiten mit Loesungen(1) exportieren oder nur die Uebungsaufgaben(2) oder nur die Loesungen(3)?", ConsoleColor.DarkBlue);
            int numberUserInput = Convert.ToInt32(Console.ReadLine());
            if (numberUserInput == 1)
            {
                ConsoleWrite("Welche Uebungsaufgaben moechten Sie exportieren? Geben Sie jeweils P oder H oder T an. Um alle Aufgaben zu exportieren schreiben Sie PH, PHT, HT, etc.", ConsoleColor.DarkBlue);
                string userInput = Console.ReadLine();
                ExportData.ExportUebungenAndLoesungen(userInput, m_dbConnection, numberUebungseinheitUserInput);
            }
            else if (numberUserInput == 2)
            {
                ConsoleWrite("Welche Uebungsaufgaben moechten Sie exportieren? Geben Sie jeweils P oder H oder T an. Um alle Aufgaben zu exportieren schreiben Sie PH, PHT, HT, etc.", ConsoleColor.DarkBlue);
                string userInput2 = Console.ReadLine();
                ExportData.ExportUebungen(userInput2, m_dbConnection);
            }
            else if (numberUserInput == 3)
            {
                ExportData.ExportLoesungenTex(m_dbConnection);
            }
            else ConsoleWrite("Ihre Angaben konnten nicht gelesen werden, bitte versuchen Sie es erneut", ConsoleColor.DarkBlue);
            m_dbConnection.Close();
        }


        public static void AskIfExist(SQLiteConnection m_dbConnection, int number)
        {
            for (int i = 0; i < uebungsart.Length; i++)
            {
                string sql = "select exists(select Uebungsaufgabe from MKB where Uebungsart='" + uebungsart[i] + "' AND Uebungseinheit="+ number+")";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count == 0)
                {
                    ConsoleWrite("Bitte beachten Sie: Es existieren keine " + uebungsart[i] + " Uebungen in der Uebungseinheit "+ number+ " \n", ConsoleColor.Red);
                }
            }

        }
        public static void ConsoleWrite(string text, ConsoleColor backgroundcolor)
        {
            Console.BackgroundColor = backgroundcolor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
