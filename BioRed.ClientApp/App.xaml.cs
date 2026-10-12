using BioRed.ClientApp.Views;

namespace BioRed.ClientApp;

public partial class App : Application
{
    // Pedimos la página de login por Inyección de Dependencias
    public App(LoginPage loginPage)
    {
        InitializeComponent();

        // Le decimos a la App que la primera pantalla sea el Login
        MainPage = loginPage;
    }
}