using BioRed.ClientApp.ViewModels;

namespace BioRed.ClientApp.Views;

public partial class ProductoDetallePage : ContentPage
{
    public ProductoDetallePage(ProductoDetalleViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}