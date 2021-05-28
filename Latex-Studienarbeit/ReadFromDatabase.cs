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
            while (true)
            {
                Functions.ConsoleWrite("Moechten Sie eine Aufgabe bearbeiten(1), Aufgabenreihenfolgen aendern oder verschieben (2),Aufgaben exportieren(3) oder eine neue Aufgabe hinzufuegen (4) oder alle Loesungen importieren(5)?", ConsoleColor.DarkBlue);
                try
                {
                    int auswahl = Convert.ToInt32(Console.ReadLine());
                    if (auswahl == 1)
                    {
                        ChangeEntry.UpdateTexEntry();
                        break;
                    }
                    if (auswahl == 2)
                    {
                        ChangeEntry.ChangeOrderinDatabase();
                        break;
                    }
                    if (auswahl == 3)
                    {
                        Functions.ConsoleWrite("Welche Uebungseinheit in Nummer moechten Sie exportieren?", ConsoleColor.DarkBlue);
                        int uebungseinheit_number = Convert.ToInt32(Console.ReadLine());
                        AskIfExist(m_dbConnection, uebungseinheit_number);
                        Functions.ConsoleWrite("Moechten Sie die Uebungseinheiten mit Loesungen(1) exportieren oder nur die Uebungsaufgaben(2) oder nur die Loesungen(3)?", ConsoleColor.DarkBlue);

                        int numberUserInput = Convert.ToInt32(Console.ReadLine());
                        ExportFiles(m_dbConnection, numberUserInput, uebungseinheit_number);
                        break;
                    }
                    if (auswahl == 5)
                    {
                        Functions.ConsoleWrite("Bitte laden Sie alle Loesungen in den Ordner Loesungen und tippen sie anschliessend weiter", ConsoleColor.DarkBlue);
                        string weiter = Console.ReadLine();
                        weiter = weiter.ToUpper();
                        if (weiter.Equals("WEITER"))
                            GetLoesungen.SendLoesungenToDB();
                        Functions.ConsoleWrite("Die Loesungen wurden erfolgreich hochgeladen", ConsoleColor.DarkBlue);
                        break;
                    }
                    if (auswahl > 5)
                        throw new Exception();
                }
                catch (Exception e)
                {
                    Functions.ConsoleWrite("Diese Eingabe war leider ungueltig. \n", ConsoleColor.DarkYellow);
                    continue;
                }
                break;
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
                    Functions.ConsoleWrite("Warnung! Bitte beachten Sie: Es existieren keine " + uebungsart[i] + " Uebungen in der Uebungseinheit " + number + " \n", ConsoleColor.DarkYellow);
                }
            }

        }
        public static void ExportFiles(SQLiteConnection m_dbConnection, int number, int auswahl)
        {
            if (number == 1 || number == 2)
            {
                while (true)
                {
                    try
                    {
                        Functions.ConsoleWrite("Welche Uebungsaufgaben moechten Sie exportieren? Geben Sie jeweils P oder H oder T an. Trennen Sie Ihre Angaben bitte mit einem ','", ConsoleColor.DarkBlue);
                        string userInput = Console.ReadLine();
                        string[] input = userInput.Split(",");
                        if (input.Length == 1)
                        {
                            throw new Exception();
                        }
                        else
                        {
                            ExportData.ExportUebungen(userInput, m_dbConnection, auswahl, number);
                        }
                    }
                    catch (Exception e)
                    {
                        Functions.ConsoleWrite("Diese Eingabe war leider ungueltig. Bitte versuchen Sie es erneut./n ", ConsoleColor.DarkYellow);
                        continue;
                    }
                    break;
                }
            }
            else if (number == 3)
            {
                ExportData.ExportLoesungenTex(m_dbConnection, auswahl);
            }
        }
    }
}
