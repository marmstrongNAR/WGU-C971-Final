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
    public partial class StudentDashboard : ContentPage
    {
        public Models.Student loggedStudent_students = new Models.Student();

        public StudentDashboard()
        {
            InitializeComponent();
            BuildStudentDetails();
        }

        public void BuildStudentDetails() // Fix Data Pull / Need To Run After Modal Dismiss  
        {
            loggedStudent_students = loggedStudent_students.LookupStudent();

            NameLabel.Text = $"Welcome:\n { loggedStudent_students.studentFirstName } { loggedStudent_students.studentLastName }\n";
            EmailLabel.Text = $"Email Address:\n { loggedStudent_students.studentEmail }\n";
            PhoneLabel.Text = $"Phone Number:\n { loggedStudent_students.studentPhone }\n";          
        }

        private async void EditStudentBtn_clicked(object sender, EventArgs e)
        {
            StudentDetails editStudentProfile = new StudentDetails(loggedStudent_students);
            await Navigation.PushModalAsync(editStudentProfile);
            editStudentProfile.Disappearing += (sender2, e2) => { BuildStudentDetails(); };
        }

        /*protected override void OnAppearing()
        {
            base.OnAppearing();
            BuildStudentDetails();
        }*/
    }
}