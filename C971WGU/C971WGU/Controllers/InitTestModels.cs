using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace C971WGU.Controllers
{
    public class InitTestModels
    {
        public InitTestModels()
        { InitCreator(); }

        private void InitCreator()
        {
            Models.Student initStudent = new Models.Student();
            Models.Instructor initInstructor = new Models.Instructor();
            Models.Term initTerm = new Models.Term();
            Models.Course initCourse = new Models.Course();
            Models.Assessment initOA = new Models.Assessment();
            Models.Assessment initPA = new Models.Assessment();

            Validator initValidation = new Validator(); // Use Student ID as Init

            if (initValidation.CheckIfExists("Student", "studentID", "WHERE studentID = 1") == false)
            {
                // Create Init Student
                initStudent.studentID = 1;
                initStudent.studentEmail = "test@address.com";
                initStudent.studentPhone = "9876543210";
                initStudent.studentFirstName = "Test";
                initStudent.studentLastName = "Student";

                initStudent.CreateInitStudent();

                // Create Init Instructor
                initInstructor.instructorFirstName = "Michael";
                initInstructor.instructorLastName = "Armstrong";
                initInstructor.instructorPhone = "8136136448";
                initInstructor.instructorEmail = "marms83@wgu.edu";

                initInstructor.AddInstructor();

                // Create Init Term
                initTerm.termTitle = "Spring 2020";
                initTerm.termStart = DateTime.Now;
                initTerm.termEnd = DateTime.Now.AddDays(7);

                initTerm.AddTerm();

                // Create Init Course
                initCourse.courseTitle = "C971";
                initCourse.courseStatus = "Pending";
                initCourse.instructorID_ref = 1;
                initCourse.courseCredits = 3;
                initCourse.courseNotes = "Mobile Applications Development";
                initCourse.courseNotifications = false;
                initCourse.courseStart = DateTime.Now;
                initCourse.courseEnd = DateTime.Now.AddDays(4);
                initCourse.termID_ref = 1;

                initCourse.AddCourse();

                // Create Init OA
                initOA.assessmentTitle = "Test OA";
                initOA.assessmentStart = DateTime.Now.AddDays(1);
                initOA.assessmentEnd = initOA.assessmentStart.AddHours(3);
                initOA.courseID_ref = 1;
                initOA.assessmentType = "Objective";
                initOA.assessmentNotifications = false;

                initOA.AddAssessment();

                // Create Init PA
                initPA.assessmentTitle = "Test PA";
                initPA.assessmentStart = DateTime.Now.AddDays(2);
                initPA.assessmentEnd = initOA.assessmentStart.AddDays(1);
                initPA.courseID_ref = 1;
                initPA.assessmentType = "Performance";
                initPA.assessmentNotifications = false;

                initPA.AddAssessment();
            }

            /*if (initValidation.CheckIfExists("Instructor", "instructorID", "WHERE instructorID = 1") == false)
            {
                if (initValidation.CheckNextIncrement("Instructor") == 0)
                {

                }
            }

            if (initValidation.CheckIfExists("Term", "termID", "WHERE termID = 1") == false)
            {
                if (initValidation.CheckNextIncrement("Term") > 0)
                {
                    
                }
            }

            if (initValidation.CheckIfExists("Course", "courseID", "WHERE courseID = 1") == false)
            {
                if (initValidation.CheckNextIncrement("Course") > 0)
                {
                    
                }
            }

            if (initValidation.CheckIfExists("Assessment", "assessmentID", "WHERE assessmentID = 1") == false)
            {
                if (initValidation.CheckNextIncrement("Assessment") > 0)
                {
                    
                }
            }*/
        }
    }
}
