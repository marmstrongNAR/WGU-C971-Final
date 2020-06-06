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
    public partial class AssessmentDetails : ContentPage
    {
        private int workingID;

        // Constructor
        public AssessmentDetails(int idNum)
        {
            InitializeComponent();
            BuildCourseList();

            workingID = idNum;

            if (workingID != 0)
            {
                Models.Assessment getAssessmentInfo = new Models.Assessment();
                AssessmentBanner.Text = $"Edit Assessment ID: { workingID }";
                getAssessmentInfo = getAssessmentInfo.LookupAssessment(workingID);
                BuildWorkingAssessment(getAssessmentInfo);                
            }
        }

        // Toggles Switches Based on Input
        private void OAInput_toggled(object sender, ToggledEventArgs e) 
        { 
            if (OAInput.IsToggled == true && PAInput.IsToggled == true)
            { PAInput.IsToggled = false; }
        }

        private void PAInput_toggled(object sender, ToggledEventArgs e)
        {
            if (PAInput.IsToggled == true && OAInput.IsToggled == true)
            { OAInput.IsToggled = false; }
        }

        // Populate Course List
        private void BuildCourseList()
        {
            Controllers.TableHelper courseLookupTable = new Controllers.TableHelper();
            List<Models.Course> courseList = courseLookupTable.GetTableData_course(true);
            List<string> lookupCourseDisplay = courseLookupTable.displayList;

            AssessmentCourseInput.Items.Add("0. None");
            AssessmentCourseInput.SelectedItem = "0. None";

            foreach (string courseInfo in lookupCourseDisplay)
            { AssessmentCourseInput.Items.Add(courseInfo); }
        }

        // Populate Assessment Fields if Editing
        private void BuildWorkingAssessment(Models.Assessment workingAssessment)
        {
            Models.Course associatedCourse = new Models.Course();
            associatedCourse = associatedCourse.LookupCourse(workingAssessment.courseID_ref);

            AssessmentTitleInput.Text = workingAssessment.assessmentTitle;
            AssessmentStartDateInput.Date = workingAssessment.assessmentStart;
            AssessmentStartTimeInput.Time = workingAssessment.assessmentStart.TimeOfDay;
            AssessmentEndDateInput.Date = workingAssessment.assessmentEnd;
            AssessmentEndTimeInput.Time = workingAssessment.assessmentEnd.TimeOfDay;
            NotificationsEnabledInput.IsChecked = workingAssessment.assessmentNotifications;

            if (workingAssessment.courseID_ref != 0) 
            { AssessmentCourseInput.SelectedItem = $"{ associatedCourse.courseID }. { associatedCourse.courseTitle }"; }

            if (workingAssessment.assessmentType == "Performance")
            { PAInput.IsToggled = true; }
            else if (workingAssessment.assessmentType == "Objective")
            { OAInput.IsToggled = true; }
        }

        // Save Assessment Changes To DB
        private async void SaveAssessmentBtn_clicked(object sender, EventArgs e)
        {
            string[] assessmentFields =
            {
                AssessmentTitleInput.Text,
                AssessmentStartDateInput.Date.ToString(),
                AssessmentStartTimeInput.Time.ToString(),
                AssessmentEndDateInput.Date.ToString(),
                AssessmentEndTimeInput.Time.ToString()
            };

            Controllers.Validator assessmentValidator = new Controllers.Validator();

            if (assessmentValidator.CheckForNulls(assessmentFields) == true)
            {
                Models.Assessment assessmentCandidate = new Models.Assessment();
                assessmentCandidate.BuildAssessment(assessmentFields, OAInput.IsToggled);

                bool confirmSave = await DisplayAlert("Confirm", "Save Assessment Changes?", "Yes", "No");

                if (workingID == 0 && confirmSave == true)
                {
                    if (AssessmentCourseInput.SelectedItem != null) 
                    { assessmentCandidate.courseID_ref = Int32.Parse(AssessmentCourseInput.SelectedItem.ToString().Split('.')[0]); }

                    assessmentCandidate.assessmentNotifications = NotificationsEnabledInput.IsChecked;
                    assessmentCandidate.AddAssessment();
                    await Navigation.PopModalAsync();
                }
                else 
                {
                    if (AssessmentCourseInput.SelectedItem != null)
                    { assessmentCandidate.courseID_ref = Int32.Parse(AssessmentCourseInput.SelectedItem.ToString().Split('.')[0]); }

                    assessmentCandidate.assessmentID = workingID;
                    assessmentCandidate.assessmentNotifications = NotificationsEnabledInput.IsChecked;
                    assessmentCandidate.EditAssessment();
                    await Navigation.PopModalAsync();
                }
            }
            else 
            { await DisplayAlert("Error", assessmentValidator.errorMessage, "Ok"); }
        }

        // Cancel And Return To Dashboard
        private async void CancelAssessmentBtn_clicked(object sender, EventArgs e)
        {
            bool confirmCancel = await DisplayAlert("Confirm", "Cancel Assessment Changes?", "Yes", "No");

            if (confirmCancel == true)
            { await Navigation.PopModalAsync(); }
        }
    }
}