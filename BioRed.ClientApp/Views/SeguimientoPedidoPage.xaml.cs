using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using BioRed.ClientApp.ViewModels;

namespace BioRed.ClientApp.Views
{
    public partial class SeguimientoPedidoPage : ContentPage
    {
        private readonly SeguimientoPedidoViewModel _viewModel;

        public SeguimientoPedidoPage(SeguimientoPedidoViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            // Mantenemos una pequeña pausa para asegurar que la animación de navegación terminó
            await Task.Delay(300);

            if (_viewModel.OrderId > 0)
            {
                await _viewModel.CargarYMonitorearPedidoAsync(_viewModel.OrderId);
            }
        }

        protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
        {
            base.OnNavigatedFrom(args);

            // Detenemos el polling al salir de la pantalla
            _viewModel.DetenerMonitoreo();
        }
    }
}