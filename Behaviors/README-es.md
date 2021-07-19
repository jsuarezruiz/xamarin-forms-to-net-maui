# Portar Behaviors de Xamarin.Forms a .NET MAUI

Los Behaviors permiten agregar funciones a los controles de la interfaz de usuario sin tener que incluirlos en subclases. En su lugar, la función se implementa en una clase Behavior y se asocia al control como si fuera parte de este. 

En .NET MAUI existe exactamente el mismo concepto, permitiendo reutilizar código de forma sencilla. 

Veamos un ejemplo.

Xamarin.Forms

```
using Xamarin.Forms;

namespace Behaviors
{
    public class NumericValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            bool isValid = double.TryParse(args.NewTextValue, out double result);
            ((Entry)sender).TextColor = isValid ? Color.Default : Color.Red;
        }
    }
}
```
.NET MAUI

```
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Behaviors
{
    public class NumericValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            bool isValid = double.TryParse(args.NewTextValue, out double result);
            ((Entry)sender).TextColor = isValid ? Colors.Black : Colors.Red;
        }
    }
}
```

_¿Cuál es la diferencia?_. El código es exactamente el mismo excepto por un detalle, namespaces.

los namespace de Xamarin.Forms cambian por el n amespace **Microsoft.Maui.Controls**. 

Por otro lado, todos los tipos básicos como: Color, Rectangle o Point ahora estan dispoinles en **Microsoft.Maui.Graphics**. Por esa razón, y porque usamos colores en este Behavior, también incluimos como cambio el nuevo namespace de Graphics.