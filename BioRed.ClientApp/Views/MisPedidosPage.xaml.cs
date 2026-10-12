using BioRed.ClientApp.ViewModels;

namespace BioRed.ClientApp.Views
{
    public partial class MisPedidosPage : ContentPage
    {
        private readonly MisPedidosViewModel _viewModel;

        public MisPedidosPage(MisPedidosViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel != null)
            {
                await _viewModel.CargarPedidosAsync();
            }
        }
    }
}