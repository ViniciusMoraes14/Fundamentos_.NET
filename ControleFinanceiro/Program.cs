using System;
using System.Globalization;
using System.IO;
using System.Linq;
using ControleFinanceiroApp.Models;
using ControleFinanceiroApp.Services;

namespace ControleFinanceiroApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new ControleFinanceiro("dados.json");

            while (true)
            {
                Console.WriteLine("\n=== Controle Financeiro ===");
                Console.WriteLine("1 - Adicionar Receita");
                Console.WriteLine("2 - Adicionar Despesa");
                Console.WriteLine("3 - Listar Transações");
                Console.WriteLine("4 - Mostrar Saldo");
                Console.WriteLine("5 - Filtrar por Mês/Ano");
                Console.WriteLine("6 - Resumo por Categoria (mês)");
                Console.WriteLine("7 - Exportar CSV");
                Console.WriteLine("0 - Sair");
                Console.Write("Escolha: ");

                if (!int.TryParse(Console.ReadLine(), out int opcao)) continue;

                switch (opcao)
                {
                    case 1: Add(manager, TipoTransacao.Receita); break;
                    case 2: Add(manager, TipoTransacao.Despesa); break;
                    case 3: ListAll(manager); break;
                    case 4: ShowBalance(manager); break;
                    case 5: FilterByMonth(manager); break;
                    case 6: SummaryByCategory(manager); break;
                    case 7: ExportCsv(manager); break;
                    case 0: return;
                }
            }
        }

        static void Add(ControleFinanceiro manager, TipoTransacao tipo)
        {
            Console.Write("Descrição: ");
            var desc = Console.ReadLine() ?? "";

            Console.Write("Categoria (ex: Alimentação, Transporte) [Geral]: ");
            var cat = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(cat)) cat = "Geral";

            decimal valor = ReadDecimal("Valor (use . ou ,): ");

            DateTime data = ReadDate("Data (dd/MM/yyyy) ou enter para hoje: ");

            var t = new Transacao
            {
                Data = data,
                Descricao = desc,
                Categoria = cat,
                Tipo = tipo,
                Valor = valor
            };

            manager.Add(t);
            Console.WriteLine($"{tipo} adicionada com sucesso!");
        }

        static decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var s = (Console.ReadLine() ?? "").Trim();
                if (string.IsNullOrEmpty(s)) { Console.WriteLine("Valor é obrigatório."); continue; }
                // aceita vírgula ou ponto
                s = s.Replace(',', '.');
                if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out var v)) return v;
                Console.WriteLine("Valor inválido. Use apenas números (ex: 1234,56 ou 1234.56).");
            }
        }

        static DateTime ReadDate(string prompt)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(s)) return DateTime.Now;
            if (DateTime.TryParseExact(s, new[] { "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                return dt;
            Console.WriteLine("Formato inválido; usando data atual.");
            return DateTime.Now;
        }

        static void ListAll(ControleFinanceiro manager)
        {
            var all = manager.GetAll().ToList();
            if (!all.Any()) { Console.WriteLine("Nenhuma transação registrada."); return; }
            foreach (var t in all) Console.WriteLine(t);
        }

        static void ShowBalance(ControleFinanceiro manager)
        {
            Console.Write("Filtrar por mês? (s/n): ");
            var r = (Console.ReadLine() ?? "").Trim().ToLower();
            if (r == "s" || r == "sim")
            {
                int m = ReadInt("Mês (1-12): ", 1, 12);
                int y = ReadInt("Ano: ", 1900, 3000);
                var bal = manager.GetBalance(m, y);
                Console.WriteLine($"Saldo {m}/{y}: R$ {bal:N2}");
            }
            else
            {
                var bal = manager.GetBalance();
                Console.WriteLine($"Saldo total: R$ {bal:N2}");
            }
        }

        static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int v) && v >= min && v <= max) return v;
                Console.WriteLine("Entrada inválida.");
            }
        }

        static void FilterByMonth(ControleFinanceiro manager)
        {
            int m = ReadInt("Mês (1-12): ", 1, 12);
            int y = ReadInt("Ano: ", 1900, 3000);
            var itens = manager.GetByMonth(m, y).ToList();
            if (!itens.Any()) { Console.WriteLine("Nenhuma transação encontrada."); return; }
            foreach (var it in itens) Console.WriteLine(it);
            Console.WriteLine($"Saldo: R$ {manager.GetBalance(m, y):N2}");
        }

        static void SummaryByCategory(ControleFinanceiro manager)
        {
            int m = ReadInt("Mês (1-12): ", 1, 12);
            int y = ReadInt("Ano: ", 1900, 3000);
            var dict = manager.GetSummaryByCategory(m, y);
            if (!dict.Any()) { Console.WriteLine("Sem despesas neste período."); return; }
            Console.WriteLine("Despesas por categoria:");
            foreach (var kv in dict.OrderByDescending(k => k.Value))
                Console.WriteLine($"{kv.Key}: R$ {kv.Value:N2}");

            var top = dict.OrderByDescending(k => k.Value).Take(5);
            Console.WriteLine("\nTop 5 categorias (maior gasto):");
            foreach (var kv in top) Console.WriteLine($"{kv.Key}: R$ {kv.Value:N2}");
        }

        static void ExportCsv(ControleFinanceiro manager)
        {
            var all = manager.GetAll().ToList();
            if (!all.Any()) { Console.WriteLine("Nada para exportar."); return; }

            var lines = new System.Collections.Generic.List<string> { "Data;Tipo;Categoria;Descricao;Valor" };
            foreach (var t in all)
                lines.Add($"{t.Data:yyyy-MM-dd};{t.Tipo};{t.Categoria};{t.Descricao};{t.Valor.ToString(CultureInfo.InvariantCulture)}");

            var file = "export.csv";
            File.WriteAllLines(file, lines);
            Console.WriteLine($"Exportado para {file}");
        }
    }
}
