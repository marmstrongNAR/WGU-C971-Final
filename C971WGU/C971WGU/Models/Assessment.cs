using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971WGU.Models
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int assessmentID { get; set; }
        public string assessmentTitle { get; set; }
        public string assessmentType { get; set; }
        public bool assessmentNotifications { get; set; }
        public DateTime assessmentStart { get; set; }
        public DateTime assessmentEnd { get; set; }
        public int courseID_ref { get; set; }

        // Build Assessment Info
        public void BuildAssessment(string[] assessmentText, bool assessmentCat)
        {
            assessmentTitle = assessmentText[0];
            assessmentStart = DateTime.Parse(assessmentText[1]) + TimeSpan.Parse(assessmentText[2]);
            assessmentEnd = DateTime.Parse(assessmentText[3]) + TimeSpan.Parse(assessmentText[4]);

            if (assessmentCat == true)
            { assessmentType = "Objective"; }
            else 
            { assessmentType = "Performance"; }
        }

        // Lookup Existing Assessment
        public Assessment LookupAssessment(int lookupID)
        {
            Controllers.DataCon lookupAssessmentDB = new Controllers.DataCon();
            return lookupAssessmentDB.dbCon.FindWithQuery<Assessment>($"SELECT * FROM Assessment WHERE assessmentID = { lookupID }");
        }

        // Add New Assessment
        public void AddAssessment()
        {
            Controllers.DataCon addAssessmentDB = new Controllers.DataCon();
            addAssessmentDB.dbCon.Insert(this);

            Controllers.MessagingHelper addNotify = new Controllers.MessagingHelper();
            addNotify.DueDateNotify_assessment();
        }

        // Edit Existing Assessment
        public void EditAssessment()
        {
            Controllers.DataCon editAssessmentDB = new Controllers.DataCon();
            editAssessmentDB.dbCon.Update(this);

            Controllers.MessagingHelper editNotify = new Controllers.MessagingHelper();
            editNotify.DueDateNotify_assessment();
        }

        // Delete Existing Assessment
        public void DeleteAssessment()
        {
            Controllers.DataCon deleteAssessmentDB = new Controllers.DataCon();
            deleteAssessmentDB.dbCon.Delete(this);
        }
    }
}
