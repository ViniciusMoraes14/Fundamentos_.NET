// See https://aka.ms/new-console-template for more information
using System;
using System.Globalization;
using CarrinhoSimples.Services;
using CarrinhoSimples.Models;

namespace CarrinhoSimples
{
    class Program
    {
        static ICatalogService catalog = new InMemoryCatalogService();
        static ICartService cart = new CartService();

        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");

            int option;
            do
            {
                ShowMenu();
                var input = Console.ReadLine();
                if (!int.TryParse(input, out option))
                {
                    Console.WriteLine("Opção inválida.");
                    continue;
                }

                switch (option)
                {
                    case 1: ListProducts(); break;
                    case 2: AddToCart(); break;
                    case 3: ShowCart(); break;
                    case 4: UpdateQuantity(); break;
                    case 5: RemoveFromCart(); break;
                    case 6: Checkout(); break;
                    case 7: ClearCart(); break;
                    case 0: Console.WriteLine("Saindo..."); break;
                    default: Console.WriteLine("Opção inválida."); break;
                }
            } while (option != 0);
        }

        static void ShowMenu()
        {
            Console.WriteLine("\n=== LOJA OOP - Carrinho em memória ===");
            Console.WriteLine("1 - Listar produtos");
            Console.WriteLine("2 - Adicionar produto ao carrinho");
            Console.WriteLine("3 - Ver carrinho");
            Console.WriteLine("4 - Atualizar quantidade");
            Console.WriteLine("5 - Remover item do carrinho");
            Console.WriteLine("6 - Finalizar compra (checkout)");
            Console.WriteLine("7 - Limpar carrinho");
            Console.WriteLine("0 - Sair");
            Console.Write("Escolha uma opção: ");
        }

        static void ListProducts()
        {
            Console.WriteLine("\nProdutos disponíveis:");
            foreach (var p in catalog.GetProducts())
                Console.WriteLine($"{p.Id} - {p.Name} - {p.Price:C}");
        }

        static void AddToCart()
        {
            Console.Write("Digite o ID do produto: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("ID inválido."); return; }
            var product = catalog.GetById(id);
            if (product == null) { Console.WriteLine("Produto não encontrado."); return; }
            Console.Write("Quantidade: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0) { Console.WriteLine("Quantidade inválida."); return; }
            cart.AddProduct(product, qty);
            Console.WriteLine($"{product.Name} x{qty} adicionado ao carrinho.");
        }

        static void ShowCart()
        {
            var items = cart.GetItems();
            Console.WriteLine("\n=== Seu Carrinho ===");
            if (items.Count == 0) { Console.WriteLine("Carrinho vazio."); return; }
            foreach (var item in items)
                Console.WriteLine($"{item.Product.Name} x{item.Quantity} = {item.SubTotal:C}");
            Console.WriteLine($"Total: {cart.GetTotal():C}");
        }

        static void UpdateQuantity()
        {
            Console.Write("Digite o ID do produto no carrinho: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("ID inválido."); return; }
            Console.Write("Nova quantidade: ");
            if (!int.TryParse(Console.ReadLine(), out int qty)) { Console.WriteLine("Quantidade inválida."); return; }
            cart.UpdateQuantity(id, qty);
            Console.WriteLine("Quantidade atualizada.");
        }

        static void RemoveFromCart()
        {
            Console.Write("Digite o ID do produto a remover: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("ID inválido."); return; }
            cart.Remove(id);
            Console.WriteLine("Item removido.");
        }

        static void Checkout()
        {
            var total = cart.GetTotal();
            if (total == 0) { Console.WriteLine("Carrinho vazio."); return; }
            Console.WriteLine($"Total da compra: {total:C}");
            Console.Write("Confirmar compra? (s/n): ");
            var resp = Console.ReadLine();
            if (resp?.Trim().ToLower() == "s")
            {
                Console.WriteLine("Compra finalizada. Obrigado!");
                cart.Clear();
            }
            else Console.WriteLine("Compra cancelada.");
        }

        static void ClearCart()
        {
            cart.Clear();
            Console.WriteLine("Carrinho limpo.");
        }
    }
}

