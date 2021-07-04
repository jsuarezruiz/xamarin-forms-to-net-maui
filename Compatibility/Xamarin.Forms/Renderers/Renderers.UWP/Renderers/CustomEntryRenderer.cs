using Renderers;
using Renderers.UWP.Renderers;
using Windows.UI;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace Renderers.UWP.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Red);
            }
        }
    }
}