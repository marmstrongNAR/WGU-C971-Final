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
    public partial class InstructorDetails : ContentPage
    {
        private int workingID;

        // Constructor
        public InstructorDetails(int idNum)
        {
            InitializeComponent();
            workingID = idNum;

            if (workingID != 0) 
            {
                Models.Instructor getInstructorInfo = new Models.Instructor();
                InstructorBanner.Text = $"Edit Instructor ID: { workingID }";
                getInstructorInfo = getInstructorInfo.LookupInstructor(workingID);
                BuildWorkingInstructor(getInstructorInfo);
            }
        }

        // Populate Instructor Fields if Editing
        public void BuildWorkingInstructor(Models.Instructor workingInstructor)
        {
            InstructorFirstInput.Text = workingInstructor.instructorFirstName;
            InstructorLastInput.Text = workingInstructor.instructorLastName;
            InstructorEmailInput.Text = workingInstructor.instructorEmail;
            InstructorPhoneInput.Text = workingInstructor.instructorPhone;
        }

        // Save Instructor Changes to DB
        private async void SaveInstructorBtn_clicked(object sender, EventArgs e)
        {
            string[] instructorFields =
            {
                InstructorFirstInput.Text,
                InstructorLastInput.Text,
                InstructorPhoneInput.Text,
                InstructorEmailInput.Text
            };

            Controllers.Validator instructorValidator = new Controllers.Validator();

            if
            (
                instructorValidator.CheckForNulls(instructorFields) == true &&
                instructorValidator.CheckContactValidity(instructorFields[3], instructorFields[2]) == true
            )
            {
                Models.Instructor instructorCandidate = new Models.Instructor();
                instructorCandidate.BuildInstructor(instructorFields);

                bool confirmSave = await DisplayAlert("Confirm", "Save Instuctor Changes?", "Yes", "No");

                if (workingID == 0 && confirmSave == true) 
                { 
                    instructorCandidate.AddInstructor();
                    await Navigation.PopModalAsync();
                }
                else if (workingID != 0 && confirmSave == true)
                {
                    instructorCandidate.instructorID = workingID;
                    instructorCandidate.EditInstructor();
                    await Navigation.PopModalAsync();
                }
            }
            else
            { await DisplayAlert("Error", instructorValidator.errorMessage, "Ok"); }
        }

        // Cancel and Return to Dashboard
        private async void CancelInstructorBtn_clicked(object sender, EventArgs e)
        {
            bool confirmCancel = await DisplayAlert("Confirm", "Cancel Instuctor Changes?", "Yes", "No");

            if (confirmCancel == true)
            { await Navigation.PopModalAsync(); }
        }
    }
}