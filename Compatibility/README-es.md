# ¿Qué es el paquete de Compatibilidad?

Para hacer transición de Xamarin.Forms a .NET MAUI lo más fluida posible, se agrega el paquete **Compatability** que agrega la funcionalidad de Xamarin.Forms que permite reutilizar código como Custom Renderers sin necesidad de realizar cambios.

## ¿Custom Renderers de Xamarin.Forms?

Las interfaces de usuario de Xamarin.Forms se crean mediante controles nativos de la plataforma de destino, lo que permite que las aplicaciones de Xamarin.Forms conserven la apariencia adecuada para cada plataforma. Los Custom Renderers permiten a los desarrolladores utilizar este proceso para personalizar la apariencia y el comportamiento de los controles de Xamarin.Forms en cada plataforma.

Veamos un ejemplo. Vamos a crear un Entry personalizada.

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

Como podemos ver, solo estamos modificando el color de fondo del control nativo. Algo realmente simple, pero suficiente para el propósito de este documento, para aprender a reutilizar Renderers sin cambiar el código de los mismos.

## Reutilizar el Renderer

El Renderer en .NET MAUI usa exactamente el mismo código:

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

Los únicos cambios son reemplazar algunos espacios de nombres para usar **Microsoft.Maui.Controls.Compatibility**.

_¿Y ya está todo listo?_

No exactamente, en lugar de usar Assembly Scanning, .NET MAUI usa la clase **Startup** para realizar tareas como registrar Handlers o Renderers.

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
