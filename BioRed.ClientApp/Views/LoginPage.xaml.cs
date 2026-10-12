using BioRed.ClientApp.ViewModels;

namespace BioRed.ClientApp.Views;

public partial class LoginPage : ContentPage
{
    // Inyectamos el LoginViewModel aquí
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();

        // Esta línea es la que "conecta" la vista gráfica con las funciones del botón
        BindingContext = viewModel;
    }
}