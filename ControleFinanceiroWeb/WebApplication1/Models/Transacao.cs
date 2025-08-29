namespace ControleFinanceiroWeb.Models
{
    public class Transacao
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string Tipo { get; set; } = string.Empty;     // "Entrada" ou "Saída"
        public string Categoria { get; set; } = string.Empty;
    }
}
