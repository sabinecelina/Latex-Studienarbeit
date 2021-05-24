using System.Data.SQLite;

namespace Latex_Studienarbeit
{
    class Functions
    {
        private static string connectionPath = @"Data Source=..\..\..\..\MKB.sqlite;Version=3;";
        private static SQLiteConnection m_dbConnection = new SQLiteConnection(connectionPath);

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
                                                .Replace("\'", "replacedonesign").Replace(@"$", "dollar");
        }
        public static string ReplaceStringToText(string text)
        {
            return text.Replace(@"\", "slash").Replace("\"", "anfuerungszeichen")
                                                .Replace("\'", "replacedonesign").Replace(@"$", "dollar");
        }
    }
}
