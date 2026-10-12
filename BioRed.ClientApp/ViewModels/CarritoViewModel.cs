using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BioRed.ClientApp.Views;
using BioRed.Core.Dtos;
using BioRed.Core.DTOs;
using BioRed.Services.Cart;
using BioRed.Services.Direcciones;
using BioRed.Services.Pagos;
using BioRed.Services.Pedidos;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace BioRed.ClientApp.ViewModels
{
    public partial class CarritoViewModel : ObservableObject
    {
        private readonly PedidoService _pedidoService;
        private readonly CartService _cartService;
        private readonly DireccionService _direccionService;
        private readonly PagoService _pagoService;

        // 🟢 Carrito y Productos
        private ObservableCollection<CartItemDto> _items = new();
        public ObservableCollection<CartItemDto> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        // 🟢 Direcciones
        private ObservableCollection<DeliveryAddressDto> _direcciones = new();
        public ObservableCollection<DeliveryAddressDto> Direcciones
        {
            get => _direcciones;
            set => SetProperty(ref _direcciones, value);
        }

        private DeliveryAddressDto? _direccionSeleccionada;
        public DeliveryAddressDto? DireccionSeleccionada
        {
            get => _direccionSeleccionada;
            set => SetProperty(ref _direccionSeleccionada, value);
        }

        // 🟢 MÉTODOS DE PAGO
        private ObservableCollection<PaymentTypeDto> _metodosPago = new();
        public ObservableCollection<PaymentTypeDto> MetodosPago
        {
            get => _metodosPago;
            set => SetProperty(ref _metodosPago, value);
        }

        private PaymentTypeDto? _metodoPagoSeleccionado;
        public PaymentTypeDto? MetodoPagoSeleccionado
        {
            get => _metodoPagoSeleccionado;
            set => SetProperty(ref _metodoPagoSeleccionado, value);
        }

        // 🟢 Totales y Estado
        private decimal _total;
        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public CarritoViewModel(
            PedidoService pedidoService,
            CartService cartService,
            DireccionService direccionService,
            PagoService pagoService)
        {
            _pedidoService = pedidoService;
            _cartService = cartService;
            _direccionService = direccionService;
            _pagoService = pagoService;

            CargarMetodosPago();
            CargarCarrito();
        }

        public async Task CargarDatosInicialesAsync()
        {
            CargarMetodosPago();
            CargarCarrito();
            await CargarDireccionesAsync();
        }

        public void CargarMetodosPago()
        {
            MetodosPago = new ObservableCollection<PaymentTypeDto>
            {
                new PaymentTypeDto { Code = "PAGO-EFECTIVO", Name = "Pago en Efectivo", Description = "Pago contra entrega" },
                new PaymentTypeDto { Code = "PAGO-PAYPAL", Name = "PayPal", Description = "Pago seguro en línea" }
            };

            if (MetodoPagoSeleccionado == null)
            {
                MetodoPagoSeleccionado = MetodosPago.FirstOrDefault();
            }
        }

        public void CargarCarrito()
        {
            if (_cartService.Items != null)
            {
                Items = new ObservableCollection<CartItemDto>(_cartService.Items);
            }
            ActualizarTotal();
        }

        public async Task CargarDireccionesAsync()
        {
            try
            {
                var lista = await _direccionService.ObtenerDireccionesClienteAsync();
                if (lista != null && lista.Count > 0)
                {
                    Direcciones = new ObservableCollection<DeliveryAddressDto>(lista);
                    if (DireccionSeleccionada == null)
                    {
                        DireccionSeleccionada = Direcciones.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar direcciones: {ex.Message}");
            }
        }

        private void ActualizarTotal()
        {
            if (Items != null && Items.Count > 0)
            {
                Total = Items.Sum(i => i.Quantity * (i.Producto?.SalePrice ?? 0));
            }
            else
            {
                Total = 0;
            }
        }

        [RelayCommand]
        private async Task ProcesarPedidoAsync()
        {
            if (IsBusy) return;

            if (Items == null || Items.Count == 0)
            {
                await Application.Current!.MainPage!.DisplayAlert("Carrito Vacío", "Agrega productos antes de realizar el pedido.", "OK");
                return;
            }

            if (DireccionSeleccionada == null)
            {
                await Application.Current!.MainPage!.DisplayAlert("Dirección Requerida", "Por favor selecciona una dirección de entrega antes de realizar el pedido.", "OK");
                return;
            }

            if (MetodoPagoSeleccionado == null)
            {
                await Application.Current!.MainPage!.DisplayAlert("Método de Pago", "Por favor selecciona un método de pago.", "OK");
                return;
            }

            IsBusy = true;

            try
            {
                var primerItem = Items.First();

                string branchCode = !string.IsNullOrEmpty(primerItem.BranchCode)
                    ? primerItem.BranchCode
                    : "SUC-ADE-01";

                decimal subtotalPedido = Items.Sum(i => i.Quantity * (i.Producto?.SalePrice ?? 0));
                decimal costoEnvio = 0;
                decimal totalCargado = subtotalPedido + costoEnvio;

                // 🟢 Mapeo inteligente del ID según tu BD (1 = PAGO-EFECTIVO, 2 = PAGO-PAYPAL)
                int paymentTypeId = (MetodoPagoSeleccionado.Code == "PAGO-PAYPAL") ? 2 : 1;

                var pedidoDto = new CreateOrderDto
                {
                    BranchCode = branchCode,
                    DeliveryAddressId = DireccionSeleccionada.AddressId,
                    PaymentTypeCode = MetodoPagoSeleccionado.Code,
                    PaymentTypeId = paymentTypeId,
                    Subtotal = subtotalPedido,
                    DeliveryCost = costoEnvio,
                    TotalCharged = totalCargado,
                    Items = Items.Select(i => new CreateOrderItemDto
                    {
                        ProductCode = i.Producto?.ProductCode ?? string.Empty,
                        ProductName = i.Producto?.ProductName ?? string.Empty,
                        Quantity = i.Quantity,
                        UnitPrice = i.Producto?.SalePrice ?? 0,
                        Subtotal = i.Quantity * (i.Producto?.SalePrice ?? 0)
                    }).ToList()
                };

                // 1. Se crea la orden en Azure
                
                int orderIdReal = await _pedidoService.CrearPedidoYObtenerIdAsync(pedidoDto);

                if (orderIdReal > 0)
                {
                    // 2. Si es PayPal, solicitamos la URL y abrimos el navegador
                    if (MetodoPagoSeleccionado.Code == "PAGO-PAYPAL")
                    {
                        var paypalInfo = await _pagoService.IniciarPagoPaypalAsync(orderIdReal);

                        if (paypalInfo != null && !string.IsNullOrEmpty(paypalInfo.ApprovalUrl))
                        {
                            await Browser.Default.OpenAsync(new Uri(paypalInfo.ApprovalUrl), BrowserLaunchMode.SystemPreferred);
                            await Application.Current!.MainPage!.DisplayAlert("PayPal", "Completa tu pago en la ventana de PayPal.", "OK");
                        }
                        else
                        {
                            await Application.Current!.MainPage!.DisplayAlert("Atención", "El pedido se creó en Azure, pero no se pudo generar la pasarela de PayPal.", "OK");
                        }
                    }

                    // 3. Limpiar carrito local tras éxito
                    _cartService.LimpiarCarrito();
                    CargarCarrito();

                    // 4. Redirección dinámica hacia Seguimiento con el OrderId real generado
                    await Shell.Current.GoToAsync($"{nameof(SeguimientoPedidoPage)}?OrderId={orderIdReal}");
                }
                else
                {
                    await Application.Current!.MainPage!.DisplayAlert("Error", "No se pudo registrar el pedido en el servidor.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert("Error al procesar", $"No se pudo realizar el pedido: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}