using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using Effects.Effects;

[assembly: XamlCompilationAttribute(XamlCompilationOptions.Compile)]

namespace Effects
{
    public class Startup : IStartup
    {
        public void Configure(IAppHostBuilder appBuilder)
        {
            appBuilder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                })
                .ConfigureEffects(effects =>
                {
                    effects.Add<FocusRoutingEffect, FocusPlatformEffect>();
                });
        }
    }
}