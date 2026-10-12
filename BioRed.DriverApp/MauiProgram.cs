using BioRed.DriverApp.ViewModels;
using BioRed.DriverApp.Views;
using BioRed.Services.Api;
using BioRed.Services.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;


namespace BioRed.DriverApp
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Servicios
            builder.Services.AddSingleton<ApiClient>();
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();

            return builder.Build();
        }
    }
}