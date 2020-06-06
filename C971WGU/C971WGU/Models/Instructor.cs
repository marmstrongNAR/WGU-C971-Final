using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971WGU.Models
{
    public class Instructor
    {
        [PrimaryKey, AutoIncrement]
        public int instructorID { get; set; }
        public string instructorFirstName { get; set; }
        public string instructorLastName { get; set; }
        public string instructorEmail { get; set; }
        public string instructorPhone { get; set; }

        public List<Course> instructorCourses;

        // Build Instructor Info
        public void BuildInstructor(string[] instructorText)
        {
            instructorFirstName = instructorText[0];
            instructorLastName = instructorText[1];
            instructorPhone = instructorText[2];
            instructorEmail = instructorText[3];
        }

        /*
        CRUD Methods 
        */

        // Lookup Existing Instructor
        public Instructor LookupInstructor(int lookupID)
        {
            Controllers.DataCon lookupInstructorDB = new Controllers.DataCon();
            return lookupInstructorDB.dbCon.FindWithQuery<Instructor>($"SELECT * FROM Instructor WHERE instructorID = { lookupID }");
        }

        // Add New Instructor
        public void AddInstructor()
        {
            Controllers.DataCon addInstructorDB = new Controllers.DataCon();
            addInstructorDB.dbCon.Insert(this);
        }

        // Edit Existing Instructor
        public void EditInstructor()
        {
            Controllers.DataCon editInstructorDB = new Controllers.DataCon();
            editInstructorDB.dbCon.Update(this);
        }

        // Delete Existing Instructor
        public void DeleteInstructor()
        {
            Controllers.DataCon deleteInstructorDB = new Controllers.DataCon();
            deleteInstructorDB.dbCon.Delete(this);
        }
    }
}
