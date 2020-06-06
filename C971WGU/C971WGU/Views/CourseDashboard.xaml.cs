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
    public partial class CourseDashboard : ContentPage
    {
        // Constructor
        public CourseDashboard()
        {
            InitializeComponent();
            TermListBuilder();
        }

        // Build Term Picklist
        private List<string> TermDisplay()
        {
            Controllers.TableHelper termDisplayTable = new Controllers.TableHelper();
            List<Models.Term> termCandidates = termDisplayTable.GetTableData_term(true);
            List<string> termDisplayList = termDisplayTable.displayList;

            return termDisplayList;
        }

        // Build Course Table For Dashboard
        private void TermListBuilder()
        {
            List<string> termBuilderList = TermDisplay();

            foreach (string termPick in termBuilderList) 
            { DashboardTermsInput.Items.Add(termPick); }

            DashboardTermsInput.SelectedItem = DashboardTermsInput.Title;
        }

        // Clears and Rebuilds Courses Table When Index Changes
        private void DashboardTermsInput_changed(object sender, EventArgs e) 
        {
            CourseDataTblRoot.Clear();
            Controllers.DashboardHelper courseTableHelper = new Controllers.DashboardHelper();
            try { courseTableHelper.BuildCourseTable_dashboard(DashboardTermsInput.SelectedItem.ToString()); }
            catch { Console.WriteLine("No Index Selected"); }
            CourseDataTblRoot.Add(courseTableHelper.dashboardSection);
        }

        /*
        Button Methods 
        */

        // Term Buttons
        private void AddTermBtn_clicked(object sender, EventArgs e)
        {
            TermDetails addTermForm = new TermDetails(-1);
            Navigation.PushModalAsync(addTermForm);
            DashboardTermsInput.Items.Clear();
            addTermForm.Disappearing += (sender2, e2) => { TermListBuilder(); };
        }

        private async void EditTermBtn_clicked(object sender, EventArgs e)
        {
            List<string> termDisplay_edit = TermDisplay();

            var termSheet_edit = await DisplayActionSheet("Terms", "Cancel", "Ok", termDisplay_edit.ToArray());
            if (termSheet_edit != "Cancel")
            {
                try
                {
                    TermDetails editTermForm = new TermDetails(Int32.Parse(termSheet_edit.Split('.')[0]));
                    await Navigation.PushModalAsync(editTermForm);
                    DashboardTermsInput.Items.Clear();
                    editTermForm.Disappearing += (sender2, e2) => { TermListBuilder(); };
                }
                catch 
                { Console.WriteLine("Modal Dissmissed"); }
            }
        }

        private async void DeleteTermBtn_clicked(object sender, EventArgs e)
        {
            Controllers.TableHelper deleteTermTable = new Controllers.TableHelper();
            List<Models.Term> termDeleteCandidates = deleteTermTable.GetTableData_term(true);
            List<string> termDisplay_delete = deleteTermTable.displayList;
            Controllers.Validator deleteTermValidator = new Controllers.Validator();

            var termSheet_delete = await DisplayActionSheet("Terms", "Cancel", "Ok", termDisplay_delete.ToArray());
            if (termSheet_delete != "Cancel")
            {
                try
                {
                    foreach (Models.Term termCandidate in termDeleteCandidates)
                    {
                        if (Int32.Parse(termSheet_delete.Split('.')[0]) == termCandidate.termID)
                        {
                            if (deleteTermValidator.CheckIfExists("Course", "termID_ref", $"WHERE termID_ref = { termCandidate.termID }") == false)
                            {
                                bool confirmDelete = await DisplayAlert("Confirm", "Delete Term?", "Yes", "No");

                                if (confirmDelete == true)
                                {
                                    termCandidate.DeleteTerm();
                                    DashboardTermsInput.Items.Clear();
                                    TermListBuilder();
                                }
                            }
                            else
                            { await DisplayAlert("Error", "Cannot Delete Term With Course Associations", "Ok"); }
                        }
                    }
                }
                catch 
                { Console.WriteLine("Modal Dissmissed"); }
            }
        }

        // Course Buttons
        private void AddCourseBtn_clicked(object sender, EventArgs e)
        {
            CourseDetails addCourseForm = new CourseDetails(-1);
            Navigation.PushModalAsync(addCourseForm);
            DashboardTermsInput.Items.Clear();
            addCourseForm.Disappearing += (sender2, e2) => { TermListBuilder(); };
        }

        private async void EditCourseBtn_clicked(object sender, EventArgs e)
        {
            Controllers.TableHelper editCourseTable = new Controllers.TableHelper();
            List<Models.Course> courseEditCandidates = editCourseTable.GetTableData_course(true);
            List<string> courseDisplay = editCourseTable.displayList;

            var courseSheet_edit = await DisplayActionSheet("Courses", "Cancel", "Ok", courseDisplay.ToArray());
            if (courseSheet_edit != "Cancel")
            {
                try
                {
                    CourseDetails editCourseForm = new CourseDetails(Int32.Parse(courseSheet_edit.Split('.')[0]));
                    await Navigation.PushModalAsync(editCourseForm);
                    DashboardTermsInput.Items.Clear();
                    editCourseForm.Disappearing += (sender2, e2) => { TermListBuilder(); };
                }
                catch 
                { Console.WriteLine("Modal Dissmissed"); }
            }
        }

        private async void DeleteCourseBtn_clicked(object sender, EventArgs e)
        {
            Controllers.TableHelper deleteCourseTable = new Controllers.TableHelper();
            List<Models.Course> courseDeleteCandidates = deleteCourseTable.GetTableData_course(true);
            List<string> courseDisplay = deleteCourseTable.displayList;
            Controllers.Validator deleteCourseValidator = new Controllers.Validator();

            var courseSheet_delete = await DisplayActionSheet("Courses", "Cancel", "Ok", courseDisplay.ToArray());
            if (courseSheet_delete != "Cancel")
            {
                try
                {
                    foreach (Models.Course courseCandidate in courseDeleteCandidates)
                    {
                        if (Int32.Parse(courseSheet_delete.Split('.')[0]) == courseCandidate.courseID)
                        {
                            if (deleteCourseValidator.CheckIfExists("Assessment", "courseID_ref", $"WHERE courseID_ref = { courseCandidate.courseID }") == false)
                            {
                                bool confirmDelete = await DisplayAlert("Confirm", "Delete Course?", "Yes", "No");

                                if (confirmDelete == true)
                                {
                                    courseCandidate.DeleteCourse();
                                    DashboardTermsInput.Items.Clear();
                                    TermListBuilder();
                                }
                            }
                            else
                            { await DisplayAlert("Error", "Cannot Delete Course With Assessment Associations", "Ok"); }
                        }
                    }
                }
                catch 
                { Console.WriteLine("Modal Dissmissed"); }
            }
        }
    }
}