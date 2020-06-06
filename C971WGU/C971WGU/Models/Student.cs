using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971WGU.Models
{
    public class Student
    {
        [PrimaryKey]
        public int studentID { get; set; }
        public string studentEmail { get; set; }
        public string studentPhone { get; set; }
        public string studentFirstName { get; set; }
        public string studentLastName { get; set; }

        // Build Student Info
        public void BuildStudent(string[] studentText)
        {
            studentFirstName = studentText[0];
            studentLastName = studentText[1];
            studentPhone = studentText[2];
            studentEmail = studentText[3];
            studentID = 1;
        }

        // Build Default Profile
        public void CreateInitStudent()
        {
            Controllers.DataCon initStudentDB = new Controllers.DataCon();
            initStudentDB.dbCon.Insert(this);
        }

        // Lookup Student Info
        public Student LookupStudent()
        {
            Controllers.DataCon lookupStudentDB = new Controllers.DataCon();
            return lookupStudentDB.dbCon.FindWithQuery<Student>("SELECT * FROM Student WHERE studentID = 1;");
        }

        // Edit Student Profile Information
        public void EditStudent()
        {
            Controllers.DataCon editStudentDB = new Controllers.DataCon();
            editStudentDB.dbCon.Update(this);
        }
    }
}
