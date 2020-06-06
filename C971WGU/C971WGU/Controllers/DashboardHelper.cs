using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971WGU.Controllers
{
    public class DashboardHelper
    {
        private Controllers.TableHelper dashboardTableHelper = new Controllers.TableHelper();
        public List<ViewCell> dashboardData = new List<ViewCell>();
        public TableSection dashboardSection = new TableSection();

        // Build Assessments Table List For Dashboard
        public void BuildAssessmentTable_dashboard()
        {
            List<Models.Assessment> assessmentDashboardList = dashboardTableHelper.GetTableData_assessment(true);

            for (int i = 0; i < assessmentDashboardList.Count; i++)
            {
                if (assessmentDashboardList[i].assessmentEnd > DateTime.Now)
                {
                    Grid assessmentTemplate_dashboard = new Grid();

                    assessmentTemplate_dashboard.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
                    assessmentTemplate_dashboard.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
                    assessmentTemplate_dashboard.Children.Add(CreateLabel(assessmentDashboardList[i].assessmentTitle), 0, 0);
                    assessmentTemplate_dashboard.Children.Add(CreateLabel(assessmentDashboardList[i].assessmentEnd.ToString()), 1, 0);

                    dashboardData.Add(new ViewCell { View = assessmentTemplate_dashboard });
                    dashboardSection.Add(new ViewCell { View = assessmentTemplate_dashboard });
                }
            }
        }

        // Build Courses Table List For Dashboard
        public void BuildCourseTable_dashboard(string selectedTerm)
        {
            List<Models.Course> courseDashboardList = dashboardTableHelper.GetTableData_course(true);
            Models.Term refTerm = new Models.Term();
            refTerm = refTerm.LookupTerm(Int32.Parse(selectedTerm.Split('.')[0]));

            for (int i = 0; i < courseDashboardList.Count; i++)
            {
                if (courseDashboardList[i].termID_ref == refTerm.termID)
                {
                    Grid courseTemplate_dashboard = new Grid();

                    courseTemplate_dashboard.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
                    courseTemplate_dashboard.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
                    courseTemplate_dashboard.Children.Add(CreateLabel(courseDashboardList[i].courseTitle), 0, 0);
                    courseTemplate_dashboard.Children.Add(CreateLabel($"{courseDashboardList[i].courseStart.ToShortDateString()}\n{courseDashboardList[i].courseEnd.ToShortDateString()}"), 1, 0);

                    dashboardData.Add(new ViewCell { View = courseTemplate_dashboard });
                    dashboardSection.Add(new ViewCell { View = courseTemplate_dashboard });
                }
            }
        }

        public void BuildInstructorTable_dashboard()
        {
            List<Models.Instructor> instructorDashboardList = dashboardTableHelper.GetTableData_instructor(true);

            for (int i = 0; i < instructorDashboardList.Count; i++)
            {
                Grid instructorTemplate_dashboard = new Grid();

                instructorTemplate_dashboard.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) });
                instructorTemplate_dashboard.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
                instructorTemplate_dashboard.Children.Add(CreateLabel($"{ instructorDashboardList[i].instructorFirstName }\n{instructorDashboardList[i].instructorLastName}"), 0, 0);
                instructorTemplate_dashboard.Children.Add(CreateLabel($"{ instructorDashboardList[i].instructorEmail }\n{ instructorDashboardList[i].instructorPhone }"), 1, 0);
                
                dashboardData.Add(new ViewCell { View = instructorTemplate_dashboard });
                dashboardSection.Add(new ViewCell { View = instructorTemplate_dashboard });
            }
        }

        // Create Text Label For Associated Tables
        private Label CreateLabel(string labelContext)
        {
            Label dashboardLabel = new Label();

            dashboardLabel.FontSize = 14;
            dashboardLabel.VerticalOptions = LayoutOptions.CenterAndExpand;
            dashboardLabel.Text = labelContext;

            return dashboardLabel;
        }
    }
}
