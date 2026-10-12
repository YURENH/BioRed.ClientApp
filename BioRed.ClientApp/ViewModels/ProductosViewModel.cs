using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BioRed.Core.DTOs;
using BioRed.Services.Productos;

namespace BioRed.ClientApp.ViewModels
{
    public partial class ProductosViewModel : ObservableObject
    {
        private readonly ProductoService _productoService;

        public ObservableCollection<ProductoDto> Productos { get; } = new();
        public ObservableCollection<BranchDto> Sucursales { get; } = new();

        [ObservableProperty]
        private BranchDto? _sucursalSeleccionada;

        [ObservableProperty]
        private bool _isBusy;

        public ProductosViewModel(ProductoService productoService)
        {
            _productoService = productoService;
        }

        [RelayCommand]
        public async Task CargarDatosInicialesAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var listaSucursales = await _productoService.ObtenerSucursalesAsync();

                Sucursales.Clear();
                foreach (var suc in listaSucursales)
                {
                    Sucursales.Add(suc);
                }

                if (Sucursales.Count > 0)
                {
                    // Asignar la sucursal disparará automáticamente OnSucursalSeleccionadaChanged
                    SucursalSeleccionada = Sucursales[0];
                }
                else
                {
                    await Application.Current!.MainPage!.DisplayAlert("Aviso", "La API respondió bien, pero la lista de sucursales vino vacía.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert("Error en Sucursales", $"No se pudieron obtener las sucursales: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Se ejecuta automáticamente cuando el usuario cambia la selección en el Picker
        partial void OnSucursalSeleccionadaChanged(BranchDto? value)
        {
            if (value != null && !string.IsNullOrEmpty(value.BranchCode))
            {
                _ = CargarProductosAsync(value.BranchCode);
            }
        }

        [RelayCommand]
        public async Task CargarProductosAsync(string branchCode)
        {
            if (string.IsNullOrEmpty(branchCode)) return;

            IsBusy = true;
            Productos.Clear();

            try
            {
                var productosDb = await _productoService.ObtenerProductosAsync(branchCode);
                foreach (var prod in productosDb)
                {
                    Productos.Add(prod);
                }
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert("Error en Productos", $"No se pudieron cargar los productos: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}