// ============================================================
// Routes/ROTA_DELETE_DESPESAS.cs — Rota DELETE de Despesas
//
// Responsabilidade:
//   - Remover uma despesa da lista em memória
//
// Endpoint registrado:
//   DELETE /api/despesas/{id} → Exclui a despesa com o ID informado
//
// Respostas possíveis:
//   200 OK        → Despesa excluída com sucesso
//   404 Not Found → ID não encontrado na lista
// ============================================================

using ApiBarbearia;
using ApiBarbearia.Models;

namespace ApiBarbearia.Routes;

public static class ROTA_DELETE_DESPESAS
{
    // Método de extensão chamado em Program.cs para registrar a rota DELETE
    public static void MapDeleteDespesas(this WebApplication app)
    {
        // Endpoint: localiza e remove a despesa com o ID informado
        app.MapDelete("/api/despesas/{id}", (int id) =>
        {
            var despesa = Dados.Despesas.FirstOrDefault(d => d.Id == id);

            if (despesa is null) return Results.NotFound("Despesa não encontrada.");

            Dados.Despesas.Remove(despesa);

            return Results.Ok("Despesa excluída com sucesso.");
        });
    }
}
