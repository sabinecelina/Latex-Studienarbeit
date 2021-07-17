using System;
using System.Data.SQLite;


namespace Latex_Studienarbeit
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateDatabase.CreateAndInsertIntoDatabase();
            MainSelection.Read();
        }
    }
}
