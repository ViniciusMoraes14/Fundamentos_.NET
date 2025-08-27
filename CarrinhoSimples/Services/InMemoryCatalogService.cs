using System.Collections.Generic;
using System.Linq;
using CarrinhoSimples.Models;

namespace CarrinhoSimples.Services
{
    public class InMemoryCatalogService : ICatalogService
    {
        private readonly List<Product> _products = new()
        {
            new Product(1, "Notebook", 3500m),
            new Product(2, "Mouse Gamer", 150m),
            new Product(3, "Teclado Mecânico", 300m),
            new Product(4, "Monitor 24\"", 800m)
        };

        public IReadOnlyList<Product> GetProducts() => _products.AsReadOnly();
        public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);
    }
}
