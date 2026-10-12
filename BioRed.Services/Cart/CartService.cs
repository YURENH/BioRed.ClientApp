using BioRed.Core.DTOs;
using System.Collections.ObjectModel;

namespace BioRed.Services.Cart
{
    public class CartService
    {
        public ObservableCollection<CartItemDto> Items { get; } = new();

        public void AddToCart(ProductoDto producto, string branchCode, int cantidad)
        {
            var existente = Items.FirstOrDefault(i => i.Producto.ProductCode == producto.ProductCode && i.BranchCode == branchCode);
            if (existente != null)
            {
                existente.Quantity += cantidad;
            }
            else
            {
                Items.Add(new CartItemDto
                {
                    Producto = producto,
                    BranchCode = branchCode,
                    Quantity = cantidad
                });
            }
        }

        public decimal ObtenerTotal() => Items.Sum(i => i.Subtotal);
        public int ObtenerCantidadTotal() => Items.Sum(i => i.Quantity);
        public void LimpiarCarrito() => Items.Clear();
    }
}