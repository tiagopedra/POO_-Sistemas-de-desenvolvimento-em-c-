using ApiVendasHardware.Models;
using ApiVendasHardware.Repositories;

namespace ApiVendasHardware.Routes
{
    // Rotas DELETE: remoção de clientes, produtos e pedidos cancelados
    public static class ROTA_DELETE
    {
        public static void MapDeleteRoutes(this WebApplication app)
        {
            // ── Clientes ──────────────────────────────────────────────────────────

            // Bloqueia remoção se o cliente tiver pedidos ativos
            app.MapDelete("/api/clientes/{id}", (int id, HardwareRepository repo) =>
            {
                var temPedidosAtivos = repo.ObterTodosPedidos()
                    .Any(p => p.ClienteId == id &&
                              p.Status != StatusPedido.Cancelado &&
                              p.Status != StatusPedido.Entregue);

                if (temPedidosAtivos)
                    return Results.Conflict($"Cliente ID {id} possui pedidos ativos e não pode ser removido.");

                var removido = repo.RemoverCliente(id);
                return removido
                    ? Results.NoContent()
                    : Results.NotFound($"Cliente com ID {id} não encontrado.");
            });

            // ── Produtos ──────────────────────────────────────────────────────────

            // Bloqueia remoção se o produto estiver em algum pedido
            app.MapDelete("/api/produtos/{id}", (int id, HardwareRepository repo) =>
            {
                var emPedido = repo.ObterTodosPedidos()
                    .Any(p => p.Itens.Any(i => i.ProdutoId == id));

                if (emPedido)
                    return Results.Conflict($"Produto ID {id} está vinculado a pedidos e não pode ser removido.");

                var removido = repo.RemoverProduto(id);
                return removido
                    ? Results.NoContent()
                    : Results.NotFound($"Produto com ID {id} não encontrado.");
            });

            // ── Pedidos ───────────────────────────────────────────────────────────

            // Apenas pedidos com status Cancelado podem ser excluídos
            app.MapDelete("/api/pedidos/{id}", (int id, HardwareRepository repo) =>
            {
                var pedido = repo.ObterPedidoPorId(id);
                if (pedido is null)
                    return Results.NotFound($"Pedido com ID {id} não encontrado.");

                if (pedido.Status != StatusPedido.Cancelado)
                    return Results.BadRequest(
                        "Somente pedidos cancelados podem ser excluídos. " +
                        "Use POST /api/pedidos/{id}/cancelar antes de deletar.");

                repo.RemoverPedido(id);
                return Results.NoContent();
            });
        }
    }
}
