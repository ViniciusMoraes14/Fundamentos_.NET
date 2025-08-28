using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using ControleFinanceiroApp.Models;

namespace ControleFinanceiroApp.Services
{
    public class ControleFinanceiro
    {
        private readonly string filePath;
        private List<Transacao> transacoes;
        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public ControleFinanceiro(string dataFilePath = "dados.json")
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            filePath = dataFilePath;
            Load();
        }

        public void Add(Transacao t)
        {
            transacoes.Add(t);
            Save();
        }

        public IEnumerable<Transacao> GetAll() => transacoes.OrderByDescending(t => t.Data);

        public IEnumerable<Transacao> GetByMonth(int month, int year)
            => transacoes.Where(t => t.Data.Month == month && t.Data.Year == year)
                         .OrderByDescending(t => t.Data);

        public decimal GetBalance(int? month = null, int? year = null)
        {
            var q = transacoes.AsEnumerable();
            if (month.HasValue && year.HasValue) q = q.Where(t => t.Data.Month == month.Value && t.Data.Year == year.Value);
            decimal receitas = q.Where(t => t.Tipo == TipoTransacao.Receita).Sum(t => t.Valor);
            decimal despesas = q.Where(t => t.Tipo == TipoTransacao.Despesa).Sum(t => t.Valor);
            return receitas - despesas;
        }

        public Dictionary<string, decimal> GetSummaryByCategory(int? month = null, int? year = null)
        {
            var q = transacoes.AsEnumerable();
            if (month.HasValue && year.HasValue) q = q.Where(t => t.Data.Month == month.Value && t.Data.Year == year.Value);
            return q.Where(t => t.Tipo == TipoTransacao.Despesa)
                    .GroupBy(t => t.Categoria)
                    .ToDictionary(g => g.Key, g => g.Sum(t => t.Valor));
        }

        private void Save()
        {
            var temp = filePath + ".tmp";
            var json = JsonSerializer.Serialize(transacoes, jsonOptions);
            File.WriteAllText(temp, json);
            File.Copy(temp, filePath, true);
            File.Delete(temp);
        }

        private void Load()
        {
            if (!File.Exists(filePath))
            {
                transacoes = new List<Transacao>();
                return;
            }

            try
            {
                var json = File.ReadAllText(filePath);
                transacoes = JsonSerializer.Deserialize<List<Transacao>>(json, jsonOptions) ?? new List<Transacao>();
            }
            catch (Exception)
            {
                // se o arquivo estiver corrompido, move para backup e começa vazio
                var bad = filePath + $".bad_{DateTime.Now:yyyyMMddHHmmss}";
                File.Move(filePath, bad);
                transacoes = new List<Transacao>();
            }
        }
    }
}
