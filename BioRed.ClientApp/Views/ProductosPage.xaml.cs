using BioRed.ClientApp.ViewModels;
using BioRed.Core.DTOs;

namespace BioRed.ClientApp.Views;

public partial class ProductosPage : ContentPage
{
    private readonly ProductosViewModel _viewModel;

    public ProductosPage(ProductosViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }


    private async void OnCarritoClicked(object sender, EventArgs e)
    {
        // Resolvemos CarritoPage usando la inyección de dependencias
        var carritoPage = Handler!.MauiContext!.Services.GetRequiredService<CarritoPage>();
        await Navigation.PushAsync(carritoPage);
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ProductosViewModel viewModel)
        {
            await viewModel.CargarDatosInicialesAsync();
        }
    }
    private async void OnProductoSeleccionado(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is ProductoDto productoSeleccionado && _viewModel.SucursalSeleccionada != null)
        {
            // Resolvemos el ViewModel del detalle desde la inyección de dependencias
            var detalleVM = Handler!.MauiContext!.Services.GetRequiredService<ProductoDetalleViewModel>();

            // Cargamos la información del producto y sucursal elegida
            detalleVM.CargarProducto(productoSeleccionado, _viewModel.SucursalSeleccionada.BranchCode);

            // Navegamos hacia la pantalla de detalle
            var detallePage = new ProductoDetallePage(detalleVM);
            await Navigation.PushAsync(detallePage);

            // Limpiamos la selección activa para corregir el estado del CollectionView
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}