using BioRed.ClientApp.ViewModels;

namespace BioRed.ClientApp.Views
{
    public partial class RecuperarContrasenaPage : ContentPage
    {
        public RecuperarContrasenaPage(RecuperarContrasenaViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}