using System;
using System.IO;
using System.Text;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Latex_Studienarbeit
{
    class CreateDatabase
    {
        private static string connectionPath = @"Data Source=..\..\..\..\MKB.sqlite;Version=3;";
        private static SQLiteConnection m_dbConnection = new SQLiteConnection(connectionPath);
        private static List<string> nameOfFiles = new List<string>();
        public static void CreateDatabaseSQLite()
        {
            nameOfFiles = Functions.GetAllFiles(new DirectoryInfo(@"..\..\..\..\MKB-1\"));
            try
            {
                SQLiteConnection.CreateFile(@"..\..\..\..\MKB.sqlite");
                string sql = "create table MKB (ID integer, Uebungseinheit integer,Uebungsnummer integer, Uebungsart varchar, NameDerAufgabe varchar, Uebungsaufgabe varchar, Loesung varchar)";
                Functions.sqlStatement(sql);
                Functions.ConsoleWrite("Die Datenbank wurde erfolgreich erstellt.", ConsoleColor.DarkGreen);
            }
            catch (Exception e)
            {
                Functions.ConsoleWrite("\n Ihre Datenbank ist in einem anderen Programm geöffnet, bitte schließen Sie das Programm, wenn Sie die Datenbank neu erstellen wollen. \n", ConsoleColor.DarkRed);
            }
        }

        /** insert information into Database*/
        public static void InsertIntoDatabase()
        {
            m_dbConnection.Open();

            int id = 1;
            int pnumber = 1;
            int hnumber = 1;
            int tnumber = 1;
            for (int m = 0; m < nameOfFiles.Count; m++)
            {
                string uebungsart = nameOfFiles[m].Substring(1, 1);
                uebungsart = uebungsart.ToUpper();
                char getNumber = nameOfFiles[m][nameOfFiles[m].Length - 5];
                string uebungseinheit = "";
                string line = "";
                string filename = @"..\..\..\..\MKB-1\" + nameOfFiles[m];
                System.IO.StreamReader file =
                    new System.IO.StreamReader(filename);
                while ((line = file.ReadLine()) != null)
                {
                    uebungseinheit += line +"\n";
                }
                //AufgabenMitLoesung beinhaltet eine U-Aufgabe mit der zugehoerigen Loesung
                string[] aufgabenMitLoesungen = uebungseinheit.Split(new string[] { "SPLITAufgabe" }, StringSplitOptions.RemoveEmptyEntries);
                string[] aufgabe = new string[aufgabenMitLoesungen.Length];
                string[] loesung = new string[aufgabenMitLoesungen.Length];
                for (int i = 0; i < aufgabenMitLoesungen.Length; i++)
                {
                    int number = 0;
                    //zuerst werden die Eintraege Datenbanksicher umgeschrieben
                    aufgabenMitLoesungen[i] = Functions.ReplaceStringToDB(aufgabenMitLoesungen[i]);
                    //AufgabenUndLoesungen getrennt beinhaltet einen Array der Laenge 2, im ersten Eintrag befindet sich die Aufgabe, im zweiten Eintrag die Loesung
                    string[] aufgabenUndLoesungGetrennt = aufgabenMitLoesungen[i].Split(new string[] { "SPLITLoesung" }, StringSplitOptions.RemoveEmptyEntries);
                    aufgabe[i] = aufgabenUndLoesungGetrennt[0];
                    if (aufgabenUndLoesungGetrennt.Length == 1)
                    {
                        loesung[i] = "%zu dieser Aufgabe existiert noch keine Loesung";
                    }
                    else
                    {
                        loesung[i] = aufgabenUndLoesungGetrennt[1];
                    }
                    switch (uebungsart)
                    {
                        case "P":
                            number = pnumber;
                            pnumber++;
                            break;
                        case "H":
                            number = hnumber;
                            hnumber++;
                            break;
                        case "T":
                            number = tnumber;
                            tnumber++;
                            break;
                    }
                    string sql = "insert into MKB (ID, Uebungseinheit, Uebungsnummer,  Uebungsart,  Uebungsaufgabe, Loesung) values (@id, @ueinheit, @un, @ua, @aufgabe, @loesung)";
                    //Functions.sqlStatement(sql);
                    //string sql = "insert into MKB (ID, Uebungseinheit, Uebungsnummer,  Uebungsart,  Uebungsaufgabe, Loesung) values (@id, @ue, @un, @ua, @aufgabe, @loesung)";
                    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@ueinheit", getNumber.ToString());
                    command.Parameters.AddWithValue("@un", number);
                    command.Parameters.AddWithValue("@ua", uebungsart);
                    command.Parameters.AddWithValue("@aufgabe", aufgabe[i]);
                    command.Parameters.AddWithValue("@loesung", loesung[i]);
                    command.ExecuteNonQuery();
                    id++;
                }
            }
            m_dbConnection.Close();
        }
        public static void CreateAndInsertIntoDatabase()
        {
            // Creates the Database and Insert Data in Database with reading the name of the task from json
            try
            {
                CreateDatabase.CreateDatabaseSQLite();
                CreateDatabase.InsertIntoDatabase();
                ReadJson.readJson();
                ReadJson.ChangeDatabaseEntry();
            }
            catch (Exception e)
            {
                Functions.ConsoleWrite("Etwas ist schiefgelaufen. Bitte starten Sie das Programm erneut.", ConsoleColor.DarkRed);
                Console.WriteLine(e);
            }
        }

    }
}
