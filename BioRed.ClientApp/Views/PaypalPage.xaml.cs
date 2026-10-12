using BioRed.ClientApp.ViewModels;

namespace BioRed.ClientApp.Views
{
    public partial class PaypalPage : ContentPage
    {
        public PaypalPage()
        {
            InitializeComponent();
        }

        private async void OnWebViewNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (BindingContext is PaypalViewModel vm)
            {
                await vm.EvaluarNavegacionAsync(e.Url);
            }
        }
    }
}