using Effects.Effects;
using Microsoft.Maui.Controls.Hosting;

namespace Effects;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureEffects(effects =>
            {
                effects.Add(typeof(FocusRoutingEffect), typeof(PlatformFocusPlatformEffect));
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        return builder.Build();
    }
}