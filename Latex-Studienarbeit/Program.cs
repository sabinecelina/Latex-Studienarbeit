using System;
using System.IO;

namespace Latex_Studienarbeit
{
    class Program
    {
        static void Main(string[] args)
        {
           CreateAndInsertIntoDatabase();
           //ReadFromDatabase.Read();
           //ChangeEntry.ChangeOrderinDatabase();
        }
        public static void CreateAndInsertIntoDatabase()
        {
            CreateDatabase.CreateDatabaseSQLite();
            CreateDatabase.InsertIntoDatabase();
            ReadJson.readJson();
            ReadJson.ChangeDatabaseEntry();
        }
    }
}
