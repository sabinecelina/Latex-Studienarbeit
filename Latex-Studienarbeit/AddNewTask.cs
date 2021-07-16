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
            Functions.ConsoleWrite("Welche Uebungsart soll diese Aufgabe haben [P,H,T]?", ConsoleColor.DarkBlue);
            string uebungsartInput = Console.ReadLine();
            uebungsartInput = uebungsartInput.ToUpper();
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
                int iduebungen = (uebungen[i].GetId()) + 1;
                int uebungsnummer = uebungen[i].GetAufgabennummer();
                sql = "update MKB set NamederAufgabe=@name, Uebungseinheit=@ueinheit, Uebungsnummer=@un, Uebungsart=@ua, Uebungsaufgabe=@aufgabe, Loesung=@loesung where ID=" + iduebungen + "";
                command = new SQLiteCommand(sql, m_dbConnection);
                command.Parameters.AddWithValue("@name", Functions.ReplaceStringToDB(uebungen[i].GetName()));
                command.Parameters.AddWithValue("@ueinheit", uebungen[i].GetUebungseinheit());
                command.Parameters.AddWithValue("@un", uebungsnummer);
                command.Parameters.AddWithValue("@ua", uebungen[i].GetUebungsart());
                command.Parameters.AddWithValue("@aufgabe", Functions.ReplaceStringToDB(uebungen[i].GetAufgabe()));
                command.Parameters.AddWithValue("@loesung", Functions.ReplaceStringToDB(uebungen[i].GetLoesung()));
                command.ExecuteNonQuery();
                uebungsnummer += 1;
                idnumber++;
            }
            string path = Functions.GetCurrentDate();
            path = path + "-neueAufgabe.tex";
            CreateNewPath(path);
            Console.WriteLine("Tippen Sie 'weiter' sobald Sie die Übungsaufgabe geändert haben.");
            getUserInput = Console.ReadLine();
            string[] fileinput = GetFileInput(path);
            string loesung = "";
            int uebungseinheit = Int32.Parse(allInput[0]);
            sql = "update MKB set Uebungseinheit=@ueinheit, Uebungsnummer = @un,  Uebungsart=@ua, NamederAufgabe=@name, Uebungsaufgabe=@aufgabe, Loesung=@loesung  where ID=" + id + "";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.Parameters.AddWithValue("@name", Functions.ReplaceStringToDB(fileinput[0]));
            command.Parameters.AddWithValue("@ueinheit", uebungseinheit);
            command.Parameters.AddWithValue("@un", uebungen[1].GetAufgabennummer());
            command.Parameters.AddWithValue("@ua", uebungsartInput);
            command.Parameters.AddWithValue("@aufgabe", Functions.ReplaceStringToDB(Functions.ReplaceStringToDB(fileinput[1])));
            command.Parameters.AddWithValue("@loesung", Functions.ReplaceStringToDB(loesung));
            command.ExecuteNonQuery();
            ChangeNumberOfNewTask(uebungsartInput);
            m_dbConnection.Close();
        }
        public static void CreateNewPath(string exportPath)
        {
            string nameDerAufgabe = "HIER SOLLTE DER NAME DER AUFGABE STEHEN \n";
            string trennung = "%SPLITAufgabe \n";
            string aufgabe = "HIER SOLLTE DIE AUFGABE STEHEN";
            string[] allInput = { nameDerAufgabe, trennung, aufgabe };

            try
            {
                String path = Functions.GetCurrentDate();
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(@"..\..\..\..\" + exportPath))
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
        public static string[] GetFileInput(string exportPath)
        {
            string uebungseinheit = "";
            string line = "";
            string filename = @"..\..\..\..\" + exportPath;
            System.IO.StreamReader file =
                new System.IO.StreamReader(filename);
            while ((line = file.ReadLine()) != null)
            {
                uebungseinheit += line + "\n";
            }
            //AufgabenMitLoesung beinhaltet eine U-Aufgabe mit der zugehoerigen Loesung
            string[] aufgabenMitLoesungen = uebungseinheit.Split(new string[] { "%SPLITAufgabe" }, StringSplitOptions.RemoveEmptyEntries);
            return aufgabenMitLoesungen;
        }
        public static void ChangeNumberOfNewTask(string uebungsart)
        {
            string sql = "SELECT COUNT(*) FROM MKB where Uebungsart='" + uebungsart + "'";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            int RowCount = Convert.ToInt32(command.ExecuteScalar());
            List<int> uebungen = new List<int>();
            int id = 0;
            sql = "select * from MKB where Uebungsart='" + uebungsart + "'";
            command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            int i = 0;
            while (reader.Read())
            {
                id = Int32.Parse(ExportFromDB.ExportID(reader));
                sql = "update MKB set Uebungsnummer = @un where ID=" + id + "";
                command = new SQLiteCommand(sql, m_dbConnection);
                command.Parameters.AddWithValue("@un", i + 1);
                command.ExecuteNonQuery();
                i++;

            }
        }
    }
}
