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
    public partial class StudentDetails : ContentPage
    {
        Models.Student workingStudent_student;

        public StudentDetails(Models.Student loggedStudent)
        {
            InitializeComponent();
            workingStudent_student = loggedStudent;
            PopulateProfile();
        }

        // Fill Fields With Profile Info
        private void PopulateProfile()
        {
            StudentFirstInput.Text = workingStudent_student.studentFirstName;
            StudentLastInput.Text = workingStudent_student.studentLastName;
            StudentPhoneInput.Text = workingStudent_student.studentPhone;
            StudentEmailInput.Text = workingStudent_student.studentEmail;
        }

        // Save Changes To Profile
        private async void SaveRegistrationBtn_clicked(object sender, EventArgs e)
        {
            string[] studentFields =
            {
                StudentFirstInput.Text,
                StudentLastInput.Text,
                StudentPhoneInput.Text,
                StudentEmailInput.Text
            };

            Controllers.Validator studentValidator = new Controllers.Validator();

            if
            (
                studentValidator.CheckForNulls(studentFields) == false ||
                studentValidator.CheckContactValidity(studentFields[3], studentFields[2]) == true
            )
            {
                Models.Student studentCandidate = new Models.Student();
                workingStudent_student.BuildStudent(studentFields);

                bool confirmCancel = await DisplayAlert("Confirm", "Save Profile Changes?", "Yes", "No");

                if (confirmCancel == true)
                { 
                    workingStudent_student.EditStudent();
                    await Navigation.PopModalAsync();
                }
            }
        }

        // Cancel And Return to Dashboard
        private async void CancelRegistrationBtn_clicked(object sender, EventArgs e)
        {
            bool confirmCancel = await DisplayAlert("Confirm", "Cancel Profile Edit?", "Yes", "No");

            if (confirmCancel == true)
            { await Navigation.PopModalAsync(); }
        }
    }
}