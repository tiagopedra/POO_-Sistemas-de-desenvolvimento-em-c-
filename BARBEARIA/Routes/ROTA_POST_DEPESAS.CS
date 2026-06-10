// ============================================================
// Routes/ROTA_POST_DEPESAS.CS — Rotas POST e PUT de Despesas
//
// Responsabilidade:
//   - Criar novas despesas (POST)
//   - Atualizar despesas existentes (PUT)
//
// Endpoints registrados:
//   POST /api/despesas       → Registra uma nova despesa
//   PUT  /api/despesas/{id}  → Atualiza a descrição e valor de uma despesa
//
// Observação:
//   O total das despesas é usado pelo endpoint /api/financeiro
//   para calcular o lucro: Lucro = Receitas - Despesas
// ============================================================

using ApiBarbearia;
using ApiBarbearia.Models;

namespace ApiBarbearia.Routes;

public static class ROTA_POST_DESPESAS
{
    // Método de extensão chamado em Program.cs para registrar as rotas POST e PUT
    public static void MapPostDespesas(this WebApplication app)
    {
        // Endpoint: cria uma nova despesa com os dados do corpo da requisição (JSON)
        app.MapPost("/api/despesas", (Despesa novo) =>
        {
            // Gera o próximo ID disponível
            novo.Id = Dados.Despesas.Count > 0 ? Dados.Despesas.Max(d => d.Id) + 1 : 1;

            Dados.Despesas.Add(novo);

            return Results.Created($"/api/despesas/{novo.Id}", novo);
        });

        // Endpoint: atualiza os dados de uma despesa existente pelo ID
        app.MapPut("/api/despesas/{id}", (int id, Despesa atualizado) =>
        {
            var despesa = Dados.Despesas.FirstOrDefault(d => d.Id == id);
            if (despesa is null) return Results.NotFound("Despesa não encontrada.");

            // Atualiza os campos editáveis da despesa
            despesa.Descricao = atualizado.Descricao;
            despesa.Valor     = atualizado.Valor;

            return Results.Ok(despesa);
        });
    }
}
