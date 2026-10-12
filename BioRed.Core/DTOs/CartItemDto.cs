using System;
using System.Collections.Generic;
using System.Text;

namespace BioRed.Core.DTOs
{
    public class CartItemDto
    {
        public ProductoDto Producto { get; set; } = new();
        public string BranchCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Subtotal => Producto.SalePrice * Quantity;
    }
}
