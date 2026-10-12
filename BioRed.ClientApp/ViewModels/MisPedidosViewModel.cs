using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BioRed.ClientApp.Views;
using BioRed.Core.Dtos;
using BioRed.Services.Pedidos.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace BioRed.ClientApp.ViewModels
{
    public partial class MisPedidosViewModel : ObservableObject
    {
        private readonly IPedidoService _pedidoService;

        [ObservableProperty]
        private ObservableCollection<OrderDetailsDto> _pedidos = new();

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private bool _isBusy;

        public MisPedidosViewModel(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [RelayCommand]
        public async Task CargarPedidosAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                var pedidos = await _pedidoService.ObtenerMisPedidosAsync();

                Pedidos.Clear();
                if (pedidos != null)
                {
                    foreach (var item in pedidos)
                    {
                        Pedidos.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR CARGAR PEDIDOS]: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        private async Task RastrearPedidoAsync(OrderDetailsDto pedido)
        {
            if (pedido == null) return;

            await Shell.Current.GoToAsync($"{nameof(SeguimientoPedidoPage)}?OrderId={pedido.OrderId}");
        }

        [RelayCommand]
        private async Task CancelarPedidoAsync(OrderDetailsDto pedido)
        {
            if (pedido == null) return;

            bool confirmar = await Shell.Current.DisplayAlert(
                "Cancelar Pedido",
                $"¿Estás seguro de que deseas cancelar el pedido #{pedido.OrderId}?",
                "Sí, cancelar",
                "No");

            if (!confirmar) return;

            try
            {
                IsBusy = true;

                // Enviamos tanto el OrderId como la razón/motivo exigido por la interfaz
                bool exito = await _pedidoService.CancelarPedidoAsync(pedido.OrderId, "Cancelado por el cliente");

                if (exito)
                {
                    await Shell.Current.DisplayAlert("Éxito", "El pedido ha sido cancelado correctamente.", "OK");
                    await CargarPedidosAsync();
                }
                else
                {
                    await Shell.Current.DisplayAlert(
                        "Error al cancelar",
                        "No se pudo procesar la cancelación. Revisa el Output/Salida de Visual Studio para ver la respuesta del servidor.",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR CANCELAR PEDIDO]: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"Excepción: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}