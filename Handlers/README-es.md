## Portear un Custom Renderer de Xamarin.Forms a un Custom Handler de .NET MAUI 

Las interfaces de usuario de Xamarin.Forms y .NET MAUI utilizan los controles nativos de la plataforma de destino, lo que permite que las aplicaciones conserven la apariencia adecuada para cada plataforma. El uso de Custom Renderers en Xamarin.Forms permite a los desarrolladores personalizar la apariencia y el comportamiento de los controles en cada plataforma.

Por diferentes motivos, entre los que podemos destacar mejoras en el rendimiento, más posibilidades de extensibilidad, en .NET MAUI tenemos el concepto de Custom Handler. Es similar al concepto de Custom Renderer, pero diferente en diferentes puntos.

¡Veamos paso a paso cómo convertir un Renderer a un Handler!. Para centrarnos en los conceptos clave de Renderers y Handlers, vamos a crear una Entry personalizado, un control bien conocido. Por otro lado, vamos a usar Android simplemente porque tanto desde Windows como desde macOS puedes lanzar las demos asociadas.

## Custom Renderer

Cada control de Xamarin.Forms tiene un Renderer adjunto para cada plataforma que crea una instancia de un control nativo. El proceso para crear un Renderer es el siguiente:

- Crea un control personalizado de Xamarin.Forms.
- Consume el control personalizado de Xamarin.Forms.
- Crea el Custom Renderer para el control en cada plataforma.

![Custom Renderer](custom-renderer.png)

### Creación del control Entry personalizado

Se puede crear un control personalizado creando una clase que herede de la clase View:

```
public class CustomEntry : View
{

}
```

El control CustomEntry se crea en el proyecto de la librería .NET Standard y es simplemente un control para capturar texto. Para personalizar la apariencia y el comportamiento del control podemos agregar BindableProperties y eventos.

```
public static readonly BindableProperty TextProperty =
    BindableProperty.Create(nameof(Text), typeof(string), typeof(Entry), string.Empty);


public string Text
{
    get { return (string)GetValue(TextProperty); }
    set { SetValue(TextProperty, value); }
}
```

### Consumir el control personalizado

Se puede hacer referencia al control CustomEntry en XAML en el proyecto de la librería .NET Standard declarando un espacio de nombres y usando el prefijo del espacio de nombres en el elemento de control.

```
<ContentPage ...
    xmlns:local="clr-namespace:CustomRenderer;assembly=CustomRenderer"
    ...>
    ...
    <local:CustomEntry Text="In Shared Code" />
    ...
</ContentPage>
```

### Creación del Custom Renderer en cada plataforma

El proceso para crear la clase del Custom Renderer es el siguiente:

1. Crea una clase que herede de **ViewRenderer** que representa el control nativo.
2. Sobreescribe el método **OnElementChanged** que representa el control nativo y la lógica para personalizar el control. Este método se llama cuando se crea el control de Xamarin.Forms correspondiente.
3. Sobreescribe el método **OnElementPropertyChanged** que responde a cualquier cambio de una BindableProperty.
4. Agrega el atributo **ExportRenderer** a la clase del Custom Renderer para especificar que se usará para representar el control Xamarin.Forms. Este atributo se usa para registrar el representador personalizado con Xamarin.Forms (usando Assembly Scanning).

La clase ViewRenderer expone el método OnElementChanged, al que se llama cuando se crea el control Xamarin.Forms para representar el control nativo correspondiente. Este método toma un parámetro **ElementChangedEventArgs** que contiene las propiedades OldElement y NewElement. Estas propiedades representan el elemento Xamarin.Forms al que se adjuntó el representador y el elemento Xamarin.Forms al que está adjunto el representador, respectivamente. En la aplicación de ejemplo, la propiedad OldElement será nula y la propiedad NewElement contendrá una referencia al control CustomEntry.

El método OnElementChanged en la clase CustomEntryRenderer es el lugar para realizar la personalización del control nativo. Se puede acceder a una referencia del control nativo que se usa en la plataforma a través de la propiedad Control. Además, se puede obtener una referencia al control Xamarin.Forms que se está representando a través de la propiedad Element.

```
protected override void OnElementChanged(ElementChangedEventArgs<CustomEntry> e)
{
    base.OnElementChanged(e);

    if (e.OldElement == null)
    {
        EditText editText = new EditText(Context);

        _defaultTextColors = editText.TextColors;
        _defaultPlaceholderColors = editText.HintTextColors;

        SetNativeControl(editText);
    }

    UpdateText();
    UpdateTextColor();
    UpdatePlaceholder();
    UpdatePlaceholderColor();
    UpdateCharacterSpacing();
    UpdateHorizontalTextAlignment();
    UpdateVerticalTextAlignment();
}
```

