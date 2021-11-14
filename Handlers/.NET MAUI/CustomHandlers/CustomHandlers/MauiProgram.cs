using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using CustomHandlers.Handlers;

[assembly: XamlCompilationAttribute(XamlCompilationOptions.Compile)]

namespace CustomHandlers
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
           
            builder    
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
            
            return builder.Build();
        }
    }
}