//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("C971WGU.MainPage.xaml", "MainPage.xaml", typeof(global::C971WGU.MainPage))]

namespace C971WGU {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("MainPage.xaml")]
    public partial class MainPage : global::Xamarin.Forms.TabbedPage {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::C971WGU.Views.StudentDashboard studentDasboard;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::C971WGU.Views.CourseDashboard courseDashboard;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::C971WGU.Views.AssessmentDashboard assessmentDashboard;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::C971WGU.Views.InstructorDashboard instructorDashboard;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(MainPage));
            studentDasboard = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::C971WGU.Views.StudentDashboard>(this, "studentDasboard");
            courseDashboard = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::C971WGU.Views.CourseDashboard>(this, "courseDashboard");
            assessmentDashboard = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::C971WGU.Views.AssessmentDashboard>(this, "assessmentDashboard");
            instructorDashboard = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::C971WGU.Views.InstructorDashboard>(this, "instructorDashboard");
        }
    }
}
