using System;
using System.IO;
using System.Text;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Latex_Studienarbeit
{
    class Functions
    {
        private static string connectionPath = @"Data Source=..\..\..\..\MKB.sqlite;Version=3;";
        private static SQLiteConnection m_dbConnection = new SQLiteConnection(connectionPath);
        public static List<string> GetAllFiles(DirectoryInfo d)
        {
            List<string> fileName = new List<string>();
            FileInfo[] Files = d.GetFiles("*.tex"); //Getting Text files
            foreach (FileInfo file in Files)
            {
                fileName.Add(file.Name);
            }
            //sortList();
            fileName.Sort();
            return fileName;
        }
        public static void sqlStatement(string sql)
        {
            m_dbConnection.Open();
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }
        public static string ReplaceStringToDB(string text)
        {
            return text.Replace(@"\", "slash").Replace("\"", "anfuerungszeichen")
                                                .Replace("\'", "replacedonesign").Replace(@"$", "dollar").Replace("-", "minus").Replace("'", "anfuerungszeichen");
        }
        public static string ReplaceStringToText(string text)
        {
            return text.Replace("slash", @"\").Replace("anfuerungszeichen", "\"")
                .Replace("replacedonesign", "\'").Replace("dollar", @"$").Replace("minus", "-").Replace("anfuerungszeichen", "'");
        }
        public static void ConsoleWrite(string text, ConsoleColor backgroundcolor)
        {
            Console.BackgroundColor = backgroundcolor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
