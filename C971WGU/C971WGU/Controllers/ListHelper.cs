using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971WGU.Controllers
{
    public class ListHelper
    {
        Controllers.TableHelper associationTableHelper = new Controllers.TableHelper();
        public TableSection associatedSection = new TableSection();
        public List<ViewCell> associatedData = new List<ViewCell>();
        public Models.Course workingCourse = new Models.Course();
        public Models.Term workingTerm = new Models.Term();

        // Build Associations List For Courses Per Term
        public void BuildCourseAssociations(int lookupTermID)
        {
            List<Models.Course> coursePickList = associationTableHelper.GetTableData_course(true);
            int indexCounter_course = 0;

            if (lookupTermID != -1) 
            { workingTerm = workingTerm.LookupTerm(lookupTermID); }

            for (int i = 0; i < coursePickList.Count; i++)
            {                
                if (lookupTermID == coursePickList[i].termID_ref) 
                {
                    Grid courseTemplate = new Grid();

                    courseTemplate.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
                    courseTemplate.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
                    courseTemplate.Children.Add(CreateLabel(associationTableHelper.displayList[i]), 0, 0);
                    courseTemplate.Children.Add(CreateDeleteBtn(indexCounter_course), 1, 0);
                    indexCounter_course++;

                    workingTerm.coursesList.Add(coursePickList[i]);
                    associatedData.Add(new ViewCell { View = courseTemplate });
                    associatedSection.Add(new ViewCell { View = courseTemplate });
                }
            }
        }

        // Build Associations List For Assessments Per Course
        public void BuildAssessmentAssociations(int lookupCourseID)
        {
            List<Models.Assessment> assessmentPickList = associationTableHelper.GetTableData_assessment(true);
            int indexCounter_assessment = 0;

            if (lookupCourseID != -1)
            { workingCourse = workingCourse.LookupCourse(lookupCourseID); }

            for (int i = 0; i < assessmentPickList.Count; i++)
            {
                if (lookupCourseID == assessmentPickList[i].courseID_ref)
                {
                    Grid assessmentTemplate = new Grid();

                    assessmentTemplate.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
                    assessmentTemplate.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
                    assessmentTemplate.Children.Add(CreateLabel(associationTableHelper.displayList[i]), 0, 0);
                    assessmentTemplate.Children.Add(CreateDeleteBtn(indexCounter_assessment), 1, 0);
                    indexCounter_assessment++;

                    workingCourse.assessmentsList.Add(assessmentPickList[i]);
                    associatedData.Add(new ViewCell { View = assessmentTemplate });
                    associatedSection.Add(new ViewCell { View = assessmentTemplate });                 
                }
            }
        }

        // Add Pending Association
        public void BuildPending(string pendingContext)
        {
            Grid pendingTemplate = new Grid();

            pendingTemplate.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
            pendingTemplate.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
            pendingTemplate.Children.Add(CreateLabel(pendingContext), 0, 0);
            pendingTemplate.Children.Add(CreateDeleteBtn(associatedData.Count), 1, 0);

            associatedData.Add(new ViewCell { View = pendingTemplate });
            associatedSection.Add(new ViewCell { View = pendingTemplate });
        }

        // Create Text Label For Associated Tables
        private Label CreateLabel(string labelContext)
        {
            Label associationTitle = new Label();

            associationTitle.FontSize = 14;
            associationTitle.HorizontalOptions = LayoutOptions.StartAndExpand;
            associationTitle.VerticalOptions = LayoutOptions.CenterAndExpand;
            associationTitle.Text = labelContext;

            return associationTitle;
        }

        // Create Delete Button For Associated Tables
        private Button CreateDeleteBtn(int delRef)
        {
            Button delBtn = new Button();

            delBtn.BackgroundColor = Color.FromHex("#9537B1");
            delBtn.ImageSource = "Del_icon.png";
            delBtn.HorizontalOptions = LayoutOptions.End;
            delBtn.VerticalOptions = LayoutOptions.Center;
            delBtn.WidthRequest = 40;
            delBtn.CornerRadius = 10;
            delBtn.Margin = 6 ;
            delBtn.Clicked += new EventHandler ((object sender, System.EventArgs e) => 
            {  
                try
                {
                    if (workingCourse.assessmentsList.Count != 0)
                    { workingCourse.assessmentsList.RemoveAt(delRef); }
                    
                    if (workingTerm.coursesList.Count != 0) 
                    { workingTerm.coursesList.RemoveAt(delRef); }

                    associatedData.RemoveAt(delRef);
                    associatedSection.Clear();
                    
                    foreach (ViewCell view in associatedData) 
                    { associatedSection.Add(view); }
                }
                catch 
                { Console.WriteLine("No Items"); }
                
            });
            
            return delBtn;
        }
    }
}
