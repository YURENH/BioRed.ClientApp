using BioRed.ClientApp.ViewModels;
using Microsoft.Maui.Controls;

namespace BioRed.ClientApp.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly HomeViewModel _viewModel;

        public HomePage(HomeViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.CargarDatosInicialesAsync();
        }
    }
}