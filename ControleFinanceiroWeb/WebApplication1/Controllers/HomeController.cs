using Microsoft.AspNetCore.Mvc;
using ControleFinanceiroWeb.Models;
using System.Linq;

namespace ControleFinanceiroWeb.Controllers
{
    public class HomeController : Controller
    {
        private static List<Transacao> _transacoes = new List<Transacao>();

        public IActionResult Index()
        {
            // Calcula o saldo
            var entradas = _transacoes.Where(t => t.Tipo == "Entrada").Sum(t => t.Valor);
            var saidas = _transacoes.Where(t => t.Tipo == "Saída").Sum(t => t.Valor);
            ViewBag.Saldo = entradas - saidas;

            return View(_transacoes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Limpar()
        {
            _transacoes.Clear();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Adicionar(string descricao, decimal valor, string tipo, string categoria)
        {
            var nova = new Transacao
            {
                Id = _transacoes.Any() ? _transacoes.Max(t => t.Id) + 1 : 1,
                Descricao = descricao,
                Valor = valor,
                Tipo = tipo,
                Categoria = categoria
            };

            _transacoes.Add(nova);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult PorCategoria()
        {
            var categorias = _transacoes
                .GroupBy(t => t.Categoria)
                .Select(g => new CategoriaViewModel
                {
                    Categoria = g.Key,
                    Total = g.Sum(t => t.Tipo == "Entrada" ? t.Valor : -t.Valor)
                })
                .ToList();

            return View(categorias);
        }

        public IActionResult TransacoesPor(string categoria)
        {
            var lista = _transacoes
                .Where(t => t.Categoria == categoria)
                .ToList();

            ViewBag.Categoria = categoria;
            return View(lista);
        }
    }
}
