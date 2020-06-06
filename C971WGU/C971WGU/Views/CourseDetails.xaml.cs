using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971WGU.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseDetails : ContentPage
    {
        private Controllers.ListHelper assessmentListHelper = new Controllers.ListHelper();
        private int workingID;

        // Constructor
        public CourseDetails(int idNum)
        {
            InitializeComponent();

            workingID = idNum;
            BuildLists();

            if (workingID != -1)
            {
                Models.Course templateCourse = new Models.Course();
                CourseBanner.Text = $"Edit Course ID: { workingID }";
                templateCourse = templateCourse.LookupCourse(workingID);
                BuildWorkingCourse(templateCourse);
            }
        }

        // Populate Status List
        private void BuildLists()
        {
            CourseStatusInput.Items.Add("Pending");
            CourseStatusInput.Items.Add("In Progress");
            CourseStatusInput.Items.Add("Completed");
            CourseStatusInput.Items.Add("Dropped");

            BuildInstructorList();
            BuildAssessmentList();
            BuildAssessmentsTable();
            BuildTermList();
        }

        // Populate Instructor List
        private void BuildInstructorList()
        {
            Controllers.TableHelper instructorLookupTable = new Controllers.TableHelper();
            List<Models.Instructor> instructorList = instructorLookupTable.GetTableData_instructor(true);
            List<string> lookupInstructorDisplay = instructorLookupTable.displayList;

            CourseInstructorInput.Items.Add("0. Add New Instructor");
            CourseInstructorInput.SelectedItem = "0. Add New Instructor";

            foreach (string instructorInfo in lookupInstructorDisplay) 
            { CourseInstructorInput.Items.Add(instructorInfo); }
        }

        // Populate Assessment List
        private void BuildAssessmentList()
        {
            Controllers.TableHelper assessmentLookupTable = new Controllers.TableHelper();
            List<Models.Assessment> assessmentList = assessmentLookupTable.GetTableData_assessment(true);
            List<string> lookupAssessmentDisplay = assessmentLookupTable.displayList;

            CourseAssessmentInput.Items.Add("Add New Assessment");
            CourseAssessmentInput.SelectedItem = "Add New Assessment";

            foreach (string assessmentInfo in lookupAssessmentDisplay) 
            { CourseAssessmentInput.Items.Add(assessmentInfo); }
        }

        // Populate Term List
        private void BuildTermList()
        {
            Controllers.TableHelper termLookupTable = new Controllers.TableHelper();
            List<Models.Term> termList = termLookupTable.GetTableData_term(true);
            List<string> lookupTermDisplay = termLookupTable.displayList;

            CourseTermInput.Items.Add("0. None");
            CourseTermInput.SelectedItem = "0. None";

            foreach (string termInfo in lookupTermDisplay)
            { CourseTermInput.Items.Add(termInfo); }
        }

        // Populate Course Fields if Editing
        private void BuildWorkingCourse(Models.Course buildCourse)
        {
            CourseTitleInput.Text = buildCourse.courseTitle;
            CourseStatusInput.SelectedItem = buildCourse.courseStatus;
            CreditStepper.Value = buildCourse.courseCredits;
            CourseNotesInput.Text = buildCourse.courseNotes;
            CourseStartDateInput.Date = buildCourse.courseStart;
            CourseEndDateInput.Date = buildCourse.courseEnd;
            NotificationsEnabledInput.IsChecked = buildCourse.courseNotifications;

            foreach (string term in CourseTermInput.Items) 
            {
                if (term.StartsWith($"{ buildCourse.termID_ref }."))
                { CourseTermInput.SelectedItem = term; }
            }

            foreach (string instructor in CourseInstructorInput.Items)
            {
                if (instructor.StartsWith($"{ buildCourse.instructorID_ref }."))
                { CourseInstructorInput.SelectedItem = instructor; }
            }
        }

        // Create Associated Assessment Table
        private void BuildAssessmentsTable()
        {
            assessmentListHelper.BuildAssessmentAssociations(workingID);
            AssociatedAssessmentsTblRoot.Add(assessmentListHelper.associatedSection);
        }

        /*
        Button Methods
        */

        // Share Via Email
        private void ShareBtn_clicked(object sender, EventArgs e)
        {
            Controllers.MessagingHelper testMessagingApp = new Controllers.MessagingHelper();
            Models.Student loggedStudent = new Models.Student();
            loggedStudent = loggedStudent.LookupStudent();

            string studentFullName = loggedStudent.studentFirstName + " " + loggedStudent.studentLastName;
            testMessagingApp.EmailMessenger(studentFullName, CourseNotesInput.Text);
        }

        // Add Instructor to List
        private void PlusInstructorBtn_clicked(object sender, EventArgs e)
        {
            if (CourseInstructorInput.SelectedItem.ToString() == "0. Add New Instructor")
            {
                Console.WriteLine("Modal Found");
                InstructorDetails interModalInstructor = new InstructorDetails(0);
                Navigation.PushModalAsync(interModalInstructor);
                interModalInstructor.Disappearing += (sender2, e2) =>
                {
                    CourseInstructorInput.Items.Clear();
                    BuildInstructorList();
                };
            }
        }

        // Add Assessment to List
        private void PlusAssessmentBtn_clicked(object sender, EventArgs e)
        {
            try 
            {
                if (CourseAssessmentInput.SelectedItem.ToString() == "Add New Assessment")
                {
                    AssessmentDetails interModalAssessment = new AssessmentDetails(0);
                    Navigation.PushModalAsync(interModalAssessment);
                    interModalAssessment.Disappearing += (sender2, e2) =>
                    {
                        AssociatedAssessmentsTblRoot.Clear();
                        CourseAssessmentInput.Items.Clear();
                        BuildAssessmentList();
                        BuildAssessmentsTable();
                    };
                }
                else
                {
                    assessmentListHelper.BuildPending(CourseAssessmentInput.SelectedItem.ToString());
                    Models.Assessment plusAssessment = new Models.Assessment();
                    plusAssessment = plusAssessment.LookupAssessment(Int32.Parse(CourseAssessmentInput.SelectedItem.ToString().Split('.')[0]));
                    assessmentListHelper.workingCourse.assessmentsList.Add(plusAssessment);
                }
            }
            catch 
            { DisplayAlert("Error", "Please Select An Assessment", "Ok"); }
        }

        // Save Course Changes to DB
        private async void SaveCourseBtn_clicked(object sender, EventArgs e)
        {
            string[] courseFields =
            {
                CourseTitleInput.Text,
                CourseStatusInput.SelectedItem.ToString(),
                CourseInstructorInput.SelectedItem.ToString(),
                CourseCreditInput.Text,
                CourseNotesInput.Text,
                CourseStartDateInput.Date.ToString(),
                CourseEndDateInput.Date.ToString()
            };

            Controllers.Validator courseValidator = new Controllers.Validator();

            if
            (
                courseValidator.CheckForNulls(courseFields) == true &&
                courseValidator.CheckKeyQty(assessmentListHelper.workingCourse.assessmentsList.Count, false) == true &&
                courseValidator.CheckAssessmentTypes(assessmentListHelper.workingCourse.assessmentsList) == true &&
                courseValidator.CheckDateValididty(CourseStartDateInput.Date, CourseEndDateInput.Date, workingID, false) == true                
            )
            {
                Models.Course courseCandidate = new Models.Course();
                courseCandidate.courseNotifications = NotificationsEnabledInput.IsChecked;
                courseCandidate.BuildCourse(courseFields);

                if (CourseTermInput.SelectedItem != null)
                { courseCandidate.termID_ref = Int32.Parse(CourseTermInput.SelectedItem.ToString().Split('.')[0]); }

                bool confirmSave = await DisplayAlert("Confirm", "Save Course Changes?", "Yes", "No");

                if (workingID == -1 && confirmSave == true)
                {
                    courseCandidate.assessmentsList = assessmentListHelper.workingCourse.assessmentsList;
                    courseCandidate.AddCourse();
                    courseCandidate.BuildAssociatedAssessments();
                    await Navigation.PopModalAsync();
                }
                else if (workingID != -1 && confirmSave == true)
                {
                    courseCandidate.courseID = workingID;
                    courseCandidate.assessmentsList = assessmentListHelper.workingCourse.assessmentsList;
                    courseCandidate.BuildAssociatedAssessments();
                    courseCandidate.EditCourse();
                    await Navigation.PopModalAsync();
                }
            }
            else 
            { await DisplayAlert("Error", courseValidator.errorMessage, "Ok"); }
        }

        // Cancel And Return To Dashboard
        private async void CancelCourseBtn_clicked(object sender, EventArgs e)
        {
            bool confirmCancel = await DisplayAlert("Confirm", "Cancel Course Changes?", "Yes", "No");
            
            if (confirmCancel == true)
            { await Navigation.PopModalAsync(); }
        }
    }
}