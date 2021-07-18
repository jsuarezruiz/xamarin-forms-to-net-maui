## Port a Xamarin.Forms Custom Renderer to a .NET MAUI Custom Handler

Xamarin.Forms and .NET MAUI user interfaces are rendered using the native controls of the target platform, allowing applications to retain the appropriate look and feel for each platform. Using Xamarin.Forms Custom Renderers let developers override this process to customize the appearance and behavior of controls on each platform.

For different reasons, among which we can highlight the improvement in performance, more extensibility possibilities, in .NET MAUI we have the concept of Custom Handler. It is similar to the Custom Renderer concept but different elsewhere.

Let's see step by step how to convert a renderer to a handler!. To focus on the key concepts of Renderers and Handlers we are going to create a custom Entry, a well-known control. On the other hand, we are going to use Android simply because both from Windows and from macOS you can launch the demos. 

## Custom Renderer

Every Xamarin.Forms control has an accompanying renderer for each platform that creates an instance of a native control. The process for creating a renderer is as follows:

- Create a Xamarin.Forms custom control.
- Consume the custom control from Xamarin.Forms.
- Create the custom renderer for the control on each platform.

![Custom Renderer](custom-renderer.png)

### Creating the Custom Entry Control

A custom control can be created by subclassing the View class:

```
public class CustomEntry : View
{

}
```

The CustomEntry control is created in the .NET Standard library project and is simply an control to capture text. To customize the appearance and behavior of the control we can add BindableProperties and events. 

```
public static readonly BindableProperty TextProperty =
    BindableProperty.Create(nameof(Text), typeof(string), typeof(Entry), string.Empty);


public string Text
{
    get { return (string)GetValue(TextProperty); }
    set { SetValue(TextProperty, value); }
}
```

### Consuming the Custom Control

The CustomEntry control can be referenced in XAML in the .NET Standard library project by declaring a namespace for its location and using the namespace prefix on the control element. 

```
<ContentPage ...
    xmlns:local="clr-namespace:CustomRenderer;assembly=CustomRenderer"
    ...>
    ...
    <local:CustomEntry Text="In Shared Code" />
    ...
</ContentPage>
```

### Creating the Custom Renderer on each Platform

The process for creating the custom renderer class is as follows:

1. Create a subclass of the **ViewRenderer** class that renders the native control.
2. Override the **OnElementChanged** method that renders the native control and write logic to customize the control. This method is called when the corresponding Xamarin.Forms control is created.
3. Override the **OnElementPropertyChanged** method that respond to any BindableProperty change.
4. Add an **ExportRenderer** attribute to the custom renderer class to specify that it will be used to render the Xamarin.Forms control. This attribute is used to register the custom renderer with Xamarin.Forms.

The ViewRenderer class exposes the OnElementChanged method, which is called when the Xamarin.Forms control is created to render the corresponding native control. This method takes an **ElementChangedEventArgs** parameter that contains OldElement and NewElement properties. These properties represent the Xamarin.Forms element that the renderer was attached to, and the Xamarin.Forms element that the renderer is attached to, respectively. In the sample application the OldElement property will be null and the NewElement property will contain a reference to the CustomEntry control.

An overridden version of the OnElementChanged method in the CustomEntryRenderer class is the place to perform the native control customization. A typed reference to the native control being used on the platform can be accessed through the Control property. In addition, a reference to the Xamarin.Forms control that's being rendered can be obtained through the Element property, although it's not used in the sample application.

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

The OnElementPropertyChanged override responds to bindable property changes on the Xamarin.Forms control.

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

Nothing really new so far, just a small review on how to work with Custom Renderers in Xamarin.Forms, let's go to .NET MAUI? 

## But first, why switch from Renderers to .NET MAUI architecture? 

To understand the motivations that drive the change, even knowing the implications in the number of changes necessary, we need to know what is wrong or improvable in the renderers. 

_Do you remember the ExportRenderer attribute that you use to register the Renderer?_ This tells Xamarin.Forms that at startup, making use of assembly scanning, it should search all the libraries referenced and using this attribute, and if it finds it, register the renderer. It's easy to use, but ... **assembly scanning is slow and penalizes startup**.

**OnElementChanged method usually causes confusion**. When to use OldElement?, and NewElement?, when do I create my default values or subscribe to events?, and when do I unsubscribe?. Cause confusion is a problem, but it is even bigger problem if not having a easy way to subscribe/unsubscribe causes to sometimes unsubscribe (for example) and... penalizes performance. 

All those **private methods** to update properties of the native control are a big problem. You may need to make a small change and due to lack of access (again, private methods here and there!), you end up creating a Custom Renderer larger than necessary, etc. 

Solving these problems, and other minor ones is the fundamental objective of the Handlers. 