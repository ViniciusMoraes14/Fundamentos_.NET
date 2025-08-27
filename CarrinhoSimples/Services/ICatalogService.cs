using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Collections.Generic;
using CarrinhoSimples.Models;

namespace CarrinhoSimples.Services
{
    public interface ICatalogService
    {
        IReadOnlyList<Product> GetProducts();
        Product? GetById(int id);
    }
}
