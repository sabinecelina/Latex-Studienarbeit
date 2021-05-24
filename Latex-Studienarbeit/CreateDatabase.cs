using System;
using System.IO;
using System.Text;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Latex_Studienarbeit
{
    class CreateDatabase
    {
        private static List<string> fileName = new List<string>();
        private static List<Uebungen> uebungen_db = new List<Uebungen>();

        /** get all MKB Files in Folder MKB-1*/
        public static void GetAllFiles()
        {
            DirectoryInfo d = new DirectoryInfo(@"..\..\..\..\MKB-1\");//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.tex"); //Getting Text files
            foreach (FileInfo file in Files)
            {
                fileName.Add(file.Name);
            }
            fileName.Reverse();
            for (int i = 0; i < fileName.Count; i++)
            {
                Console.WriteLine(fileName[i]);
            }
        }
        /** create Table MKB */
        public static void CreateDatabaseSQLite()
        {
            GetAllFiles();
            SQLiteConnection.CreateFile(@"..\..\..\..\MKB.sqlite");
            string sql = "create table MKB (ID integer, Uebungseinheit integer,Uebungsnummer integer, Uebungsart varchar, WirdVerwendet integer, NameDerAufgabe varchar, Uebungsaufgabe varchar, Loesung varchar)";
            Functions.sqlStatement(sql);
        }

        /** insert information into Database*/
        public static void InsertIntoDatabase()
        {
            int id = 1;
            int pnumber = 1;
            int hnumber = 1;
            int tnumber = 1;
            for (int m = 0; m < fileName.Count; m++)
            {
                string uebungsart = fileName[m].Substring(0, 1);
                uebungsart = uebungsart.ToUpper();
                char getNumber = fileName[m][fileName[m].Length - 5];
                string uebungseinheit = "";
                string line = "";
                string filename = @"..\..\..\..\MKB-1\" + fileName[m];
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
                    aufgabenMitLoesungen[i] = Functions.ReplaceStringToDB(aufgabenMitLoesungen[i]);
                    //AufgabenUndLoesungen getrennt beinhaltet einen Array der Laenge 2, im ersten Eintrag befindet sich die Aufgabe, im zweiten Eintrag die Loesung
                    string[] aufgabenUndLoesungGetrennt = aufgabenMitLoesungen[i].Split(new string[] { "SPLITLoesung" }, StringSplitOptions.RemoveEmptyEntries);
                    aufgabe[i] = aufgabenUndLoesungGetrennt[0];
                    if (aufgabenUndLoesungGetrennt.Length == 1)
                    {
                        loesung[i] = "NULL";
                    } else
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
                    string sql = "insert into MKB (ID, Uebungseinheit, Uebungsnummer,  Uebungsart,WirdVerwendet,  Uebungsaufgabe, Loesung) values ('" + id + "','" + getNumber + "', '" + number + "', '" + uebungsart + "', '0', '" + aufgabe[i] + "', '" + loesung[i] + "')";
                    Functions.sqlStatement(sql);
                    id++;
                }
            }
        }

    }
}
