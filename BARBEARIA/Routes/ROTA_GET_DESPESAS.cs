// ============================================================
// Routes/ROTA_GET_DESPESAS.cs — Rotas GET de Despesas
//
// Responsabilidade:
//   - Expor endpoints de consulta das despesas da barbearia
//
// Endpoints registrados:
//   GET /api/despesas       → Lista todas as despesas cadastradas
//   GET /api/despesas/{id}  → Retorna uma despesa específica pelo ID
//
// Respostas possíveis:
//   200 OK        → Despesa(s) encontrada(s) e retornada(s)
//   404 Not Found → ID não encontrado na lista
// ============================================================

using ApiBarbearia;
using ApiBarbearia.Models;

namespace ApiBarbearia.Routes;

public static class ROTA_GET_DESPESAS
{
    // Método de extensão chamado em Program.cs para registrar as rotas GET
    public static void MapGetDespesas(this WebApplication app)
    {
        // Endpoint: retorna a lista completa de despesas
        app.MapGet("/api/despesas", () =>
        {
            return Results.Ok(Dados.Despesas);
        });

        // Endpoint: busca uma despesa específica pelo ID informado na URL
        app.MapGet("/api/despesas/{id}", (int id) =>
        {
            var despesa = Dados.Despesas.FirstOrDefault(d => d.Id == id);

            return despesa is not null
                ? Results.Ok(despesa)
                : Results.NotFound("Despesa não encontrada.");
        });
    }
}
