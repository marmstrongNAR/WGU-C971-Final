using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SQLite;
using Xamarin.Forms;

namespace C971WGU.Controllers
{
    public class Validator
    {
        private bool _isValid;
        private string _errorMessage;

        // Getters
        public bool isValid { get { return _isValid; } }
        public string errorMessage { get { return _errorMessage; } }

        // Checks Form Input For Nulls
        public bool CheckForNulls(string[] checkList)
        {
            _isValid = true;

            foreach (string txt in checkList)
            {
                if (txt == null || txt.Length == 0)
                {
                    _isValid = false;
                    _errorMessage = "Please Fill All Fields";
                }
            }

            return _isValid;
        }

        // Check Contact Information Validity
        public bool CheckContactValidity(string emailContext, string phoneContext)
        {
            _isValid = true;

            if (!emailContext.Contains("@") || !emailContext.Contains("."))
            {
                _isValid = false;
                _errorMessage = "Invalid Email Address";
            }

            string formattedPhone = string.Concat(phoneContext.Where(a => char.IsDigit(a)));

            if (formattedPhone.Length > 10)
            {
                _isValid = false;
                _errorMessage = "Invalid Phone Number";
            }

            return _isValid;
        }

        // Checks Date Fields For Validity
        public bool CheckDateValididty(DateTime startTime, DateTime endTime, int refID, bool isTerm)
        {
            _isValid = true;

            TableHelper lookupTimeTables = new TableHelper();
            List<Models.Term> timeTable_term = lookupTimeTables.GetTableData_term(false);

            if (isTerm == true)
            {
                for (int i = 0; i < timeTable_term.Count; i++)
                {
                    if ((startTime > timeTable_term[i].termStart && startTime < timeTable_term[i].termEnd) || (endTime > timeTable_term[i].termStart && endTime < timeTable_term[i].termEnd))
                    {
                        if (timeTable_term[i].termID != refID)
                        {
                            _isValid = false;
                            _errorMessage = $"Time Range Conflict";
                        }
                    }                    
                }
            }

            if (isTerm == false)
            {
                List<Models.Course> timeTable_course = lookupTimeTables.GetTableData_course(false);
                for (int i = 0; i < timeTable_course.Count; i++)
                {
                    if ((startTime > timeTable_course[i].courseStart && startTime < timeTable_course[i].courseEnd) || (endTime > timeTable_course[i].courseStart && endTime < timeTable_course[i].courseEnd))
                    {
                        if (timeTable_course[i].courseID != refID)
                        {
                            _isValid = false;
                            _errorMessage = $"Time Range Conflict";
                        }
                    }
                }
            }

            if (startTime > endTime)
            {
                _isValid = false;
                _errorMessage = "Start Time Cannont Occur After End Time";
            }

            return _isValid;
        }

        // Checks If Notifications Are Valid
        public bool CheckNotifications(DateTime dueDate, bool isEnabled)
        {
            _isValid = true;

            if (isEnabled == false) 
            { _isValid = false; }

            if (dueDate <= DateTime.Now)
            { _isValid = false; }

            return _isValid;
        }

        // Checks For Quantities of Foreign Keys
        public bool CheckKeyQty(int fkQty, bool isTerm)
        {
            _isValid = true;

            if (isTerm == true)
            {
                if (fkQty > 6) 
                { _isValid = false; }
            }
            else if (isTerm == false)
            {
                if (fkQty > 2)
                { _isValid = false; }
            }

            if (_isValid == false) 
            { _errorMessage = "Invalid Association Quantity"; }

            return _isValid;
        }

        // Checks To Ensure Correct Types Of Assessments
        public bool CheckAssessmentTypes(List<Models.Assessment> assessmentValidations)
        {
            _isValid = false;

            bool hasOA = false;
            bool hasPA = false;

            for (int i = 0; i < assessmentValidations.Count; i++) 
            {
                Console.WriteLine(assessmentValidations[i].assessmentType);
                if (assessmentValidations[i].assessmentType == "Performance") 
                { hasPA = true; }
                else if (assessmentValidations[i].assessmentType == "Objective") 
                { hasOA = true; }
            }

            if (hasOA == true && hasPA == true) 
            { _isValid = true; }
            else 
            { _errorMessage = "Course Must Include Both Objective and Performance Assessments"; }

            return _isValid;
        }

        // Checks If Object Exist In DB
        public bool CheckIfExists(string tableName, string lookupField, string lookupContext)
        {
            _isValid = false;

            DataCon validatorDB = new DataCon();
            List<int> queryList = validatorDB.dbCon.Query<int>($"SELECT { lookupField } FROM { tableName } { lookupContext };");

            if (queryList.Count > 0)
            {
                _isValid = true;
                _errorMessage = "Item Already Exists";
            }

            return _isValid;
        }
    }
}
