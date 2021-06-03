using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace Latex_Studienarbeit
{
    class AddNewTask
    {
        public static SQLiteConnection m_dbConnection = new SQLiteConnection(@"Data Source=..\..\..\..\MKB.sqlite;Version=3;");

        public static void AddTask()
        {
            m_dbConnection.Open();
            Functions.ConsoleWrite("In welcher Übungseinheit wollen Sie eine neue Aufgabe hinzufügen?", ConsoleColor.DarkBlue);
            string getInput = Console.ReadLine();
            string[] allInput = getInput.Split(',');
            Functions.AllUebungenFromNumner(allInput);
            Functions.ConsoleWrite("Nach welcher ID möchten Sie die neue Aufgabe hinzufügen?", ConsoleColor.DarkBlue);
            string getUserInput = Console.ReadLine();
            string sql = "SELECT COUNT(*) FROM MKB";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            int RowCount = Convert.ToInt32(command.ExecuteScalar());
            RowCount += 1;
            int newId = RowCount;
            sql = "insert into MKB (ID) values (" + RowCount + ")";
            Functions.sqlStatement(sql);
            int id = Int32.Parse(getUserInput) + 1;
            RowCount = RowCount - 1 - id;
            List<Uebungen> uebungen = new List<Uebungen>();
            Uebungen newUebung = new Uebungen("name", 2, id);
            uebungen.Add(newUebung);
            for (int i = 0; i <= RowCount; i++)
            {
                int getid = id + i;
                sql = "select * from MKB where ID=" + getid + "";
                command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    uebungen.Add(ExportFromDB.ExportAll(reader));
                }
            }
            int idnumber = id;
            for (int i = 1; i < uebungen.Count; i++)
            {
                Functions.ConsoleWrite(uebungen[i].GetId().ToString() + "||" + uebungen[i].GetAufgabennummer().
                    ToString() + "||" + idnumber.ToString(), ConsoleColor.Red);
                int iduebungen = (uebungen[i].GetId()) + 1;
                int uebungsnummer = uebungen[i].GetAufgabennummer();
                sql = "update MKB set NamederAufgabe='" + Functions.ReplaceStringToDB(uebungen[i].GetName()) + "', Uebungseinheit=" + uebungen[i].GetUebungseinheit() + ", Uebungsnummer=" + uebungsnummer + ",  Uebungsart='" + uebungen[i].GetUebungsart() + "', WirdVerwendet=0, NamederAufgabe='" + Functions.ReplaceStringToDB(uebungen[i].GetName()) + "', Uebungsaufgabe='" + Functions.ReplaceStringToDB(uebungen[i].GetAufgabe()) + "', Loesung='" + Functions.ReplaceStringToDB(uebungen[i].GetLoesung()) + "'  where ID=" + iduebungen + "";
                Functions.sqlStatement(sql);
                //sql = "update MKB set Uebungsnummer=" + uebungsnummer + " where ID=" + iduebungen + " AND Uebungsart='"+ uebungen[i].GetUebungsart() + "'";

                idnumber++;
            }

            CreateNewPath();
            Console.WriteLine("Tippen Sie 'weiter' sobald Sie die Übungsaufgabe geändert haben.");
            getUserInput = Console.ReadLine();
            string[] fileinput = GetFileInput();
            string loesung = "";
            int uebungseinheit = Int32.Parse(allInput[0]);
            sql = "update MKB set Uebungseinheit=" + uebungseinheit + ", Uebungsnummer=" + uebungen[1].GetAufgabennummer() + ",  Uebungsart='" + uebungen[1].GetUebungsart() + "', WirdVerwendet=0, NamederAufgabe='" + Functions.ReplaceStringToDB(fileinput[0]) + "', Uebungsaufgabe='" + Functions.ReplaceStringToDB(fileinput[1]) + "', Loesung='" + loesung + "'  where ID=" + id + "";
            Functions.sqlStatement(sql);
            m_dbConnection.Close();
        }
        public static void CreateNewPath()
        {
            string nameDerAufgabe = "HIER SOLLTE DER NAME DER AUFGABE STEHEN \n";
            string trennung = "%SPLITAufgabe \n";
            string aufgabe = "HIER SOLLTE DIE AUFGABE STEHEN";
            string[] allInput = { nameDerAufgabe, trennung, aufgabe };

            try
            {
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(@"..\..\..\..\" + "neueAufgabe.tex"))
                {
                    for (int i = 0; i < allInput.Length; i++)
                    {
                        byte[] info = new UTF8Encoding(true).GetBytes(allInput[i]);
                        // Add some information to the file.
                        fs.Write(info, 0, info.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Functions.ConsoleWrite("Im Verzeichnis befindet sich eine neue .tex Datei mit dem Namen 'neue Aufgabe'. Bitte schreiben Sie den Code in die Felder", ConsoleColor.DarkGreen);
        }
        public static string[] GetFileInput()
        {
            string uebungseinheit = "";
            string line = "";
            string filename = @"..\..\..\..\" + "neueAufgabe.tex";
            System.IO.StreamReader file =
                new System.IO.StreamReader(filename);
            while ((line = file.ReadLine()) != null)
            {
                uebungseinheit += line + "\n";
            }
            //AufgabenMitLoesung beinhaltet eine U-Aufgabe mit der zugehoerigen Loesung
            string[] aufgabenMitLoesungen = uebungseinheit.Split(new string[] { "SPLITAufgabe" }, StringSplitOptions.RemoveEmptyEntries);
            return aufgabenMitLoesungen;
        }
    }
}
