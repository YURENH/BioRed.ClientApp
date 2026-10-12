using BioRed.ClientApp.Views;
using BioRed.Services.Auth;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BioRed.ClientApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthService _authService;
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private bool _isBusy;

        public LoginViewModel(AuthService authService, IServiceProvider serviceProvider)
        {
            _authService = authService;
            _serviceProvider = serviceProvider;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current!.MainPage!.DisplayAlert("Atención", "Por favor ingresa tu correo y contraseña.", "OK");
                return;
            }

            IsBusy = true;

            try
            {
                bool exito = await _authService.LoginAsync(Email, Password);

                if (exito)
                {
                    Application.Current!.MainPage = new AppShell();
                }
                else // 👈 Aquí se agregó el 'else' faltante
                {
                    await Application.Current!.MainPage!.DisplayAlert("Error", "Correo o contraseña incorrectos.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert("Error de Conexión", $"No pudimos conectar con el servidor: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task IrARecuperarContrasenaAsync()
        {
            try
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync(nameof(RecuperarContrasenaPage));
                }
                else if (Application.Current?.MainPage != null)
                {
                    var services = IPlatformApplication.Current?.Services;
                    var paginaRecuperacion = services?.GetService<RecuperarContrasenaPage>();

                    if (paginaRecuperacion != null)
                    {
                        // 🟢 PushModalAsync funciona sobre cualquier MainPage sin requerir NavigationPage
                        await Application.Current.MainPage.Navigation.PushModalAsync(paginaRecuperacion);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NAVEGACION ERROR]: {ex.Message}");
            }
        }
    }
}