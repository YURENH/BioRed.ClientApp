using BioRed.ClientApp.ViewModels;
using BioRed.ClientApp.Views;
using BioRed.Services;
using BioRed.Services.Api;
using BioRed.Services.Auth;
using BioRed.Services.Cart;
using BioRed.Services.Direcciones;
using BioRed.Services.Pagos;
using BioRed.Services.Pagos.Interfaces;
using BioRed.Services.Pedidos;
using BioRed.Services.Pedidos.Interfaces;
using BioRed.Services.Productos;
using BioRed.Services.RealTime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using System;

namespace BioRed.ClientApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG




            builder.Services.AddHttpClient<IGeolocationService, GeolocationService>(client =>
            {
                client.BaseAddress = new Uri("https://biored-api-backend-c4hre9ajabb9dmbf.westus3-01.azurewebsites.net/");
            })
.AddHttpMessageHandler<AuthHeaderHandler>();
            builder.Services.AddTransient<AuthHeaderHandler>();

            // 2. Agregar .AddHttpMessageHandler<AuthHeaderHandler>() a cada servicio que requiera autenticación
            builder.Services.AddHttpClient<IPedidoService, PedidoService>(client =>
            {
                client.BaseAddress = new Uri("https://biored-api-backend-c4hre9ajabb9dmbf.westus3-01.azurewebsites.net/");
            })
            .AddHttpMessageHandler<AuthHeaderHandler>();

            builder.Services.AddHttpClient<IPagoService, PagoService>(client =>
            {
                client.BaseAddress = new Uri("https://biored-api-backend-c4hre9ajabb9dmbf.westus3-01.azurewebsites.net/");
            })
            .AddHttpMessageHandler<AuthHeaderHandler>();
            builder.Logging.AddDebug();
#endif

            // 🌐 Base URL de Azure
            var apiUri = new Uri("https://biored-api-backend-c4hre9ajabb9dmbf.westus3-01.azurewebsites.net/");

            // 1. Servicios HTTP registrados por Interfaz
            builder.Services.AddHttpClient<IPedidoService, PedidoService>(client => client.BaseAddress = apiUri);
            builder.Services.AddHttpClient<IPagoService, PagoService>(client => client.BaseAddress = apiUri);

            // 2. Mapeo para permitir inyectar Clases Concretas directamente si un ViewModel las solicita
            builder.Services.AddTransient<PedidoService>(sp => (PedidoService)sp.GetRequiredService<IPedidoService>());
            builder.Services.AddTransient<PagoService>(sp => (PagoService)sp.GetRequiredService<IPagoService>());

            // 3. Servicios Singleton
            builder.Services.AddSingleton<ApiClient>();
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<ProductoService>();
            builder.Services.AddSingleton<CartService>();
            builder.Services.AddSingleton<DireccionService>();

            // 4. ViewModels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<ProductosViewModel>();
            builder.Services.AddTransient<ProductoDetalleViewModel>();
            builder.Services.AddTransient<CarritoViewModel>();
            builder.Services.AddTransient<MisPedidosViewModel>();
            builder.Services.AddTransient<SeguimientoPedidoViewModel>();
            builder.Services.AddTransient<PaypalViewModel>();
            builder.Services.AddTransient<RecuperarContrasenaViewModel>();
            builder.Services.AddTransient<RecuperarContrasenaPage>();
            Routing.RegisterRoute(nameof(RecuperarContrasenaPage), typeof(RecuperarContrasenaPage));
            // 5. Vistas / Páginas
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<ProductosPage>();
            builder.Services.AddTransient<ProductoDetallePage>();
            builder.Services.AddTransient<CarritoPage>();
            builder.Services.AddTransient<MisPedidosPage>();
            builder.Services.AddTransient<SeguimientoPedidoPage>();
            builder.Services.AddTransient<PaypalPage>();

            return builder.Build();
        }
    }
}