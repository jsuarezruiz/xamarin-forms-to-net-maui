# Portar Triggers de Xamarin.Forms a .NET MAUI

Los **Triggers**  permiten expresar acciones de forma declarativa en XAML que cambian la apariencia de los controles en función de eventos o cambios de propiedad. Además, los state triggers, que son un grupo especializado de triggers, definen cuándo se debe aplicar un VisualState.

Como regla general, todos los conceptos relacionados con XAML en Xamarin.Forms funcionarán sin requerir cambios en .NET MAUI.

Xamarin.Forms

```
<Entry 
    x:Name="Entry"
    Text=""
    Placeholder="Required field" />
<!-- Referenced below in DataTrigger-->
<Button 
    x:Name="Button"
    Text="Save"
    FontSize="Large"
    HorizontalOptions="Center">
    <Button.Triggers>
        <DataTrigger
            TargetType="Button"
            Binding="{Binding Source={x:Reference Entry},
                            Path=Text.Length}"
            Value="0">
            <Setter Property="IsEnabled" Value="False" />
        </DataTrigger>
    </Button.Triggers>
</Button>
```

.NET MAUI

```
<Entry 
    x:Name="Entry"
    Text=""
    Placeholder="Required field" />
<!-- Referenced below in DataTrigger-->
<Button 
    x:Name="Button"
    Text="Save"
    FontSize="Large"
    HorizontalOptions="Center">
    <Button.Triggers>
        <DataTrigger
            TargetType="Button"
            Binding="{Binding Source={x:Reference Entry},
                            Path=Text.Length}"
            Value="0">
            <Setter Property="IsEnabled" Value="False" />
        </DataTrigger>
    </Button.Triggers>
</Button>
```