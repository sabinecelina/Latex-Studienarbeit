using System;
using System.IO;

namespace Latex_Studienarbeit
{
    class Program
    {
        static void Main(string[] args)
        {
            //createAndInsertIntoDatabase();
           ReadFromDatabase.Read();
           //ChangeEntry.ChangeOrderinDatabase();
        }
        public static void createAndInsertIntoDatabase()
        {
            CreateDatabase.CreateDatabaseSQLite();
            CreateDatabase.InsertIntoDatabase();
            ReadJson.readJson();
            ReadJson.ChangeDatabaseEntry();
        }
    }
}
