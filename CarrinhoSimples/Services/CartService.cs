using System.Collections.Generic;
using CarrinhoSimples.Domain;
using CarrinhoSimples.Models;

namespace CarrinhoSimples.Services
{
    public class CartService : ICartService
    {
        private readonly Cart _cart = new();

        public void AddProduct(Product product, int quantity = 1) => _cart.AddProduct(product, quantity);
        public void UpdateQuantity(int productId, int quantity) => _cart.UpdateQuantity(productId, quantity);
        public void Remove(int productId) => _cart.RemoveProduct(productId);
        public IReadOnlyCollection<CartItem> GetItems() => _cart.Items;
        public decimal GetTotal() => _cart.Total();
        public void Clear() => _cart.Clear();
    }
}
