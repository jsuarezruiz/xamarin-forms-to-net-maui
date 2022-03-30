using Microsoft.Maui.Controls;
using System;
using System.ComponentModel;

namespace Library.Effects
{
    public class FocusRoutingEffect : RoutingEffect
    {

    }

#if WINDOWS
	public class PlatformFocusPlatformEffect : Microsoft.Maui.Controls.Platform.PlatformEffect
    {
		public PlatformFocusPlatformEffect() : base()
		{
		}

		protected override void OnAttached()
		{
			try
			{
				if (Control is Microsoft.UI.Xaml.Controls.Control control)
					control.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Cyan);
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
	public class PlatformFocusPlatformEffect : Microsoft.Maui.Controls.Platform.PlatformEffect
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
	public class PlatformFocusPlatformEffect : Microsoft.Maui.Controls.Platform.PlatformEffect
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
#elif STANDARD
	public class PlatformFocusPlatformEffect
    {
	}
#endif
}