﻿using System;
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
                    Functions.ConsoleWrite("Sie haben erfolgreich den Eintrag geändert. Möchten Sie noch eine Funktion ausführen? [J,N]", ConsoleColor.DarkBlue);
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
                    Functions.ConsoleWrite("Sie haben erfolgreich den Eintrag geändert. Möchten Sie noch eine Funktion ausführen? [J,N]", ConsoleColor.DarkBlue);
                    string getUserInput = Console.ReadLine().ToUpper();
                    if (getUserInput.Equals("J"))
                        Read();
                }
                if (auswahl == 3)
                {
                    m_dbConnection.Open();
                    ExportFromDB.ExportFiles(m_dbConnection);
                    m_dbConnection.Close();
                    Functions.ConsoleWrite("Es wurden neue Dateien im Verzeichnis angelegt.", ConsoleColor.DarkGreen);
                    Functions.ConsoleWrite("Sie haben erfolgreich den Eintrag geändert. Möchten Sie noch eine Funktion ausführen? [J,N]", ConsoleColor.DarkBlue);
                    string getUserInput = Console.ReadLine().ToUpper();
                    if (getUserInput.Equals("J"))
                        Read();
                }
                if(auswahl == 4)
                {
                    AddNewTask.AddTask();
                    Functions.ConsoleWrite("Die neue Aufgabe wurde erfolgreich hochgeladen", ConsoleColor.DarkBlue);
                    Functions.ConsoleWrite("Sie haben erfolgreich den Eintrag geändert. Möchten Sie noch eine Funktion ausführen? [J,N]", ConsoleColor.DarkBlue);
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
                    throw new ExceptionHandler("Sie haben eine Zahl eingegeben, der keinen Befehl zugewiesen wurde.", ConsoleColor.DarkRed);
            }
            catch (System.FormatException e)
            {
                Functions.ConsoleWrite("\n Diese Eingabe war leider ungueltig. \n", ConsoleColor.DarkRed);
                Console.WriteLine(e);
                Read();
            }
            catch (ExceptionHandler e)
            {
                Read();
            }
        }

    }
}
