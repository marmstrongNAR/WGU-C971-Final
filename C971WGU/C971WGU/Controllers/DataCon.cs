using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Data;
using SQLite;

namespace C971WGU.Controllers
{
    public class DataCon
    {
        public SQLiteConnection dbCon;
         
        // Constructor
        public DataCon()
        {
            GetCon();
            TableInit();
        }

        // Build DB Connection
        private void GetCon() // System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)
        {
            string dbName = "C971.db";
            string dbFolder = "storage/1A07-3510/Android/data/com.companyname.c971wgu/";
            string dbPath = Path.Combine(dbFolder, dbName);

            dbCon = new SQLite.SQLiteConnection(dbPath);
        }

        // Build Tables If Not Existing
        private void TableInit()
        {
            dbCon.CreateTable<Models.Student>();
            dbCon.CreateTable<Models.Instructor>();
            dbCon.CreateTable<Models.Term>();
            dbCon.CreateTable<Models.Course>();
            dbCon.CreateTable<Models.Assessment>();
        }
    }
}
