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
    public partial class AssessmentDashboard : ContentPage
    {
        public AssessmentDashboard()
        {
            InitializeComponent();
            AssessmentTableBuilder();
        }

        // Build Upcoming Assessments Table
        private void AssessmentTableBuilder()
        {
            AssessmentDataTblRoot.Clear();
            Controllers.DashboardHelper assessmentsTableBuilder = new Controllers.DashboardHelper();
            assessmentsTableBuilder.BuildAssessmentTable_dashboard();
            AssessmentDataTblRoot.Add(assessmentsTableBuilder.dashboardSection);
        }

        /*
        Button Methods 
        */

        // Open Modal to Create New Assessment
        private async void AddAssessmentBtn_clicked(object sender, EventArgs e)
        {
            AssessmentDetails addAssessmentForm = new AssessmentDetails(0);
            await Navigation.PushModalAsync(addAssessmentForm);
            addAssessmentForm.Disappearing += (sender2, e2) => { AssessmentTableBuilder(); };
        }

        // Open Modal to Modify Existing Assessment
        private async void EditAssessmentBtn_clicked(object sender, EventArgs e)
        {
            Controllers.TableHelper editAssessmentTable = new Controllers.TableHelper();
            List<Models.Assessment> assessmentEditCandidates = editAssessmentTable.GetTableData_assessment(true);
            List<string> assessmentDisplay = editAssessmentTable.displayList;

            var assessmentSheet_edit = await DisplayActionSheet("Assessments", "Cancel", "Ok", assessmentDisplay.ToArray());
            if (assessmentSheet_edit != "Cancel")
            {
                try
                {
                    AssessmentDetails editAssessmentForm = new AssessmentDetails(Int32.Parse(assessmentSheet_edit.Split('.')[0]));
                    await Navigation.PushModalAsync(editAssessmentForm);
                    editAssessmentForm.Disappearing += (sender2, e2) => { AssessmentTableBuilder(); };
                }
                catch
                { Console.WriteLine("Modal Dissmissed"); }          
            }
        }

        // Open Modal to Delete Existing Assessment
        private async void DeleteAssessmentBtn_clicked(object sender, EventArgs e)
        {
            Controllers.TableHelper deleteAssessmentTable = new Controllers.TableHelper();
            List<Models.Assessment> assessmentDeleteCandidates = deleteAssessmentTable.GetTableData_assessment(true);
            List<string> assessmentDisplay = deleteAssessmentTable.displayList;

            var assessmentSheet_delete = await DisplayActionSheet("Assessments", "Cancel", "Ok", assessmentDisplay.ToArray());
            if (assessmentSheet_delete != "Cancel") 
            {
                try
                {
                    foreach (Models.Assessment insCandidate in assessmentDeleteCandidates)
                    {
                        if (Int32.Parse(assessmentSheet_delete.Split('.')[0]) == insCandidate.assessmentID)
                        {
                            bool confirmDelete = await DisplayAlert("Confirm", "Delete Assessment?", "Yes", "No");

                            if (confirmDelete == true)
                            {
                                insCandidate.DeleteAssessment();
                                AssessmentTableBuilder();
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