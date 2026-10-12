using System;
using System.Threading.Tasks;
using BioRed.Services.Auth;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace BioRed.ClientApp.ViewModels
{
    public partial class RecuperarContrasenaViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        [ObservableProperty] private string _email = string.Empty;
        [ObservableProperty] private string _claveTemporal = string.Empty;
        [ObservableProperty] private string _nuevaContrasena = string.Empty;
        [ObservableProperty] private bool _isLoading;
        [ObservableProperty] private bool _esPaso1 = true;
        [ObservableProperty] private bool _esPaso2 = false;

        public RecuperarContrasenaViewModel(AuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task EnviarCorreoRecuperacionAsync()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                await MostrarAlertaAsync("Atención", "Por favor ingresa tu correo electrónico.", "OK");
                return;
            }

            try
            {
                IsLoading = true;
                bool exito = await _authService.SolicitarRecuperacionAsync(Email.Trim());

                if (exito)
                {
                    await MostrarAlertaAsync("Correo Enviado", "Hemos enviado la clave temporal a tu correo electrónico.", "OK");
                    EsPaso1 = false;
                    EsPaso2 = true;
                }
                else
                {
                    await MostrarAlertaAsync("Error", "No se pudo procesar la solicitud. Verifica que el correo esté registrado.", "OK");
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task ConfirmarCambioContrasenaAsync()
        {
            if (string.IsNullOrWhiteSpace(ClaveTemporal))
            {
                await MostrarAlertaAsync("Atención", "Ingresa la clave temporal recibida.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(NuevaContrasena))
            {
                await MostrarAlertaAsync("Atención", "Ingresa tu nueva contraseña.", "OK");
                return;
            }

            try
            {
                IsLoading = true;
                bool exito = await _authService.CambiarContrasenaTemporalAsync(Email.Trim(), ClaveTemporal.Trim(), NuevaContrasena.Trim());

                if (exito)
                {
                    await MostrarAlertaAsync("¡Éxito!", "Tu contraseña ha sido actualizada. Ya puedes iniciar sesión con tu nueva contraseña.", "OK");
                    await CerrarPantallaAsync();
                }
                else
                {
                    await MostrarAlertaAsync("Error", "No se pudo actualizar la contraseña. Revisa que la clave temporal sea correcta y que la nueva contraseña cumpla los requisitos.", "OK");
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task VolverALoginAsync()
        {
            await CerrarPantallaAsync();
        }

        private async Task CerrarPantallaAsync()
        {
            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync("..");
            }
            else if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        private async Task MostrarAlertaAsync(string titulo, string mensaje, string boton)
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert(titulo, mensaje, boton);
            }
        }
    }
}