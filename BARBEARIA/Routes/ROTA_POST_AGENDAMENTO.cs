// ============================================================
// Routes/ROTA_POST_AGENDAMENTO.cs — Rotas POST e PUT de Agendamentos
//
// Responsabilidade:
//   - Criar novos agendamentos (POST)
//   - Atualizar agendamentos existentes (PUT)
//
// Endpoints registrados:
//   POST /api/agendamentos       → Cria um novo agendamento
//   PUT  /api/agendamentos/{id}  → Atualiza os dados de um agendamento existente
//
// Comportamento do POST:
//   - O ID é gerado automaticamente (máximo ID atual + 1)
//   - O agendamento é adicionado à lista em memória
//   - Retorna 201 Created com os dados do agendamento criado
//
// Comportamento do PUT:
//   - Localiza o agendamento pelo ID da URL
//   - Substitui todos os campos pelo conteúdo do corpo da requisição
//   - Retorna 200 OK com os dados atualizados
//   - Retorna 404 se o ID não for encontrado
// ============================================================

using ApiBarbearia;
using ApiBarbearia.Models;

namespace ApiBarbearia.Routes;

public static class ROTA_POST_AGENDAMENTO
{
    // Método de extensão chamado em Program.cs para registrar as rotas POST e PUT
    public static void MapPostAgendamentosRoutes(this WebApplication app)
    {
        // Endpoint: cria um novo agendamento com os dados do corpo da requisição (JSON)
        app.MapPost("/api/agendamentos", (Agendamento novo) =>
        {
            // Gera o próximo ID disponível (evita conflito com os existentes)
            novo.Id = Dados.Agendamentos.Count > 0 ? Dados.Agendamentos.Max(a => a.Id) + 1 : 1;

            Dados.Agendamentos.Add(novo);

            // Retorna 201 Created com a URL do novo recurso e os dados criados
            return Results.Created($"/api/agendamentos/{novo.Id}", novo);
        });

        // Endpoint: atualiza os dados de um agendamento existente pelo ID
        app.MapPut("/api/agendamentos/{id}", (int id, Agendamento atualizado) =>
        {
            var agendamento = Dados.Agendamentos.FirstOrDefault(a => a.Id == id);
            if (agendamento is null) return Results.NotFound("Agendamento não encontrado.");

            // Atualiza cada campo do agendamento com os novos valores recebidos
            agendamento.Cliente    = atualizado.Cliente;
            agendamento.Servico    = atualizado.Servico;
            agendamento.Data       = atualizado.Data;
            agendamento.Horario    = atualizado.Horario;
            agendamento.Observacao = atualizado.Observacao;

            return Results.Ok(agendamento);
        });
    }
}
