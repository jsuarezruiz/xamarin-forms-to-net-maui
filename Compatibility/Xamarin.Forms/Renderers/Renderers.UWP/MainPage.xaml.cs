using XamarinApp = Renderers.App;

namespace Renderers.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new XamarinApp());
        }
    }
}
