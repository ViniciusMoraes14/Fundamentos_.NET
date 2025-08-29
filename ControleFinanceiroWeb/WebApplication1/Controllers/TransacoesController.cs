using Microsoft.AspNetCore.Mvc;
using ControleFinanceiroWeb.Models;

namespace ControleFinanceiroWeb.Controllers
{
    public class TransacoesController : Controller
    {
        private static List<Transacao> transacoes = new List<Transacao>();

        public IActionResult Index()
        {
            return View(transacoes);
        }

        [HttpPost]
        public IActionResult Adicionar(string descricao, decimal valor, string tipo)
        {
            transacoes.Add(new Transacao
            {
                Descricao = descricao,
                Valor = valor,
                Tipo = tipo
            });

            return RedirectToAction("Index");
        }
    }
}