La invalidación OnElementPropertyChanged responde a los cambios de BindableProperties enlazadas al control Xamarin.Forms.

```
protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
{
    if (e.PropertyName == CustomEntry.TextProperty.PropertyName)
        UpdateText();
    else if (e.PropertyName == CustomEntry.TextColorProperty.PropertyName)
        UpdateTextColor();
    if (e.PropertyName == CustomEntry.PlaceholderProperty.PropertyName)
        UpdatePlaceholder();
    else if (e.PropertyName == Entry.PlaceholderColorProperty.PropertyName)
        UpdatePlaceholderColor();
    else if (e.PropertyName == Entry.CharacterSpacingProperty.PropertyName)
        UpdateCharacterSpacing();
    else if (e.PropertyName == Entry.HorizontalTextAlignmentProperty.PropertyName)
        UpdateHorizontalTextAlignment();
    else if (e.PropertyName == Entry.VerticalTextAlignmentProperty.PropertyName)
        UpdateVerticalTextAlignment();

    base.OnElementPropertyChanged(sender, e);
}
```

Nada realmente nuevo hasta ahora, solo una pequeña revisión sobre cómo trabajar con Custom Renderers en Xamarin.Forms, ¿vamos a .NET MAUI?

## Pero primero, ¿por qué cambiar de la arqquitectura de Renderers a la de Handlers en .NET MAUI?

Para comprender las motivaciones que impulsan el cambio, incluso conociendo las implicaciones en la cantidad de cambios necesarios, necesitamos saber qué es lo que está mal o es mejorable en los Renderers.

_¿Recuerdas el atributo ExportRenderer que usa para registrar el Renderer?_ Esto le dice a Xamarin.Forms que en el arranque, haciendo uso del escaneo de ensamblados, debe buscar todas las bibliotecas referenciadas y usando este atributo, y si lo encuentra, registra el Renderer. Es fácil de usar, pero... **el escaneo del ensamblados es lento y penaliza el inicio**.

**El método OnElementChanged generalmente causa confusión**. ¿Cuándo usar OldElement , y NewElement?, ¿cuándo creo valores por defecto o me suscribo a eventos?. Causar confusión es un problema, pero es un problema aún mayor si no tener una forma fácil de suscribirse/desuscribirse hace que a veces no se anule la suscripción (por ejemplo) y... penaliza el rendimiento.

Todos esos **métodos privados** para actualizar las propiedades del control nativo son un gran problema. Es posible que debas hacer un pequeño cambio y debido a la falta de acceso (nuevamente, ¡métodos privados aquí y allá!), terminas creando un Custom Renderer más grande de lo necesario, etc.

Resolver estos problemas, y otros menores, es el objetivo fundamental de los Handlers.

## Custom Handlers

El proceso para crear la clase del Custom Handler es el siguiente:

1. Crea una clase que implementa la clase **ViewHandler** que representa el control nativo.
2. Sobrecarga el método **CreateNativeView** que representa el control nativo.
3. Crea el diccionario **Mapper** que responda a los cambios de propiedad.
4. Registra el controlador usando el método **AddHandler** en la clase Startup.


![Custom Handler](custom-handler.png)

### Creación del control Entry personalizado

Los Handlers utilizan interfaces que derivan de la interfaz **IView**. Esto evita que el control multiplataforma tenga que hacer referencia a su Handler y que el Handler tenga que hacer referencia al control multiplataforma. El Mapper se encarga de mapear las propiedades multiplataforma a lo nativo en cada plataforma.

De esta forma comenzamos a crear la interfaz que define nuestro control:

```
public interface ICustomEntry : IView
{
    public string Text { get; }
    public Color TextColor { get; }
    public string Placeholder { get; }
    public Color PlaceholderColor { get; }
    public double CharacterSpacing { get; }
    public TextAlignment HorizontalTextAlignment { get; }
    public TextAlignment VerticalTextAlignment { get; }

    void Completed();
}
```

Se puede crear un control personalizado heredando de la clase View e implementando la interfaz del control:

```
public class CustomEntry : View, ICustomEntry
{

}
```

### Crear el Custom Handler cada plataforma

Cree una subclase de la clase **ViewHandler** que representa el control nativo.

```
public partial class CustomEntryHandler : ViewHandler<ICustomEntry, EditText>
{

}
```

Parece un cambio trivial, pasamos de heredar de ViewRenderer a ViewHandler, ¡pero es mucho más!.

