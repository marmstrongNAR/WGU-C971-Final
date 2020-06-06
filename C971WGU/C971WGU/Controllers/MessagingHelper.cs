using System;
using System.Collections.Generic;
using System.Text;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.LocalNotifications;


namespace C971WGU.Controllers
{
    public class MessagingHelper
    {
        private List<Models.Assessment> notificationsList_assessment = new List<Models.Assessment>();
        private List<Models.Course> notificationsList_course = new List<Models.Course>();
        Controllers.Validator notificationValidator = new Controllers.Validator();

        // Set Notifications For Assessment Due Dates - 15 Minutes Prior
        public void DueDateNotify_assessment()
        {
            Controllers.TableHelper notificationHelper_assessment = new Controllers.TableHelper();
            notificationsList_assessment = notificationHelper_assessment.GetTableData_assessment(false);

            for (int i = 0; i < notificationsList_assessment.Count; i++) 
            {
                DateTime startDate_assessment = notificationsList_assessment[i].assessmentStart;
                DateTime dueDate_assessment = notificationsList_assessment[i].assessmentEnd;

                Console.WriteLine($"{ notificationsList_assessment[i].assessmentTitle } \nStarting At: { startDate_assessment.AddMinutes(-15) } \nEnding At: { dueDate_assessment.AddMinutes(-15) }");

                if (notificationValidator.CheckNotifications(startDate_assessment, notificationsList_assessment[i].assessmentNotifications) == true) 
                { CrossLocalNotifications.Current.Show("Assessment Start Reminder", $"Assessment { notificationsList_assessment[i].assessmentTitle } \nStarting At: { startDate_assessment }", 101, startDate_assessment.AddMinutes(-15)); }

                if (notificationValidator.CheckNotifications(dueDate_assessment, notificationsList_assessment[i].assessmentNotifications) == true)
                { CrossLocalNotifications.Current.Show("Assessment Due Date Reminder", $"Assessment { notificationsList_assessment[i].assessmentTitle } \nDue By: { dueDate_assessment }", 102, dueDate_assessment.AddMinutes(-15)); }
            }
        }

        // Set Notifications For Course Due Dates - 1 Day Prior
        public void DueDateNotify_course()
        {
            Controllers.TableHelper notificationHelper_course = new Controllers.TableHelper();
            notificationsList_course = notificationHelper_course.GetTableData_course(false);

            for (int i = 0; i < notificationsList_course.Count; i++)
            {
                DateTime startDate = notificationsList_course[i].courseStart;
                DateTime dueDate = notificationsList_course[i].courseEnd;

                if (notificationValidator.CheckNotifications(startDate, notificationsList_course[i].courseNotifications) == true)
                { CrossLocalNotifications.Current.Show("Course Start Reminder", $"Course: { notificationsList_course[i].courseTitle } \nStarting On: { startDate.ToShortDateString() }", 103, startDate.AddDays(-1)); }

                if (notificationValidator.CheckNotifications(dueDate, notificationsList_course[i].courseNotifications) == true)
                { CrossLocalNotifications.Current.Show("Course Due Date Reminder", $"Course { notificationsList_course[i].courseTitle } \nDue By: { dueDate.ToShortDateString() }", 104, dueDate.AddDays(-1)); }
            }
        }

        // Share Course Notes Via Email
        public async void EmailMessenger(string studentName, string emailContext)
        {
            try
            {
                EmailMessage testMessage = new EmailMessage
                {
                    Subject = $"Message From { studentName }",
                    Body = emailContext
                };

                await Email.ComposeAsync(testMessage);
            }
            catch 
            { Console.WriteLine("Failed Send"); }
        }
    }
}
