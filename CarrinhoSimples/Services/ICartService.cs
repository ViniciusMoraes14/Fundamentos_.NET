using System.Collections.Generic;
using CarrinhoSimples.Models;

namespace CarrinhoSimples.Services
{
    public interface ICartService
    {
        void AddProduct(Product product, int quantity = 1);
        void UpdateQuantity(int productId, int quantity);
        void Remove(int productId);
        IReadOnlyCollection<CartItem> GetItems();
        decimal GetTotal();
        void Clear();
    }
}
