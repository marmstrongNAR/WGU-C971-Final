using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971WGU.Models
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int termID { get; set; }
        public string termTitle { get; set; }
        public DateTime termStart { get; set; }
        public DateTime termEnd { get; set; }

        public List<Course> coursesList = new List<Course>();

        // Build Term Info
        public void BuildTerm(string[] termText)
        {
            termTitle = termText[0];
            termStart = DateTime.Parse(termText[1]);
            termEnd = DateTime.Parse(termText[2]);
        }

        // Association Mapping
        public void BuildAssociatedCourses()
        {
            Controllers.TableHelper associationLookupTable = new Controllers.TableHelper();
            List<Course> associationList_term = associationLookupTable.GetTableData_course(false);

            for (int i = 0; i < associationList_term.Count; i++)
            {
                if (associationList_term[i].termID_ref == termID && !coursesList.Contains(associationList_term[i]))
                {
                    associationList_term[i].termID_ref = 0;
                    associationList_term[i].EditCourse();
                }
            }

            foreach (Course association in coursesList)
            {
                Console.WriteLine($"Updating : { association.courseID }");
                association.termID_ref = termID;
                association.EditCourse();
            }
        }

        // Lookup Existing Term
        public Term LookupTerm(int lookupID)
        {
            Controllers.DataCon lookupTermDB = new Controllers.DataCon();
            return lookupTermDB.dbCon.FindWithQuery<Term>($"SELECT * FROM Term WHERE termID = { lookupID }");
        }

        // Add New Term
        public void AddTerm()
        {
            Controllers.DataCon addTermDB = new Controllers.DataCon();
            addTermDB.dbCon.Insert(this);
        }

        // Edit Existing Term
        public void EditTerm()
        {
            Controllers.DataCon editTermDB = new Controllers.DataCon();
            editTermDB.dbCon.Update(this);
        }

        // Delete Existing Term
        public void DeleteTerm()
        {
            Controllers.DataCon deleteTermDB = new Controllers.DataCon();
            deleteTermDB.dbCon.Delete(this);
        }
    }
}
