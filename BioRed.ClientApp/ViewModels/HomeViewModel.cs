using System;
using System.Collections.ObjectModel; // 👈 Corregido
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;

using BioRed.Core.DTOs;
using BioRed.Services.Direcciones;
using BioRed.ClientApp.Views;

namespace BioRed.ClientApp.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly DireccionService _direccionService;
        private readonly IServiceProvider _serviceProvider;

        private string _direccionActiva = "Cargando dirección...";
        public string DireccionActiva
        {
            get => _direccionActiva;
            set => SetProperty(ref _direccionActiva, value);
        }

        private ObservableCollection<QuickActionDto> _accionesRapidas = new();
        public ObservableCollection<QuickActionDto> AccionesRapidas
        {
            get => _accionesRapidas;
            set => SetProperty(ref _accionesRapidas, value);
        }

        private string _textoBusqueda = string.Empty;
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set => SetProperty(ref _textoBusqueda, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public HomeViewModel(DireccionService direccionService, IServiceProvider serviceProvider)
        {
            _direccionService = direccionService;
            _serviceProvider = serviceProvider;
            CargarAccionesRapidas();
        }

        public async Task CargarDatosInicialesAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                await CargarDireccionUsuarioAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task CargarDireccionUsuarioAsync()
        {
            try
            {
                var lista = await _direccionService.ObtenerDireccionesClienteAsync();
                if (lista != null && lista.Count > 0)
                {
                    var direccionDefecto = lista.FirstOrDefault(d => d.IsDefault) ?? lista.FirstOrDefault();
                    if (direccionDefecto != null)
                    {
                        DireccionActiva = $"{direccionDefecto.Name} - {direccionDefecto.FullAddress}";
                    }
                }
                else
                {
                    DireccionActiva = "Seleccionar dirección de entrega";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar dirección en Home: {ex.Message}");
                DireccionActiva = "Seleccionar ubicación";
            }
        }

        private void CargarAccionesRapidas()
        {
            AccionesRapidas = new ObservableCollection<QuickActionDto>
            {
                new QuickActionDto { Id = 1, Title = "Catálogo / Sucursales", Icon = "🏪", ActionCode = "SUCURSALES", BackgroundColor = "#E3F2FD" },
                new QuickActionDto { Id = 2, Title = "Mi Carrito", Icon = "🛒", ActionCode = "CARRITO", BackgroundColor = "#E8F5E9" },
                new QuickActionDto { Id = 3, Title = "Mis Pedidos", Icon = "📦", ActionCode = "PEDIDOS", BackgroundColor = "#FFF3E0" },
                new QuickActionDto { Id = 4, Title = "Ofertas Especiales", Icon = "🏷️", ActionCode = "OFERTAS", BackgroundColor = "#FFEBEE" }
            };
        }

        [RelayCommand]
        private async Task EjecutarAccionAsync(QuickActionDto accion)
        {
            if (accion == null) return;

            switch (accion.ActionCode)
            {
                case "SUCURSALES":
                    var vistaProductos = _serviceProvider.GetRequiredService<ProductosPage>();

                    if (Application.Current?.MainPage?.Navigation != null)
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(vistaProductos);
                    }
                    break;

                case "CARRITO":
                    var vistaCarrito = _serviceProvider.GetRequiredService<CarritoPage>();
                    if (Application.Current?.MainPage?.Navigation != null)
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(vistaCarrito);
                    }
                    break;

                case "PEDIDOS":
                    try
                    {
                        var vistaSeguimiento = _serviceProvider.GetService<SeguimientoPedidoPage>();
                        if (vistaSeguimiento != null && Application.Current?.MainPage?.Navigation != null)
                        {
                            await Application.Current.MainPage.Navigation.PushAsync(vistaSeguimiento);
                        }
                    }
                    catch (Exception ex)
                    {
                        await Application.Current!.MainPage!.DisplayAlert("Error", $"No se pudo abrir el seguimiento: {ex.Message}", "OK");
                    }
                    break;
            }
        }
        }
    }