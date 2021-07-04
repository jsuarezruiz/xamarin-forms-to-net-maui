# Port Xamarin.Forms Triggers to .NET MAUI

**Triggers** allow you to express actions declaratively in XAML that change the appearance of controls based on events or property changes. In addition, state triggers, which are a specialized group of triggers, define when a VisualState should be applied.

As a general rule, all XAML-related concepts in Xamarin.Forms will work without require changes in .NET MAUI. 

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