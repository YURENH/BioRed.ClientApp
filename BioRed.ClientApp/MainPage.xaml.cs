using BioRed.Services.Api;

namespace BioRed.ClientApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        try
        {
            var apiClient = new ApiClient();

            // Reemplaza "api/v1/productos" por un endpoint GET que exista en Scalar (ej. productos, categorias, status)
            var resultado = await apiClient.GetAsync<object>("api/v1/productos");

            await DisplayAlert("¡Conexión Exitosa! 🚀", "La app se conectó a Azure correctamente.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error de Conexión ❌", ex.Message, "OK");
        }
    }
}