ViewRenderer en Xamarin.Forms crea un elemento padre, en el caso de Android un ViewGroup, que se usaba para tareas de posicionamiento auxiliares. **ViewHandler NO crea ningún elemento padre** que ayudea a reducir la jerarquía visual y, por lo tanto, mejora el rendimiento.

Heredando de ViewHandler, tenemos que implementar el método **CreateNativeView**.

```
protected override EditText CreateNativeView()
{
    return new EditText(Context);
}
```

¿Recuerdas que anteriormente revisamos cómo se usó OnElementChanged en Xamarin.Forms?. En este método creamos el control nativo, inicializamos valores predeterminados, nos suscribimos a eventos, etc. Sin embargo, requiere una gran diversidad de conocimientos: qué es OldElement y NewElement, etc.

.NET MAUI simplifica y distribuye todo lo que hicimos anteriormente en el método OnElementChanged en diferentes métodos de una manera más sencilla.

Creamos el control nativo en el método CreateNativeView. Por otro lado, tenemos otros métodos como **ConnectHandler** y **DisconnectHandler**.

```
protected override void ConnectHandler(EditText nativeView)
{
    _defaultTextColors = nativeView.TextColors;
    _defaultPlaceholderColors = nativeView.HintTextColors;

    _watcher.Handler = this;
    nativeView.AddTextChangedListener(_watcher);

    base.ConnectHandler(nativeView);
}

protected override void DisconnectHandler(EditText nativeView)
{
    nativeView.RemoveTextChangedListener(_watcher);
    _watcher.Handler = null;

    base.DisconnectHandler(nativeView);
}
```

ConnectHandler es el lugar ideal para inicializar, suscribir eventos, etc. y de la misma manera podemos eliminar, cancelar la suscripción de eventos, etc. en DisconnectHandler.

### El Mapper

** Mapper ** es un nuevo concepto introducido por los Handlers en .NET MAUI. No es más que un diccionario con las propiedades (y acciones) definidas en la interfaz de nuestro control (recuerda, usamos interfaces en el Handler). Reemplaza todo lo que se hacía en el método OnElementPropertyChanged en Xamarin.Forms. 

```
public static PropertyMapper<ICustomEntry, CustomEntryHandler> CustomEntryMapper = new PropertyMapper<ICustomEntry, CustomEntryHandler>(ViewHandler.ViewMapper)
{
    [nameof(ICustomEntry.Text)] = MapText,
    [nameof(ICustomEntry.TextColor)] = MapTextColor,
    [nameof(ICustomEntry.Placeholder)] = MapPlaceholder,
    [nameof(ICustomEntry.PlaceholderColor)] = MapPlaceholderColor,
    [nameof(ICustomEntry.CharacterSpacing)] = MapCharacterSpacing,
    [nameof(ICustomEntry.HorizontalLayoutAlignment)] = MapHorizontalLayoutAlignment,
    [nameof(ICustomEntry.VerticalLayoutAlignment)] = MapVerticalLayoutAlignment
};
```

Mapper mapea propiedades a métodos estáticos.

```
public static void MapText(CustomEntryHandler handler, ICustomEntry entry)
{
    handler.NativeView?.UpdateText(entry);
}
```

El Mapper, además de simplificar la gestión de cambios de propiedad (notifica las propiedades al inicializar el control y también, cada vez que cambia una propiedad) nos permite más opciones de extensibilidad.

Por ejemplo:

using Microsoft.Maui;
using Microsoft.Maui.Controls;

```
public partial class App : Application
{
    public App()
    {
        InitializeComponent();

#if __ANDROID__
        CustomEntryMapper[nameof(ICustomEntry.Text)] = (handler, view) =>
        {
            (handler.NativeView as Android.Widget.EditText).Text = view.text + "custom";
        };
#endif
    }
}
```

### Registrar el handler

A diferencia de Xamarin.Forms que usa el atributo ExportRenderer que a su vez hace uso del escaneo de ensamblado, en .NET MAUI el registro del handler es ligeramente diferente.

```
appBuilder
    .UseMauiApp<App>()
    .ConfigureMauiHandlers(handlers =>
    {
#if __ANDROID__
        handlers.AddHandler(typeof(CustomEntry), typeof(CustomEntryHandler));
#endif
    });
```

Hacemos uso de AppHostBuilder y el método **AddHandler** para registrar el Handler. Al final requiere, como en Xamarin.Forms, una línea para indicar que queremos dar de alta el Handler, pero se evita el uso de Assembly Scanning, que es lento y costoso, penalizando el inicio.