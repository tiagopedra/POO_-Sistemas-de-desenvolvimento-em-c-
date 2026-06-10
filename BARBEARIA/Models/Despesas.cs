// ============================================================
// Models/Despesas.cs — Modelo de dados: Despesa
//
// Responsabilidade:
//   - Representar uma despesa fixa ou variável da barbearia
//   - Ser usado no cálculo do resumo financeiro (/api/financeiro)
//
// Campos:
//   Id        → Identificador único gerado automaticamente
//   Descricao → Nome ou descrição da despesa (ex: "Aluguel", "Água")
//   Valor     → Valor monetário da despesa em reais (decimal)
// ============================================================

namespace ApiBarbearia.Models
{
    public class Despesa
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
    }
}
