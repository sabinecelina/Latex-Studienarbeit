using System;
using System.IO;
using System.Text;
using System.Data.SQLite;
using System.Collections.Generic;
namespace Latex_Studienarbeit
{
    class MainSelection

    {
        private static string[] uebungsart = new string[] { "P", "H", "T" };
        private static SQLiteConnection m_dbConnection = new(@"Data Source=..\..\..\..\MKB.sqlite;Version=3;");
        public static void Read()
        {
            Functions.ConsoleWrite("Möchten Sie eine Aufgabe bearbeiten (1), Aufgabenreihenfolgen ändern oder verschieben (2), Aufgaben exportieren (3), eine neue Aufgabe hinzufügen (4) oder alle Lösungen importieren (5)?", ConsoleColor.DarkBlue);
            try
            {
                int auswahl = Convert.ToInt32(Console.ReadLine());
                if (auswahl == 1)
                {
                    m_dbConnection.Open();
                    ChangeEntry.UpdateTexEntry();
                    m_dbConnection.Close();
                    Functions.ConsoleWrite("Sie haben erfolgreich den Eintrag geändert. Möchten Sie noch eine Funktion ausführen? [J,N]", ConsoleColor.DarkGreen);
                    string getUserInput = Console.ReadLine().ToUpper();
                    if (getUserInput.Equals("J"))
                        Read();
                }
                if (auswahl == 2)
                {
                    m_dbConnection.Open();
                    ChangeEntry.ChangeOrderinDatabase();
                    m_dbConnection.Close();
                    Functions.ConsoleWrite("Sie haben erfolgreich zwei Übungen miteinander vertauscht", ConsoleColor.DarkGreen);
                    string getUserInput = Console.ReadLine().ToUpper();
                    if (getUserInput.Equals("J"))
                        Read();
                }
                if (auswahl == 3)
                {
                    m_dbConnection.Open();
                    Functions.ConsoleWrite("Welche Übungseinheit in Nummer möchten Sie exportieren?", ConsoleColor.DarkBlue);
                    int uebungseinheit_number = Convert.ToInt32(Console.ReadLine());
                    AskIfExist(m_dbConnection, uebungseinheit_number);
                    Functions.ConsoleWrite("Moechten Sie die Übungseinheiten mit Lösungen(1) exportieren oder nur die Übungsaufgaben(2) oder nur die Lösungen(3)?", ConsoleColor.DarkBlue);

                    int numberUserInput = Convert.ToInt32(Console.ReadLine());
                    ExportFromDB.ExportFiles(m_dbConnection, numberUserInput, uebungseinheit_number);
                    m_dbConnection.Close();
                    Functions.ConsoleWrite("Es wurden neue Dateien angelegt.", ConsoleColor.DarkGreen);
                    string getUserInput = Console.ReadLine().ToUpper();
                    if (getUserInput.Equals("J"))
                        Read();
                }
                if (auswahl == 5)
                {
                    Functions.ConsoleWrite("Bitte laden Sie alle Lösungen in den Ordner Loesungen und tippen sie anschließend weiter", ConsoleColor.DarkBlue);
                    string weiter = Console.ReadLine();
                    weiter = weiter.ToUpper();
                    if (weiter.Equals("WEITER"))
                        GetLoesungen.SendLoesungenToDB();
                    Functions.ConsoleWrite("Die Loesungen wurden erfolgreich hochgeladen", ConsoleColor.DarkBlue);
                }
                if (auswahl > 5)
                    throw new Exception();
            }
            catch (Exception e)
            {
                Console.Write(e);
                Functions.ConsoleWrite("Diese Eingabe war leider ungueltig. \n", ConsoleColor.DarkYellow);
                Read();
            }
        }


        public static void AskIfExist(SQLiteConnection m_dbConnection, int number)
        {
            for (int i = 0; i < uebungsart.Length; i++)
            {
                string sql = "select exists(select Uebungsaufgabe from MKB where Uebungsart='" + uebungsart[i] + "' AND Uebungseinheit=" + number + ")";
                SQLiteCommand command = new(sql, m_dbConnection);
                command.ExecuteNonQuery();
                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count == 0)
                {
                    Functions.ConsoleWrite("Warnung! Bitte beachten Sie: Es existieren keine " + uebungsart[i] + " Uebungen in der Uebungseinheit " + number + " \n", ConsoleColor.DarkYellow);
                }
            }

        }

    }
}
