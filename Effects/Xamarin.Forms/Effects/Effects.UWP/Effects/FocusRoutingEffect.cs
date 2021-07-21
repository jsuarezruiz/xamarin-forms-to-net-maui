using Effects.UWP.Effects;
using System;
using System.Diagnostics;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ResolutionGroupName("Effects")]
[assembly: ExportEffect(typeof(FocusPlatformEffect), "FocusEffect")]
namespace Effects.UWP.Effects
{
    public class FocusPlatformEffect : PlatformEffect
    {
        public FocusPlatformEffect() : base()
        {
        }

        protected override void OnAttached()
        {
            try
            {
                (Control as Control).Background = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Cyan);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
        }
    }
}
