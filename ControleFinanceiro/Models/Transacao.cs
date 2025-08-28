using System;
using System.Text.Json.Serialization;

namespace ControleFinanceiroApp.Models
{
    public enum TipoTransacao { Receita, Despesa }

    public class Transacao
    {
        public DateTime Data { get; set; }
        public string Descricao { get; set; } = "";
        public decimal Valor { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TipoTransacao Tipo { get; set; }
        public string Categoria { get; set; } = "Geral";

        public override string ToString() =>
            $"{Data:yyyy-MM-dd} | {Tipo} | {Categoria} | {Descricao} | R$ {Valor:N2}";
    }
}
