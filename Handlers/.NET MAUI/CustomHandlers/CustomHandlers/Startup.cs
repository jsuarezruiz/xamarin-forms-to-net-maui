using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using CustomHandlers.Handlers;

[assembly: XamlCompilationAttribute(XamlCompilationOptions.Compile)]

namespace CustomHandlers
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
                .ConfigureMauiHandlers(handlers =>
                {
#if __ANDROID__
                    handlers.AddHandler(typeof(CustomEntry), typeof(CustomEntryHandler));
#endif
                });
        }
    }
}