using BioRed.ClientApp.ViewModels;

namespace BioRed.ClientApp.Views
{
    public partial class CarritoPage : ContentPage
    {
        public CarritoPage(CarritoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // 🟢 Al abrir la pantalla, recarga el carrito y consulta las direcciones en Azure
            if (BindingContext is CarritoViewModel viewModel)
            {
                await viewModel.CargarDatosInicialesAsync();
            }
        }
    }
}