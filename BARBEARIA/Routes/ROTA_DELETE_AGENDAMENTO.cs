// ============================================================
// Routes/ROTA_DELETE_AGENDAMENTO.cs — Rota DELETE de Agendamentos
//
// Responsabilidade:
//   - Remover um agendamento da lista em memória
//
// Endpoint registrado:
//   DELETE /api/agendamentos/{id} → Exclui o agendamento com o ID informado
//
// Respostas possíveis:
//   200 OK        → Agendamento excluído com sucesso
//   404 Not Found → ID não encontrado na lista
// ============================================================

using ApiBarbearia;
using ApiBarbearia.Models;

namespace ApiBarbearia.Routes;

public static class ROTA_DELETE_AGENDAMENTO
{
    // Método de extensão chamado em Program.cs para registrar a rota DELETE
    public static void MapDeleteAgendamentosRoutes(this WebApplication app)
    {
        // Endpoint: localiza e remove o agendamento com o ID informado
        app.MapDelete("/api/agendamentos/{id}", (int id) =>
        {
            var agendamento = Dados.Agendamentos.FirstOrDefault(a => a.Id == id);

            // Se não encontrar, retorna 404 com mensagem de erro
            if (agendamento is null) return Results.NotFound("Agendamento não encontrado.");

            Dados.Agendamentos.Remove(agendamento);

            return Results.Ok("Agendamento excluído com sucesso.");
        });
    }
}
