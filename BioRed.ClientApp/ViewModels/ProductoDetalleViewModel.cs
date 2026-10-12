using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BioRed.Core.DTOs;
using BioRed.Services.Cart;

namespace BioRed.ClientApp.ViewModels
{
    public partial class ProductoDetalleViewModel : ObservableObject
    {
        private readonly CartService _cartService;

        [ObservableProperty]
        private ProductoDto _producto = new();

        [ObservableProperty]
        private string _branchCode = string.Empty;

        [ObservableProperty]
        private int _cantidad = 1;

        [ObservableProperty]
        private decimal _subtotal;

        public ProductoDetalleViewModel(CartService cartService)
        {
            _cartService = cartService;
        }

        public void CargarProducto(ProductoDto producto, string branchCode)
        {
            Producto = producto;
            BranchCode = branchCode;
            Cantidad = 1;
            CalcularSubtotal();
        }

        [RelayCommand]
        private void Incrementar()
        {
            if (Cantidad < Producto.AvailableQuantity)
            {
                Cantidad++;
                CalcularSubtotal();
            }
        }

        [RelayCommand]
        private void Decrementar()
        {
            if (Cantidad > 1)
            {
                Cantidad--;
                CalcularSubtotal();
            }
        }

        private void CalcularSubtotal()
        {
            Subtotal = Producto.SalePrice * Cantidad;
        }

        [RelayCommand]
        private async Task AgregarAlCarritoAsync()
        {
            if (Producto.AvailableQuantity <= 0)
            {
                await Application.Current!.MainPage!.DisplayAlert("Sin Stock", "Producto agotado en esta sucursal.", "OK");
                return;
            }
            _cartService.AddToCart(Producto, BranchCode, Cantidad);
            await Application.Current!.MainPage!.DisplayAlert("¡Agregado!", $"{Cantidad} unidad(es) agregadas al carrito.", "OK");
            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync("..");
            }
            else if (Application.Current?.MainPage?.Navigation?.NavigationStack.Count > 1)
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }
    }
}