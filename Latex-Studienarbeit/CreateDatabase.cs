using System;
using System.IO;
using System.Text;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Latex_Studienarbeit
{
    class CreateDatabase
    {
        private static string connectionPath = @"Data Source=..\..\..\..\MKB.sqlite;Version=3;";
        private static List<string> uebungen = new List<string>();
        public static void GetAllFiles()
        {
            DirectoryInfo d = new DirectoryInfo(@"..\..\..\..\MKB-1\");//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.tex"); //Getting Text files
            foreach (FileInfo file in Files)
            {
                uebungen.Add(file.Name);
            }
            uebungen.Reverse();
            for (int i = 0; i < uebungen.Count; i++)
            {
                Console.WriteLine(uebungen[i]);
            }
        }
        public static void CreateDatabaseSQLite()
        {
            GetAllFiles();
            SQLiteConnection.CreateFile(@"..\..\..\..\MKB.sqlite");

            SQLiteConnection m_dbConnection = new SQLiteConnection(connectionPath);
            m_dbConnection.Open();

            string sql = "create table MKB (Uebungseinheit integer,Uebungsnummer integer, Uebungsart varchar, WirdVerwendet integer, NameDerAufgabe varchar, Uebungsaufgabe varchar, Loesung varchar)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }
        public static void InsertIntoDatabase()
        {
            SQLiteConnection m_dbConnection = new SQLiteConnection(connectionPath);
            m_dbConnection.Open();
            // Read the file and display it line by line.
            int pnumber = 1;
            int hnumber = 1;
            int tnumber = 1;
            for (int m = 0; m < uebungen.Count; m++)
            {
                string uebungsart = uebungen[m].Substring(0, 1);
                uebungsart = uebungsart.ToUpper();
                char var = uebungen[m][uebungen[m].Length - 5];
                string uebungseinheit = "";
                string line = "";
                string filename = @"..\..\..\..\MKB-1\" + uebungen[m];
                System.IO.StreamReader file =
                    new System.IO.StreamReader(filename);
                while ((line = file.ReadLine()) != null)
                {
                    uebungseinheit += "\n" + line;
                }
                //AufgabenMitLoesung beinhaltet eine U-Aufgabe mit der zugehoerigen Loesung
                string[] aufgabenMitLoesungen = uebungseinheit.Split(new string[] { "SPLITAufgabe" }, StringSplitOptions.RemoveEmptyEntries);
                string[] aufgabe = new string[aufgabenMitLoesungen.Length];
                string[] loesung = new string[aufgabenMitLoesungen.Length];
                for (int i = 0; i < aufgabenMitLoesungen.Length; i++)
                {
                    int number = 0;
                    //zuerst werden die Eintraege Datenbanksicher umgeschrieben
                    aufgabenMitLoesungen[i] = aufgabenMitLoesungen[i].Replace(@"\", "slash").Replace("\"", "anfuerungszeichen")
                                                .Replace("\'", "replacedonesign").Replace(@"$", "dollar");
                    //AufgabenUndLoesungen getrennt beinhaltet einen Array der Laenge 2, im ersten Eintrag befindet sich die Aufgabe, im zweiten Eintrag die Loesung
                    string[] aufgabenUndLoesungGetrennt = aufgabenMitLoesungen[i].Split(new string[] { "SPLITLoesung" }, StringSplitOptions.RemoveEmptyEntries);
                    aufgabe[i] = aufgabenUndLoesungGetrennt[0];
                    loesung[i] = aufgabenUndLoesungGetrennt[1];
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
                    string sql = "insert into MKB (Uebungseinheit, Uebungsnummer,  Uebungsart,WirdVerwendet,  Uebungsaufgabe, Loesung) values ('" + var + "', '" + number + "', '" + uebungsart + "', '0', '" + aufgabe[i] + "', '" + loesung[i] + "')";
                    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();
                    number++;
                }
            }
            m_dbConnection.Close();
        }

    }
}
