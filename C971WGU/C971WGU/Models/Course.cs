using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971WGU.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int courseID { get; set; }
        public string courseTitle { get; set; }
        public string courseStatus { get; set; }
        public int courseCredits { get; set; }
        public string courseNotes { get; set; }
        public DateTime courseStart { get; set; }
        public DateTime courseEnd { get; set; }
        public bool courseNotifications { get; set; }
        public int instructorID_ref { get; set; }
        public int termID_ref { get; set; }

        public List<Assessment> assessmentsList = new List<Assessment>();

        // Build Course Info
        public void BuildCourse(string[] courseText)
        {
            courseTitle = courseText[0];
            courseStatus = courseText[1];
            instructorID_ref = Int32.Parse(courseText[2].Split('.')[0]);
            courseCredits = Int32.Parse(courseText[3]);
            courseNotes = courseText[4];
            courseStart = DateTime.Parse(courseText[5]);
            courseEnd = DateTime.Parse(courseText[6]);
        }

        // Association Mapping
        public void BuildAssociatedAssessments()
        {
            Controllers.TableHelper associationLookupTable = new Controllers.TableHelper();
            List<Assessment> associationList_assessment = associationLookupTable.GetTableData_assessment(false);

            for (int i = 0; i < associationList_assessment.Count; i++)
            {
                if (associationList_assessment[i].courseID_ref == courseID && !assessmentsList.Contains(associationList_assessment[i]))
                {
                    associationList_assessment[i].courseID_ref = 0;
                    associationList_assessment[i].EditAssessment();
                }
            }

            foreach (Assessment association in assessmentsList)
            {
                Console.WriteLine($"Updating : { courseID }");
                association.courseID_ref = courseID;
                association.EditAssessment();
            }
        }

        // Lookup Existing Course
        public Course LookupCourse(int lookupID)
        {
            Controllers.DataCon lookupCourseDB = new Controllers.DataCon();
            return lookupCourseDB.dbCon.FindWithQuery<Course>($"SELECT * FROM Course WHERE courseID = { lookupID }");
        }

        // Add New Course
        public void AddCourse()
        {
            Controllers.DataCon addCourseDB = new Controllers.DataCon();
            addCourseDB.dbCon.Insert(this);

            Controllers.MessagingHelper addNotify = new Controllers.MessagingHelper();
            addNotify.DueDateNotify_course();
        }

        // Edit Existing Course
        public void EditCourse()
        {
            Controllers.DataCon editCourseDB = new Controllers.DataCon();
            editCourseDB.dbCon.Update(this);

            Controllers.MessagingHelper addNotify = new Controllers.MessagingHelper();
            addNotify.DueDateNotify_course();
        }

        // Delete Existing Course
        public void DeleteCourse()
        {
            Controllers.DataCon deleteCourseDB = new Controllers.DataCon();
            deleteCourseDB.dbCon.Delete(this);
        }
    }
}
