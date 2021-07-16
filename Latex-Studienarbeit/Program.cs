using System;
using System.Data.SQLite;


namespace Latex_Studienarbeit
{
    class Program
    {
        static void Main(string[] args)
        {
            //CreateAndInsertIntoDatabase();
            AddNewTask.AddTask();
            //MainSelection.Read();
        }
        public static void CreateAndInsertIntoDatabase()
        {
            // Creates the Database and Insert Data in Database with reading the name of the task from json
            try
            {
                CreateDatabase.CreateDatabaseSQLite();
                CreateDatabase.InsertIntoDatabase();
                ReadJson.readJson();
                ReadJson.ChangeDatabaseEntry();
            }
            catch (Exception e)
            {
                Functions.ConsoleWrite("Etwas ist schiefgelaufen. Bitte starten Sie das Programm erneut.", ConsoleColor.DarkRed);
                Console.WriteLine(e);
            }
        }
    }
}
