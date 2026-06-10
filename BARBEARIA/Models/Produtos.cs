// ============================================================
// Models/Produtos.cs — Modelo de dados: Produto (Serviço)
//
// Responsabilidade:
//   - Representar um serviço oferecido pela barbearia
//   - O campo "Nome" é usado como chave de vínculo com agendamentos
//     (o campo Agendamento.Servico deve corresponder a Produto.Nome)
//
// Campos:
//   Id        → Identificador único gerado automaticamente
//   Nome      → Nome do serviço (ex: "Corte Degradê", "Barba Completa")
//   Descricao → Texto explicativo sobre o que o serviço inclui
//   Preco     → Valor cobrado pelo serviço em reais (decimal)
//
// Observação:
//   A rota financeira (/api/financeiro) usa o Preco dos Produtos
//   para calcular o total de receitas com base nos agendamentos.
// ============================================================

namespace ApiBarbearia.Models
{
    public class Produtos
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
    }
}
