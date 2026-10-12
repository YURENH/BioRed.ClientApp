using System;
using System.Threading;
using System.Threading.Tasks;
using BioRed.Core.Dtos;
using BioRed.Services.Pedidos.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Controls;

namespace BioRed.ClientApp.ViewModels
{
    [QueryProperty(nameof(OrderId), "OrderId")]
    public partial class SeguimientoPedidoViewModel : ObservableObject
    {
        private readonly IPedidoService _pedidoService;
        private PeriodicTimer? _timer;
        private CancellationTokenSource? _cts;

        public event EventHandler? DatosActualizados;

        [ObservableProperty] private int _orderId;
        [ObservableProperty] private OrderDetailsDto? _pedido;
        [ObservableProperty] private string _mensajeEstado = "Cargando estado...";
        [ObservableProperty] private string _colorEstado = "#007A33";
        [ObservableProperty] private string? _nombreRepartidor;

        [ObservableProperty] private bool _paso1Completado;
        [ObservableProperty] private bool _paso2Completado;
        [ObservableProperty] private bool _paso3Completado;
        [ObservableProperty] private bool _paso4Completado;

        public bool PuedeCancelar => Pedido?.Status?.Equals("PENDIENTE", StringComparison.OrdinalIgnoreCase) == true;

        public SeguimientoPedidoViewModel(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        partial void OnOrderIdChanged(int value)
        {
            // Dejar vacío para no interferir con la navegación de Shell
        }

        public async Task CargarYMonitorearPedidoAsync(int orderId)
        {
            if (orderId <= 0) return;

            OrderId = orderId;
            DetenerMonitoreo();

            _cts = new CancellationTokenSource();

            // Primera carga
            await ConsultarEstadoAsync(orderId);

            // Bucle de sondeo cada 7 segundos
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(7));
            try
            {
                while (await _timer.WaitForNextTickAsync(_cts.Token))
                {
                    await ConsultarEstadoAsync(orderId);
                }
            }
            catch (OperationCanceledException) { /* Cancelación normal */ }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[POLLING ERROR]: {ex.Message}");
            }
        }

        public void DetenerMonitoreo()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;

            _timer?.Dispose();
            _timer = null;
        }

        public async Task ConsultarEstadoAsync(int orderId)
        {
            try
            {
                var data = await _pedidoService.ObtenerSeguimientoAsync(orderId);
                if (data == null) return;

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Pedido = data;
                    NombreRepartidor = string.IsNullOrWhiteSpace(data.DriverCode) ? "Pendiente" : data.DriverCode;

                    ProcesarEtapaDelPedido(data);

                    OnPropertyChanged(nameof(PuedeCancelar));
                    DatosActualizados?.Invoke(this, EventArgs.Empty);
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CONSULTAR ESTADO ERROR]: {ex.Message}");
            }
        }

        private void ProcesarEtapaDelPedido(OrderDetailsDto data)
        {
            string estadoNormalizado = data.Status?.ToUpper() ?? "";

            Paso1Completado = true;
            Paso2Completado = estadoNormalizado.Contains("PREPAR") || estadoNormalizado.Contains("CAMINO") || estadoNormalizado.Contains("ENTREGADO");
            Paso3Completado = estadoNormalizado.Contains("CAMINO") || estadoNormalizado.Contains("ENTREGADO");
            Paso4Completado = estadoNormalizado.Contains("ENTREGADO");

            switch (estadoNormalizado)
            {
                case "PENDIENTE":
                    MensajeEstado = "Esperando confirmación en sucursal";
                    ColorEstado = "#E67E22";
                    break;

                case "EN_PREPARACION":
                case "EN PREPARACION":
                    MensajeEstado = "Empaquetando tus medicamentos";
                    ColorEstado = "#3498DB";
                    break;

                case "EN_CAMINO":
                case "EN CAMINO":
                    MensajeEstado = "¡Tu pedido va en camino!";
                    ColorEstado = "#2ECC71";
                    break;

                case "ENTREGADO":
                    MensajeEstado = "Pedido entregado con éxito";
                    ColorEstado = "#007A33";
                    DetenerMonitoreo();
                    break;

                default:
                    MensajeEstado = $"Estado: {data.Status}";
                    ColorEstado = "#7F8C8D";
                    break;
            }
        }

        [RelayCommand]
        private async Task LlamarRepartidorAsync()
        {
            if (!string.IsNullOrEmpty(Pedido?.DriverCode) && PhoneDialer.Default.IsSupported)
            {
                PhoneDialer.Default.Open(Pedido.DriverCode);
            }
            else
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.DisplayAlert("Info", "El repartidor aún no ha sido asignado.", "OK");
                }
            }
        }

        [RelayCommand]
        private async Task CancelarPedidoAsync()
        {
            if (Pedido == null) return;

            bool confirmar = await Shell.Current.DisplayAlert(
                "Cancelar Pedido",
                $"¿Estás seguro de cancelar el pedido #{Pedido.OrderCode}?",
                "Sí, Cancelar",
                "No");

            if (!confirmar) return;

            bool exito = await _pedidoService.CancelarPedidoAsync(Pedido.OrderId);

            if (exito)
            {
                await Shell.Current.DisplayAlert("Éxito", "Tu pedido ha sido cancelado.", "OK");
                DetenerMonitoreo();
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "No se pudo cancelar el pedido.", "OK");
            }
        }
    }
}