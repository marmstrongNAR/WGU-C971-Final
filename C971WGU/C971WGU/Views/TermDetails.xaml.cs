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
    public partial class TermDetails : ContentPage
    {
        private Controllers.ListHelper courseListHelper = new Controllers.ListHelper();
        private int workingID;

        // Constructor
        public TermDetails(int idNum)
        {
            InitializeComponent();  

            workingID = idNum;
            BuildCourseList();
            BuildCourseTable();
            
            if (workingID != -1)
            {
                Models.Term templateTerm = new Models.Term();
                TermBanner.Text = $"Edit Term ID: { workingID }";
                templateTerm = templateTerm.LookupTerm(workingID);
                BuildWorkingTerm(templateTerm);
            }
        }

        // Populate Term Fields if Editing
        public void BuildWorkingTerm(Models.Term workingTerm)
        {
            TermTitleInput.Text = workingTerm.termTitle;
            TermStartDateInput.Date = workingTerm.termStart;
            TermEndDateInput.Date = workingTerm.termEnd;
        }

        // Populate Course List
        private void BuildCourseList()
        {
            Controllers.TableHelper courseLookupTable = new Controllers.TableHelper();
            List<Models.Course> courseList = courseLookupTable.GetTableData_course(true);
            List<string> lookupCourseDisplay = courseLookupTable.displayList;

            TermCoursesInput.Items.Add("Add New Course");
            TermCoursesInput.SelectedItem = "Add New Course";

            foreach (string courseInfo in lookupCourseDisplay)
            { TermCoursesInput.Items.Add(courseInfo); }
        }

        // Create Associated Course Table
        private void BuildCourseTable()
        {
            courseListHelper.BuildCourseAssociations(workingID);
            AssociatedCoursesTblRoot.Add(courseListHelper.associatedSection);
        }

        /*
        Button Methods
        */

        // Add Course to List
        private void PlusCourseBtn_clicked(object sender, EventArgs e)
        {
            try
            {
                if (TermCoursesInput.SelectedItem.ToString() == "Add New Course")
                {
                    CourseDetails interModalCourse = new CourseDetails(-1);
                    Navigation.PushModalAsync(interModalCourse);
                    interModalCourse.Disappearing += (sender2, e2) => 
                    {
                        AssociatedCoursesTblRoot.Clear();
                        TermCoursesInput.Items.Clear();
                        BuildCourseList();
                        BuildCourseTable();
                    };
                }
                else
                {
                    courseListHelper.BuildPending(TermCoursesInput.SelectedItem.ToString());
                    Models.Course plusCourse = new Models.Course();
                    plusCourse = plusCourse.LookupCourse(Int32.Parse(TermCoursesInput.SelectedItem.ToString().Split('.')[0]));
                    courseListHelper.workingTerm.coursesList.Add(plusCourse);
                }
            }
            catch
            { DisplayAlert("Error", "Please Select A Course", "Ok"); }
        }

        // Save Term Changes to DB
        private async void SaveTermBtn_clicked(object sender, EventArgs e)
        {
            string[] termFields =
            {
                TermTitleInput.Text,
                TermStartDateInput.Date.ToShortDateString(),
                TermEndDateInput.Date.ToShortDateString()
            };
            
            Controllers.Validator termValidator = new Controllers.Validator();

            foreach (Models.Course idVal in courseListHelper.workingTerm.coursesList) 
            { Console.WriteLine("ID Added: " + idVal.courseID); }

            if
            (
                termValidator.CheckForNulls(termFields) == true &&
                termValidator.CheckDateValididty(TermStartDateInput.Date, TermEndDateInput.Date, workingID, true) == true
            )
            {
                Models.Term termCandidate = new Models.Term();
                termCandidate.BuildTerm(termFields);

                bool confirmSave = await DisplayAlert("Confirm", "Save Term Changes?", "Yes", "No");

                if (workingID == -1 && confirmSave == true)
                {
                    termCandidate.coursesList = courseListHelper.workingTerm.coursesList;
                    termCandidate.AddTerm();
                    termCandidate.BuildAssociatedCourses();
                    await Navigation.PopModalAsync();
                }
                else if (workingID != -1 && confirmSave == true)
                {
                    termCandidate.termID = workingID;
                    termCandidate.coursesList = courseListHelper.workingTerm.coursesList;
                    termCandidate.EditTerm();
                    termCandidate.BuildAssociatedCourses();
                    await Navigation.PopModalAsync();
                }
            }
            else
            { await DisplayAlert("Error", termValidator.errorMessage, "Ok"); }
        }

        // Cancel And Return To Dashboard
        private async void CancelTermBtn_clicked(object sender, EventArgs e)
        {
            bool confirmCancel = await DisplayAlert("Confirm", "Cancel Term Changes?", "Yes", "No");

            if (confirmCancel == true)
            { await Navigation.PopModalAsync(); }
        }
    }
}