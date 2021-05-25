using System;
using System.IO;
using System.Text;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Latex_Studienarbeit
{
    class GetLoesungen
    {
        private static List<string> nameOfFiles = new List<string>();
        public static void SendLoesungenToDB()
        {
            nameOfFiles = Functions.GetAllFiles(new DirectoryInfo(@"..\..\..\..\Loesungen\"));
            foreach (string file in nameOfFiles)
            {
                string loesung = "";
                string line = "";
                string filename = @"..\..\..\..\Loesungen\" + file;
                System.IO.StreamReader files =
                    new System.IO.StreamReader(filename);
                while ((line = files.ReadLine()) != null)
                {
                    loesung += line + "\n";
                }
                string[] aufgabenUndLoesungGetrennt = file.Split(".tex", StringSplitOptions.RemoveEmptyEntries);
                int id = Int32.Parse(aufgabenUndLoesungGetrennt[0]);
                loesung = Functions.ReplaceStringToDB(loesung);
                string sql = "update MKB set Loesung='" + loesung + "' where ID=" + id + "";
                Functions.sqlStatement(sql);
            }
        }
    }
}
