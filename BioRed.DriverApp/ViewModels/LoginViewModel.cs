
using BioRed.Services.Auth;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BioRed.DriverApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private bool _isBusy;

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            // 1. Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current!.MainPage!.DisplayAlert("Atención", "Por favor ingresa tu correo y contraseña.", "OK");
                return;
            }

            // 2. Mostrar la ruedita de carga
            IsBusy = true;

            try
            {
                // 3. Enviar datos a la API de Azure
                bool exito = await _authService.LoginAsync(Email, Password);

                if (exito)
                {
                    // 4. ¡ÉXITO!
                    await Application.Current!.MainPage!.DisplayAlert("¡Bienvenido!", "Inicio de sesión correcto. 🎉", "OK");
                }
                else
                {
                    // Error de credenciales
                    await Application.Current!.MainPage!.DisplayAlert("Error", "Correo o contraseña incorrectos.", "OK");
                }
            }
            catch (Exception ex)
            {
                // Error de servidor o de internet
                await Application.Current!.MainPage!.DisplayAlert("Error de Conexión", $"No pudimos conectar con el servidor: {ex.Message}", "OK");
            }
            finally
            {
                // 5. Ocultar la ruedita de carga
                IsBusy = false;
            }
        }
    }
}