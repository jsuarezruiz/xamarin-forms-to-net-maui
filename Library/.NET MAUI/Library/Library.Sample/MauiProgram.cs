using Library.Hosting;

namespace Library.Sample
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureLibrary();

            return builder.Build();
        }
    }
}