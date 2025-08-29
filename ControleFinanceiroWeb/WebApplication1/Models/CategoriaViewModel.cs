namespace ControleFinanceiroWeb.Models
{
    public class CategoriaViewModel
    {
        public string Categoria { get; set; } = string.Empty; // Nome da categoria
        public decimal Total { get; set; }                    // Entradas - Saídas
    }
}
