using System;
using System.Data.SQLite;
using System.Collections;

namespace Latex_Studienarbeit
{
    class ExportFromDB
    {
        public static string ExportAufgaben(SQLiteDataReader reader)
        {
            string aufgabe = "";
            aufgabe = reader["Uebungsaufgabe"].ToString();
            aufgabe = Functions.ReplaceStringToText(aufgabe);
            return aufgabe;
        }
        public static string ExportLoesungen(SQLiteDataReader reader)
        {
            string loesung = reader["Loesung"].ToString();
            loesung = Functions.ReplaceStringToText(loesung);
            return loesung;
        }
        public static string ExportLoesungenForUser(SQLiteDataReader reader)
        {
            string loesunsgname = reader["NameDerAufgabe"].ToString();
            string loesung = reader["Loesung"].ToString();
            string id = reader["ID"].ToString();
            loesung = Functions.ReplaceStringToText(loesung);
            loesunsgname = Functions.ReplaceStringToText(loesunsgname);
            loesung = "%ID: " + id + " -- Loesungen zu: " + loesunsgname + "\n" + loesung;
            if (loesung.Equals("NULL"))
            {
                loesung = "%zu dieser Aufgabe existiert noch keine Loesung";
            }
            return loesung;
        }
        public static void ExportLoesungenTex(SQLiteConnection m_dbConnection, int number)
        {
            string sql;
            sql = "select ID, Loesung, NameDerAufgabe from MKB where Uebungsnummer='" + number + "'";
            ExportData.CreatePath(sql, m_dbConnection, "loesungen-" + number + ".tex", 3);
            Console.WriteLine("Es wurde eine neue loesung.tex Datei erstellt");
        }
        public static string ExportNameDerAufgabe(SQLiteDataReader reader)
        {
            string aufgabe = "";
            aufgabe = reader["NameDerAufgabe"].ToString();
            aufgabe = Functions.ReplaceStringToText(aufgabe);
            return aufgabe;
        }
        public static string ExportUebungsnummer(SQLiteDataReader reader)
        {
            string nummer = "";
            nummer = reader["Uebungsnummer"].ToString();
            return nummer;
        }
        public static string ExportID(SQLiteDataReader reader)
        {
            string id = "";
            id = reader["ID"].ToString();
            return id;
        }
        public static string ExportUebungseinheit(SQLiteDataReader reader)
        {
            string id = "";
            id = reader["Uebungseinheit"].ToString();
            return id;
        }
        public static string ExportUebungsart(SQLiteDataReader reader)
        {
            string id = "";
            id = reader["Uebungsart"].ToString();
            return id;
        }
        public static Uebungen ExportAll(SQLiteDataReader reader)
        {
            string uebungsart = ExportUebungsart(reader);
            string aufgabe = ExportAufgaben(reader);
            string loesung = ExportLoesungen(reader);
            string name = ExportNameDerAufgabe(reader);
            string uebungsnummer = ExportUebungsnummer(reader);
            int nummer = Int32.Parse(uebungsnummer);
            string id = ExportID(reader);
            int idnummer = Int32.Parse(id);
            int uebungseinheit = Int32.Parse(ExportUebungseinheit(reader));
            Uebungen uebung = new Uebungen(uebungseinheit, uebungsart, aufgabe, loesung, name, nummer, idnummer);
            return uebung;
        }
        public static void ExportFiles(SQLiteConnection m_dbConnection)
        {
            Functions.ConsoleWrite("Welche Übungseinheiten möchten Sie als .tex Datei anlegen [1,2,3]?", ConsoleColor.DarkBlue);
            string getInput = Console.ReadLine();
            string[] input = getInput.Split(',');
            int uebungseinheit_number = 0;
            Functions.ConsoleWrite("Möchten Sie die Übungseinheiten mit Lösungen [1] exportieren, nur die Übungsaufgaben [2] oder die Lösungen [3]?", ConsoleColor.DarkBlue);
            int numberUserInput = Convert.ToInt32(Console.ReadLine());
            for (int m = 0; m < input.Length; m++)
            {
                uebungseinheit_number = Int32.Parse(input[m]);
                string[] uebungen = new string[] {"praesuebg-", "hausuebg-", "tutorium-"};
                uebungen[0] = uebungen[0] + uebungseinheit_number + ".tex";
                uebungen[1] = uebungen[1] + uebungseinheit_number + ".tex";
                uebungen[2] = uebungen[2] + uebungseinheit_number + ".tex";
                if (numberUserInput == 1 || numberUserInput == 2)
                {
                    try
                    {
                        if(input.Length == 1)
                        {
                            ArrayList uebungsart = AskIfExist(m_dbConnection, uebungseinheit_number);
                            Functions.ConsoleWrite("Welche Uebungsaufgaben moechten Sie exportieren? Geben Sie jeweils P oder H oder T an. Trennen Sie Ihre Angaben bitte mit einem ','", ConsoleColor.DarkBlue);
                            string userInput = Console.ReadLine();
                            ExportData.ExportUebungen(userInput, m_dbConnection, uebungen, uebungseinheit_number, numberUserInput);
                        }
                        else
                        {
                            ArrayList uebungsart = AskIfExist(m_dbConnection, uebungseinheit_number);
                            foreach(string art in uebungsart)
                            {
                                ExportData.ExportUebungen(art, m_dbConnection, uebungen, uebungseinheit_number, numberUserInput);

                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Functions.ConsoleWrite("Diese Eingabe war leider ungueltig. Bitte versuchen Sie es erneut. \n ", ConsoleColor.DarkYellow);
                    }
                }
                else if (numberUserInput == 3)
                {
                    ExportFromDB.ExportLoesungenTex(m_dbConnection, uebungseinheit_number);
                }
            }
        }
        public static ArrayList AskIfExist(SQLiteConnection m_dbConnection, int number)
        {
            ArrayList uebungsart = new ArrayList { "P", "H", "T" };

            for (int i = 0; i < uebungsart.Count; i++)
            {
                string sql = "select exists(select Uebungsaufgabe from MKB where Uebungsart='" + uebungsart[i] + "' AND Uebungseinheit=" + number + ")";
                SQLiteCommand command = new(sql, m_dbConnection);
                command.ExecuteNonQuery();
                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count == 0)
                {
                    Functions.ConsoleWrite("Warnung! Bitte beachten Sie: Es existieren keine " + uebungsart[i] + " Uebungen in der Uebungseinheit " + number + " \n", ConsoleColor.DarkYellow);
                    uebungsart.RemoveAt(i);
                }
            }
            return uebungsart;
        }
    }
}
