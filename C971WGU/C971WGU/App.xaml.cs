using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971WGU
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            Controllers.MessagingHelper initNotify = new Controllers.MessagingHelper();
            initNotify.DueDateNotify_assessment();
            initNotify.DueDateNotify_course();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
