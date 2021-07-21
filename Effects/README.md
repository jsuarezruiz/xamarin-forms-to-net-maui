# Port Xamarin.Forms Effects to .NET MAUI

Xamarin.Forms user interfaces are rendered using the native controls of the target platform, allowing Xamarin.Forms applications to retain the appropriate look and feel for each platform. Effects allow the native controls on each platform to be customized without having to resort to a custom renderer implementation.

With the new extensibility possibilities of .NET MAUI Handlers, the use of Effects will probably decrease but even so, there is the same concept and porting effects is simple.

## Xamarin.Forms

The process for creating an effect in each platform-specific project is as follows:

1. Create a subclass of the PlatformEffect class.
2. Override the OnAttached method and write logic to customize the control.
3. Override the OnDetached method and write logic to clean up the control customization, if required.
4. Add a ResolutionGroupName attribute to the effect class. This attribute sets a company wide namespace for effects, preventing collisions with other effects with the same name. Note that this attribute can only be applied once per project.
5. Add an ExportEffect attribute to the effect class. This attribute registers the effect with a unique ID that's used by Xamarin.Forms, along with the group name, to locate the effect prior to applying it to a control. The attribute takes two parameters – the type name of the effect, and a unique string that will be used to locate the effect prior to applying it to a control.


### Creating the Effect on Each Platform

```
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("MyCompany")]
[assembly: ExportEffect(typeof(EffectsDemo.Droid.FocusEffect), nameof(EffectsDemo.Droid.FocusEffect))]
namespace EffectsDemo.Droid
{
    public class FocusEffect : PlatformEffect
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

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
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
}
```

The OnAttached method calls the SetBackgroundColor method to set the background color of the control to light green, and also stores this color in a field. This functionality is wrapped in a try/catch block in case the control the effect is attached to does not have a SetBackgroundColor property. No implementation is provided by the OnDetached method because no cleanup is necessary.

The OnElementPropertyChanged override responds to bindable property changes on the Xamarin.Forms control. When the IsFocused property changes, the background color of the control is changed to white if the control has focus, otherwise it's changed to light green. This functionality is wrapped in a try/catch block in case the control the effect is attached to does not have a BackgroundColor property.

### Consuming the Effect in XAML

The following XAML code example shows an Entry control to which the FocusEffect is attached:

```
<Entry Text="Effect attached to an Entry" ...>
    <Entry.Effects>
        <local:FocusEffect />
    </Entry.Effects>
    ...
</Entry>
```

The FocusEffect class subclasses the RoutingEffect class, which represents a platform-independent effect that wraps an inner effect that is usually platform-specific. The FocusEffect class calls the base class constructor, passing in a parameter consisting of a concatenation of the resolution group name (specified using the ResolutionGroupName attribute on the effect class), and the unique ID that was specified using the ExportEffect attribute on the effect class. Therefore, when the Entry is initialized at runtime, a new instance of the Effects.FocusEffect is added to the control's Effects collection.

```
public class FocusEffect : RoutingEffect
{
    public FocusEffect () : base ($"Effects.{nameof(FocusEffect)}")
    {
    }
}
```
## .NET MAUI

We can reuse all the effects code by simply changing the namespace from Xamarin.Forms to Microsoft.Maui.Controls. 

```
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
```

Where if we have an important change is when registering the effect. 

In Xamarin.Forms we use the  ExportAttribure to register the effect with a unique ID that's used by Xamarin.Forms, along with the group name, to locate the effect prior to applying it to a control. As with Renderers, this attribute makes use of Assembly Scanning which is slow and penalizes performance. 

In .NET MAUI we use the AppHostBuilder and **ConfigureEffects** to register effects. 

```
appBuilder
    .ConfigureEffects(effects =>
    {
        effects.Add<FocusRoutingEffect, FocusPlatformEffect>();
    });
```