using BioRed.ClientApp.Views;
using Microsoft.Maui.Controls;

namespace BioRed.ClientApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // ⚠️ NO registrar HomePage aquí si ya existe en AppShell.xaml
            // Registra SOLO las páginas secundarias a las que vas con navegación (Push)
            Routing.RegisterRoute(nameof(ProductosPage), typeof(ProductosPage)); // Ajusta si se llama distinto
            Routing.RegisterRoute(nameof(CarritoPage), typeof(CarritoPage));
            Routing.RegisterRoute(nameof(SeguimientoPedidoPage), typeof(SeguimientoPedidoPage));
            Routing.RegisterRoute(nameof(MisPedidosPage), typeof(MisPedidosPage));
            Routing.RegisterRoute(nameof(SeguimientoPedidoPage), typeof(SeguimientoPedidoPage));

        }
    }
}