using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace C971WGU
{
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {
        // Constructor
        public MainPage()
        {
            Controllers.InitTestModels initModelsCreator = new Controllers.InitTestModels();
            InitializeComponent();
        }
    }
}
