using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Forms;

namespace C971WGU.Controllers
{
    public class TableHelper
    {
        private DataCon tableDB = new DataCon();
        public List<string> displayList = new List<string>();

        // Pulls List of Assessments For Table
        public List<Models.Assessment> GetTableData_assessment(bool forModal_assessment)
        {
            string getData = $"SELECT * FROM Assessment;";
            List<Models.Assessment> assessmentTableData = tableDB.dbCon.Query<Models.Assessment>(getData);

            if (forModal_assessment == true)
            {
                foreach (Models.Assessment displayCandidate in assessmentTableData) 
                { displayList.Add($"{ displayCandidate.assessmentID }. { displayCandidate.assessmentTitle }"); }
            }

            return assessmentTableData;
        }

        // Pulls List of Instructors For Table
        public List<Models.Instructor> GetTableData_instructor(bool forModal_instructor)
        {
            string getData = $"SELECT * FROM Instructor;";
            List<Models.Instructor> instructorTableData = tableDB.dbCon.Query<Models.Instructor>(getData);

            if (forModal_instructor == true) 
            {
                foreach (Models.Instructor displayCandidate in instructorTableData)
                { displayList.Add($"{ displayCandidate.instructorID }. { displayCandidate.instructorFirstName } { displayCandidate.instructorLastName }"); }
            }

            return instructorTableData;
        }

        // Pulls List of Terms For Table
        public List<Models.Term> GetTableData_term(bool forModal_term)
        {
            string getData = $"SELECT * FROM Term;";
            List<Models.Term> termTableData = tableDB.dbCon.Query<Models.Term>(getData);

            if (forModal_term == true)
            {
                foreach (Models.Term displayCandidate in termTableData) 
                { displayList.Add($"{ displayCandidate.termID }. { displayCandidate.termTitle }"); }
            }

            return termTableData;
        }

        // Pulls List of Courses This Term For Table
        public List<Models.Course> GetTableData_course(bool forModal_course)
        {
            string getData = $"SELECT * FROM Course";
            List<Models.Course> courseTableData = tableDB.dbCon.Query<Models.Course>(getData);

            if (forModal_course == true)
            {
                foreach (Models.Course displayCandidate in courseTableData) 
                { displayList.Add($"{ displayCandidate.courseID }. { displayCandidate.courseTitle }"); }
            }

            return courseTableData;
        }
    }
}
