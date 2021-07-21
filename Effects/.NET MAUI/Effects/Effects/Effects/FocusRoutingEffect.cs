using Microsoft.Maui.Controls;
using System;
using System.ComponentModel;

namespace Effects.Effects
{
    public class FocusRoutingEffect : RoutingEffect
    {

    }

#if WINDOWS
	public class FocusPlatformEffect : Microsoft.Maui.Controls.Compatibility.Platform.UWP.PlatformEffect
    {
		public FocusPlatformEffect() : base()
		{
		}

		protected override void OnAttached()
		{
			try
			{
				(Control as Microsoft.UI.Xaml.Controls.Control).Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Cyan);
				(Control as Microsoft.Maui.MauiTextBox).BackgroundFocusBrush = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
			}
			catch (Exception ex)
			{
                System.Diagnostics.Debug.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
			}

		}

		protected override void OnDetached()
		{
		}
	}
#elif __ANDROID__
    public class FocusPlatformEffect : Microsoft.Maui.Controls.Compatibility.Platform.Android.PlatformEffect
    {
        Android.Graphics.Color originalBackgroundColor = new Android.Graphics.Color(0, 0, 0, 0);
        Android.Graphics.Color backgroundColor;

        protected override void OnAttached()
        {
            try
            {
                backgroundColor = Android.Graphics.Color.LightGreen;
                Control.SetBackgroundColor(backgroundColor);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            try
            {
                if (args.PropertyName == "IsFocused")
                {
                    if (((Android.Graphics.Drawables.ColorDrawable)Control.Background).Color == backgroundColor)
                    {
                        Control.SetBackgroundColor(originalBackgroundColor);
                    }
                    else
                    {
                        Control.SetBackgroundColor(backgroundColor);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }
    }
#elif __IOS__
	public class FocusPlatformEffect : Microsoft.Maui.Controls.Compatibility.Platform.iOS.PlatformEffect
    {
		UIKit.UIColor backgroundColor;

		protected override void OnAttached()
		{
			try
			{
				Control.BackgroundColor = backgroundColor = UIKit.UIColor.FromRGB(204, 153, 255);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
			}
		}

		protected override void OnDetached()
		{
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);

			try
			{
				if (args.PropertyName == "IsFocused")
				{
					if (Control.BackgroundColor == backgroundColor)
					{
						Control.BackgroundColor = UIKit.UIColor.White;
					}
					else
					{
						Control.BackgroundColor = backgroundColor;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
			}
		}
	}
#endif
}