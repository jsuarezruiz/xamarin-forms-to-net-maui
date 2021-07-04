# Port Xamarin.Forms Converters to .NET MAUI

Data bindings usually transfer data from a source property to a target property, and in some cases from the target property to the source property. This transfer is straightforward when the source and target properties are of the same type, or when one type can be converted to the other type through an implicit conversion. When that is not the case, a **type conversion** must take place.

Suppose you want to define a data binding where the source property is of type int but the target property is a bool. You want this data binding to produce a false value when the integer source is equal to 0, and true otherwise.

You can do this with a class that implements the **IValueConverter** interface.

Like other Xamarin.Forms concepts, **Converters** can be reused in  .NET MAUI without require code changes.

Let's see an example. 

Xamarin.Forms

```
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Converters
{
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1 : 0;
        }
    }
}
```
.NET MAUI

```
using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace Converters
{
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1 : 0;
        }
    }
}
```

_What is the difference?_. The code is exactly the same except for one detail, namespaces.

Xamarin.Forms namespace changes to **Microsoft.Maui.Controls** namespace. 