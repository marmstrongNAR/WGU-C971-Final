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
    public partial class InstructorDashboard : ContentPage
    {
        // Constructor
        public InstructorDashboard()
        { 
            InitializeComponent();
            InstructorTableBuilder();
        }

        // Build Instructor Table
        private void InstructorTableBuilder()
        {
            InstructorDataTblRoot.Clear();
            Controllers.DashboardHelper instructorsTableBuilder = new Controllers.DashboardHelper();
            instructorsTableBuilder.BuildInstructorTable_dashboard();
            InstructorDataTblRoot.Add(instructorsTableBuilder.dashboardSection);
        }

        /*
        Button Methods
        */

        // Add New Instructor
        private void AddInstructorBtn_clicked(object sender, EventArgs e)
        {
            InstructorDetails addInstructorForm = new InstructorDetails(0);
            Navigation.PushModalAsync(addInstructorForm);
            addInstructorForm.Disappearing += (sender2, e2) => { InstructorTableBuilder(); };
        }

        // Edit Existing Instructor
        private async void EditInstructorBtn_clicked(object sender, EventArgs e)
        {
            Controllers.TableHelper editInstructorTable = new Controllers.TableHelper();
            List<Models.Instructor> instructorEditCandidates = editInstructorTable.GetTableData_instructor(true);
            List<string> instructorDisplay = editInstructorTable.displayList;

            var instructorSheet_edit = await DisplayActionSheet("Instructors", "Cancel", "Ok", instructorDisplay.ToArray());
            if (instructorSheet_edit != "Cancel") 
            {
                try
                {
                    InstructorDetails editInstructorForm = new InstructorDetails(Int32.Parse(instructorSheet_edit.Split('.')[0]));
                    await Navigation.PushModalAsync(editInstructorForm);
                    editInstructorForm.Disappearing += (sender2, e2) => { InstructorTableBuilder(); };
                }
                catch 
                { Console.WriteLine("Modal Dissmissed"); }
            }
        }

        // Delete Existing Instructor
        private async void DeleteInstructorBtn_clicked(object sender, EventArgs e)
        {
            Controllers.TableHelper deleteInstructorTable = new Controllers.TableHelper();
            List<Models.Instructor> instructorDeleteCandidates = deleteInstructorTable.GetTableData_instructor(true);
            List<string> instructorDisplay = deleteInstructorTable.displayList;

            var instructorSheet_delete = await DisplayActionSheet("Instructors", "Cancel", "Ok", instructorDisplay.ToArray());
            if (instructorSheet_delete != "Cancel")
            {
                try
                {
                    foreach (Models.Instructor insCandidate in instructorDeleteCandidates)
                    {
                        if (Int32.Parse(instructorSheet_delete.Split('.')[0]) == insCandidate.instructorID)
                        {
                            bool confirmDelete = await DisplayAlert("Confirm", "Delete Instructor?", "Yes", "No");

                            if (confirmDelete == true)
                            {
                                insCandidate.DeleteInstructor();
                                InstructorTableBuilder();
                            }
                        }
                    }
                }
                catch 
                { Console.WriteLine("Modal Dissmissed"); }
            }
        }
    }
}