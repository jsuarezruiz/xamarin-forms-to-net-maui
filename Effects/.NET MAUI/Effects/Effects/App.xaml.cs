using Application = Microsoft.Maui.Controls.Application;

namespace Effects
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }
    }
}
