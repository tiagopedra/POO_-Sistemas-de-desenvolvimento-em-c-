using ApiVendasHardware.Models;
using ApiVendasHardware.Repositories;
using ApiVendasHardware.Services;

namespace ApiVendasHardware.Routes
{
    // Rotas POST: criação de clientes, produtos, pedidos e ações de status
    public static class ROTA_POST
    {
        public static void MapPostRoutes(this WebApplication app)
        {
            // ── Clientes ──────────────────────────────────────────────────────────

            app.MapPost("/api/clientes", (Cliente novo, HardwareRepository repo) =>
            {
                if (string.IsNullOrWhiteSpace(novo.Nome))
                    return Results.BadRequest("O nome do cliente é obrigatório.");

                if (!novo.EmailValido())
                    return Results.BadRequest($"E-mail '{novo.Email}' é inválido.");

                var criado = repo.AdicionarCliente(novo);
                return Results.Created($"/api/clientes/{criado.Id}", criado);
            });

            // ── Produtos ──────────────────────────────────────────────────────────

            app.MapPost("/api/produtos", (Produto novo, HardwareRepository repo) =>
            {
                if (string.IsNullOrWhiteSpace(novo.Nome))
                    return Results.BadRequest("O nome do produto é obrigatório.");

                if (novo.Preco <= 0)
                    return Results.BadRequest("O preço deve ser maior que zero.");

                var criado = repo.AdicionarProduto(novo);
                return Results.Created($"/api/produtos/{criado.Id}", criado);
            });

            // ── Pedidos ───────────────────────────────────────────────────────────

            // Criação do pedido com validação via PedidoService
            app.MapPost("/api/pedidos", (Pedido novo, HardwareRepository repo, PedidoService service) =>
            {
                var erro = service.ValidarEPreparar(novo);
                if (erro is not null)
                    return Results.BadRequest(erro);

                novo.Confirmar();
                var criado = repo.AdicionarPedido(novo);
                return Results.Created($"/api/pedidos/{criado.Id}", criado);
            });

            // Avança o status para o próximo estágio do fluxo
            app.MapPost("/api/pedidos/{id}/avancar-status", (int id, HardwareRepository repo) =>
            {
                var pedido = repo.ObterPedidoPorId(id);
                if (pedido is null)
                    return Results.NotFound($"Pedido com ID {id} não encontrado.");

                try
                {
                    pedido.AvancarStatus();
                    return Results.Ok(pedido);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            // Cancela o pedido se ainda for possível
            app.MapPost("/api/pedidos/{id}/cancelar", (int id, HardwareRepository repo) =>
            {
                var pedido = repo.ObterPedidoPorId(id);
                if (pedido is null)
                    return Results.NotFound($"Pedido com ID {id} não encontrado.");

                try
                {
                    pedido.Cancelar();
                    return Results.Ok(pedido);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });
        }
    }
}
