// ============================================================
// Routes/ROTA_GET_AGENDAMENTO.cs — Rotas GET de Agendamentos
//
// Responsabilidade:
//   - Expor endpoints de consulta (leitura) de agendamentos
//   - Retorna dados em formato JSON
//
// Endpoints registrados:
//   GET /api/agendamentos       → Lista todos os agendamentos
//   GET /api/agendamentos/{id}  → Retorna um agendamento específico pelo ID
//
// Respostas possíveis:
//   200 OK         → Dados encontrados e retornados
//   404 Not Found  → ID informado não existe na lista
// ============================================================

using ApiBarbearia;
using ApiBarbearia.Models;

namespace ApiBarbearia.Routes;

public static class ROTA_GET_AGENDAMENTO
{
    // Método de extensão chamado em Program.cs para registrar as rotas GET
    public static void MapGetAgendamentosRoutes(this WebApplication app)
    {
        // Endpoint: retorna a lista completa de agendamentos
        app.MapGet("/api/agendamentos", () =>
        {
            return Results.Ok(Dados.Agendamentos);
        });

        // Endpoint: busca um agendamento específico pelo ID informado na URL
        app.MapGet("/api/agendamentos/{id}", (int id) =>
        {
            var agendamento = Dados.Agendamentos.FirstOrDefault(a => a.Id == id);

            // Se não encontrar o ID, retorna 404 com mensagem de erro
            return agendamento != null
                ? Results.Ok(agendamento)
                : Results.NotFound("Agendamento não encontrado.");
        });
    }
}
