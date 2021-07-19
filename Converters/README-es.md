# Portar Converters de Xamarin.Forms a .NET MAUI

Los enlaces de datos generalmente transfieren datos de una propiedad de origen a una propiedad de destino y, en algunos casos, de la propiedad de destino a la propiedad de origen. Esta transferencia es sencilla cuando las propiedades de origen y destino son del mismo tipo, o cuando un tipo se puede convertir al otro tipo mediante una conversión implícita. Cuando ese no es el caso, se debe realizar un **Converter**.

Suponga que desea definir un enlace de datos donde la propiedad de origen es de tipo int pero la propiedad de destino es bool. Desea que este enlace de datos produzca un valor falso cuando la fuente entera sea igual a 0 y verdadero en caso contrario.

Puede hacer esto con una clase que implemente la interfaz **IValueConverter**.

Al igual que otros conceptos de Xamarin.Forms, los **Converters** se pueden reutilizar en .NET MAUI sin requerir cambios de código.

Veamos un ejemplo.

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

_¿Cuál es la diferencia?_. El código es exactamente el mismo excepto por un detalle, los espacios de nombres.

El espacio de nombres de Xamarin.Forms cambia al espacio de nombres **Microsoft.Maui.Controls**.