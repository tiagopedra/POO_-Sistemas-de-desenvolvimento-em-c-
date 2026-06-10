// ============================================================
// Routes/ROTA_FINCANCEIRO.cs — Rota GET do Resumo Financeiro
//
// Responsabilidade:
//   - Calcular e retornar o resumo financeiro da barbearia
//   - Consolidar receitas (agendamentos × preço dos serviços)
//     com as despesas cadastradas para apurar o lucro
//
// Endpoint registrado:
//   GET /api/financeiro → Retorna o resumo financeiro completo
//
// Lógica de cálculo:
//   Receitas  = Soma do Preco de cada serviço agendado
//               (buscado por correspondência de nome em Produtos)
//   Despesas  = Soma de todos os valores em Dados.Despesas
//   Lucro     = Receitas - Despesas
//   Situacao  = "Positivo" se Lucro >= 0, caso contrário "Negativo"
//
// Retorno JSON:
//   {
//     TotalReceitas, TotalDespesas, Lucro, Situacao,
//     Detalhes: { TotalAgendamentos, Despesas: [{ Descricao, Valor }] }
//   }
// ============================================================

using ApiBarbearia;
using ApiBarbearia.Models;

namespace ApiBarbearia.Routes;

public static class ROTA_FINANCEIRO
{
    // Método de extensão chamado em Program.cs para registrar a rota financeira
    public static void MapFinanceiroRoutes(this WebApplication app)
    {
        app.MapGet("/api/financeiro", () =>
        {
            // Calcula o total de receitas:
            // Para cada agendamento, busca o produto com o mesmo nome do serviço
            // e soma o preço. Se o serviço não for encontrado, considera R$ 0,00
            var totalReceitas = Dados.Agendamentos.Sum(a =>
                Dados.Produtos.FirstOrDefault(p => p.Nome == a.Servico)?.Preco ?? 0m);

            // Soma o valor de todas as despesas cadastradas
            var totalDespesas = Dados.Despesas.Sum(d => d.Valor);

            // Calcula o lucro final
            var lucro = totalReceitas - totalDespesas;

            // Retorna o resumo completo com detalhes de despesas e total de agendamentos
            return Results.Ok(new
            {
                TotalReceitas = totalReceitas,
                TotalDespesas = totalDespesas,
                Lucro         = lucro,
                Situacao      = lucro >= 0 ? "Positivo" : "Negativo",
                Detalhes = new
                {
                    TotalAgendamentos = Dados.Agendamentos.Count,
                    Despesas = Dados.Despesas.Select(d => new { d.Descricao, d.Valor })
                }
            });
        });
    }
}
