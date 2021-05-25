using System;
using System.IO;
using System.Text;
using System.Data.SQLite;
using System.Collections.Generic;
namespace Latex_Studienarbeit
{
    class ReadFromDatabase

    {
        private static string[] uebungsart = new string[] { "P", "H", "T" };
        private static SQLiteConnection m_dbConnection = new SQLiteConnection(@"Data Source=..\..\..\..\MKB.sqlite;Version=3;");
        public static void Read()
        {
            m_dbConnection.Open();
            Functions.ConsoleWrite("Moechten Sie eine Aufgabe bearbeiten(1), Aufgaben exportieren(2) oder eine neue Aufgabe hinzufuegen (3) oder alle Loesungen importieren(4)?", ConsoleColor.DarkBlue);
            int auswahl = Convert.ToInt32(Console.ReadLine());
            if (auswahl == 1)
            {
                ChangeEntry.UpdateTexEntry();
            }
            if (auswahl == 2)
            {
                Functions.ConsoleWrite("Welche Uebungseinheit in Nummer moechten Sie exportieren?", ConsoleColor.DarkBlue);
                int uebungseinheit_number = Convert.ToInt32(Console.ReadLine());
                AskIfExist(m_dbConnection, uebungseinheit_number);
                Functions.ConsoleWrite("Moechten Sie die Uebungseinheiten mit Loesungen(1) exportieren oder nur die Uebungsaufgaben(2) oder nur die Loesungen(3)?", ConsoleColor.DarkBlue);

                int numberUserInput = Convert.ToInt32(Console.ReadLine());
                ExportFiles(m_dbConnection, numberUserInput, uebungseinheit_number);
            }
            if (auswahl == 4)
            {
                Functions.ConsoleWrite("Bitte laden Sie alle Loesungen in den Ordner Loesungen und tippen sie anschliessend weiter", ConsoleColor.DarkBlue);
                string weiter = Console.ReadLine();
                weiter = weiter.ToUpper();
                if(weiter.Equals("WEITER"))
                GetLoesungen.SendLoesungenToDB();
                Functions.ConsoleWrite("Die Loesungen wurden erfolgreich hochgeladen", ConsoleColor.DarkBlue);
            }
            m_dbConnection.Close();
        }

        public static void AskIfExist(SQLiteConnection m_dbConnection, int number)
        {
            for (int i = 0; i < uebungsart.Length; i++)
            {
                string sql = "select exists(select Uebungsaufgabe from MKB where Uebungsart='" + uebungsart[i] + "' AND Uebungseinheit=" + number + ")";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count == 0)
                {
                    Functions.ConsoleWrite("Bitte beachten Sie: Es existieren keine " + uebungsart[i] + " Uebungen in der Uebungseinheit " + number + " \n", ConsoleColor.Red);
                }
            }

        }
        public static void ExportFiles(SQLiteConnection m_dbConnection, int number, int auswahl)
        {
            if (number == 1)
            {
                Functions.ConsoleWrite("Welche Uebungsaufgaben moechten Sie exportieren? Geben Sie jeweils P oder H oder T an. Trennen Sie Ihre Angaben bitte mit einem ','", ConsoleColor.DarkBlue);
                string userInput = Console.ReadLine();
                ExportData.ExportUebungen(userInput, m_dbConnection, auswahl, number);
            }
            else if (number == 2)
            {
                Functions.ConsoleWrite("Welche Uebungsaufgaben moechten Sie exportieren? Geben Sie jeweils P oder H oder T an. Trennen Sie Ihre Angaben bitte mit einem ','", ConsoleColor.DarkBlue);
                string userInput2 = Console.ReadLine();
                ExportData.ExportUebungen(userInput2, m_dbConnection, auswahl, number);
            }
            else if (number == 3)
            {
                ExportData.ExportLoesungenTex(m_dbConnection, auswahl);
            }
        }
    }
}
