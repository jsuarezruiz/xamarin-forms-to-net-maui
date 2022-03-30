using Library.Controls;
using Library.Effects;
using Library.Handlers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Maui.Controls.Compatibility.Hosting;

namespace Library.Hosting
{
    public static class AppHostBuilderExtensions
    {
        public static MauiAppBuilder ConfigureLibrary(this MauiAppBuilder builder, bool useCompatibilityRenderers = false)
        {
            builder
                .ConfigureEffects(effects =>
                {
                    effects.Add(typeof(FocusRoutingEffect), typeof(PlatformFocusPlatformEffect));
                })
                .ConfigureMauiHandlers(handlers =>
                {
                    if (useCompatibilityRenderers)
                        handlers.AddLibraryCompatibilityRenderers();
                    else
                        handlers.AddLibraryHandlers();
                });

            return builder;
        }

        public static IMauiHandlersCollection AddLibraryCompatibilityRenderers(this IMauiHandlersCollection handlers)
        {
#if __ANDROID__
            handlers.AddCompatibilityRenderer(typeof(CustomEntry), typeof(Renderers.Android.CustomEntryRenderer));
#endif
            return handlers;
        }

        public static IMauiHandlersCollection AddLibraryHandlers(this IMauiHandlersCollection handlers)
        {
            handlers.AddTransient(typeof(CustomEntry), h => new CustomEntryHandler());
        
            return handlers;
        }
    }
}