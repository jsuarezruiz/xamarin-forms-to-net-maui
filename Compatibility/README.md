# What is the Compatibility package?

To make the transition from Xamarin.Forms to .NET MAUI as smooth as possible, the **Compatability** package is added that adds Xamarin.Forms functionality allowing to reuse code such as Custom Renderers without require to make changes. 

## Xamarin.Forms Custom Renderers?

Xamarin.Forms user interfaces are rendered using the native controls of the target platform, allowing Xamarin.Forms applications to retain the appropriate look and feel for each platform. Custom Renderers let developers override this process to customize the appearance and behavior of Xamarin.Forms controls on each platform.

Let's see an example. We are going to create a custom Entry.

```
using Xamarin.Forms;

namespace Renderers
{
    public class CustomEntry : Entry
    {

    }
}
```

Android Implementation

```
using Android.Content;
using Renderers;
using Renderers.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace Renderers.Droid.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundColor(global::Android.Graphics.Color.LightGreen);
            }
        }
    }
}
```

As we can see we are only modifying the background color of the native control. Something really simple, but enough for the purpose of this document, to learn how to reuse renderers without changing the code. 

## Reuse the Renderer

The Renderer in .NET MAUI use exactly the same code:

```
using Android.Content;
using Compatibility;
using Compatibility.Droid.Renderers;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace Compatibility.Droid.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundColor(global::Android.Graphics.Color.LightGreen);
            }
        }
    }
}
```

The only changes are replace some namespace to use **Microsoft.Maui.Controls.Compatibility**.

_And is everything is ready?_

Not quite, instead of using assembly scanning, .NET MAUI uses the **Startup** class to perform tasks like registering Handlers or Renderers.

```
public class Startup : IStartup
{
    public void Configure(IAppHostBuilder appBuilder)
    {
        appBuilder
            .UseMauiApp<App>()
            .ConfigureMauiHandlers(handlers =>
            {
#if __ANDROID__
                handlers.AddCompatibilityRenderer(typeof(CustomEntry), typeof(Droid.Renderers.CustomEntryRenderer));
#endif
            });
    }
}
```
