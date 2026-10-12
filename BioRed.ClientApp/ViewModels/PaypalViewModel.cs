using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using BioRed.Core.DTOs;
using BioRed.Services.Pagos.Interfaces;

namespace BioRed.ClientApp.ViewModels
{
    public partial class PaypalViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IPagoService _pagoService;

        [ObservableProperty] private string? _approvalUrl;
        [ObservableProperty] private string? _providerOrderId;
        [ObservableProperty] private bool _isLoading;

        public PaypalViewModel(IPagoService pagoService)
        {
            _pagoService = pagoService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("approvalUrl", out var url))
                ApprovalUrl = url?.ToString();

            if (query.TryGetValue("providerOrderId", out var pOrderId))
                ProviderOrderId = pOrderId?.ToString();
        }

        public async Task EvaluarNavegacionAsync(string url)
        {
            // Intercepta cuando PayPal redirige tras la aprobación del cliente
            if (url.Contains("token=") || url.Contains("approved") || url.Contains("success"))
            {
                if (!string.IsNullOrEmpty(ProviderOrderId))
                {
                    await ProcesarCapturaPagoAsync();
                }
            }
            else if (url.Contains("cancel"))
            {
                await Shell.Current.DisplayAlert("Pago Cancelado", "Has cancelado el proceso de pago.", "OK");
                await Shell.Current.Navigation.PopModalAsync();
            }
        }

        public async Task ProcesarCapturaPagoAsync()
        {
            if (string.IsNullOrEmpty(ProviderOrderId) || IsLoading) return;

            try
            {
                IsLoading = true;
                var resultado = await _pagoService.CapturarPagoPaypalAsync(ProviderOrderId);

                // Comprobamos si el pago se completó
                bool exito = resultado != null && (resultado.IsPaid ||
                             resultado.Payments.Any(p => p.Status.Equals("COMPLETED", StringComparison.OrdinalIgnoreCase)
                                                      || p.Status.Equals("APPROVED", StringComparison.OrdinalIgnoreCase)));

                if (exito)
                {
                    await Shell.Current.DisplayAlert("¡Pago Exitoso!", "Tu pago por PayPal ha sido verificado correctamente.", "OK");
                    await Shell.Current.Navigation.PopModalAsync();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "No se pudo verificar el pago con PayPal.", "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[PAYPAL ERROR]: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Ocurrió un error al procesar la transacción.", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}