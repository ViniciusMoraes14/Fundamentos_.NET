using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Linq;
using CarrinhoSimples.Models;

namespace CarrinhoSimples.Domain
{
    public class Cart
    {
        private readonly List<CartItem> _items = new();

        public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

        public void AddProduct(Product product, int quantity = 1)
        {
            if (quantity <= 0) return;
            var existing = _items.FirstOrDefault(i => i.Product.Id == product.Id);
            if (existing == null) _items.Add(new CartItem(product, quantity));
            else existing.Quantity += quantity;
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var item = _items.FirstOrDefault(i => i.Product.Id == productId);
            if (item == null) return;
            if (quantity <= 0) _items.Remove(item);
            else item.Quantity = quantity;
        }

        public void RemoveProduct(int productId)
        {
            _items.RemoveAll(i => i.Product.Id == productId);
        }

        public decimal Total() => _items.Sum(i => i.SubTotal);

        public void Clear() => _items.Clear();
    }
}
