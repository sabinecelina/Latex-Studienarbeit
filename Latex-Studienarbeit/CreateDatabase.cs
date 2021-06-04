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
        private static List<string> nameOfFiles = new List<string>();
        //public static void sortList()
        //{
        //    string[] array = new string[] { "p", "h", "t" };
        //    List<string> uebungen = new List<string>();
        //    for (int i = 0; i < 2; i++)
        //    {
        //        string ii = i.ToString();
        //        for (int j = 0; j < fileName.Count; j++)
        //        {
        //            if (fileName[j].Contains(ii))
        //            {
        //                Console.WriteLine(fileName[j]);
        //            }
        //        }
        //    }
        //}
        /** create Table MKB */
        public static void CreateDatabaseSQLite()
        {
            nameOfFiles = Functions.GetAllFiles(new DirectoryInfo(@"..\..\..\..\MKB-1\"));
            try
            {
                SQLiteConnection.CreateFile(@"..\..\..\..\MKB.sqlite");
                string sql = "create table MKB (ID integer, Uebungseinheit integer,Uebungsnummer integer, Uebungsart varchar, WirdVerwendet integer, NameDerAufgabe varchar, Uebungsaufgabe varchar, Loesung varchar)";
                Functions.sqlStatement(sql);
            } catch(Exception e)
            {
                Console.WriteLine("\n Ihre Datenbank ist in einem anderen Programm geöffnet, bitte schließen Sie das Programm, wenn Sie die Datenbank neu erstellen wollen. \n");
            }
        }

        /** insert information into Database*/
        public static void InsertIntoDatabase()
        {
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
                    string sql = "insert into MKB (ID, Uebungseinheit, Uebungsnummer,  Uebungsart, WirdVerwendet,  Uebungsaufgabe, Loesung) values ('" + id + "','" + getNumber + "', '" + number + "', '" + uebungsart + "', '0', '" + aufgabe[i] + "', '" + loesung[i] + "')";
                    Functions.sqlStatement(sql);
                    id++;
                }
            }
        }

    }
}
