using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarrinhoSimples.Models
{
    public class CartItem
    {
        public Product Product { get; }
        public int Quantity { get; set; }
        public decimal SubTotal => Product.Price * Quantity;

        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public override string ToString() => $"{Product.Name} x{Quantity} = {SubTotal:C}";
    }
}
