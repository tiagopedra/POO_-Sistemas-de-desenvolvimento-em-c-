using ApiVendasHardware.Models;
using ApiVendasHardware.Repositories;

namespace ApiVendasHardware.Routes
{
    public static class ROTA_PUT
    {
        public static void MapPutRoutes(this WebApplication app)
        {
            // ── Clientes ──────────────────────────────────────────────────────────

            app.MapPut("/api/clientes/{id}", (int id, Cliente atualizado, HardwareRepository repo) =>
            {
                if (string.IsNullOrWhiteSpace(atualizado.Nome))
                    return Results.BadRequest("O nome do cliente é obrigatório.");

                if (!atualizado.EmailValido())
                    return Results.BadRequest($"E-mail '{atualizado.Email}' é inválido.");

                var cliente = repo.AtualizarCliente(id, atualizado);
                return cliente is not null
                    ? Results.Ok(cliente)
                    : Results.NotFound($"Cliente com ID {id} não encontrado.");
            });

            // ── Produtos ──────────────────────────────────────────────────────────

            app.MapPut("/api/produtos/{id}", (int id, Produto atualizado, HardwareRepository repo) =>
            {
                if (string.IsNullOrWhiteSpace(atualizado.Nome))
                    return Results.BadRequest("O nome do produto é obrigatório.");

                if (atualizado.PrecoVenda <= 0)
                    return Results.BadRequest("O preço de venda deve ser maior que zero.");

                if (atualizado.PrecoCusto < 0)
                    return Results.BadRequest("O preço de custo não pode ser negativo.");

                var produto = repo.AtualizarProduto(id, atualizado);
                return produto is not null
                    ? Results.Ok(produto)
                    : Results.NotFound($"Produto com ID {id} não encontrado.");
            });

            // Reabastece estoque de um produto
            app.MapPut("/api/produtos/{id}/estoque", (int id, ReabastecimentoRequest req, HardwareRepository repo) =>
            {
                var produto = repo.ObterProdutoPorId(id);
                if (produto is null)
                    return Results.NotFound($"Produto com ID {id} não encontrado.");

                try
                {
                    produto.ReabastecerEstoque(req.Quantidade);
                    return Results.Ok(produto);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            // ── Pedidos ───────────────────────────────────────────────────────────

            app.MapPut("/api/pedidos/{id}/observacoes", (int id, ObservacoesRequest req, HardwareRepository repo) =>
            {
                var pedido = repo.ObterPedidoPorId(id);
                if (pedido is null)
                    return Results.NotFound($"Pedido com ID {id} não encontrado.");

                if (pedido.Status == StatusPedido.Cancelado || pedido.Status == StatusPedido.Entregue)
                    return Results.BadRequest("Não é possível editar pedidos cancelados ou já entregues.");

                pedido.Observacoes = req.Observacoes;
                return Results.Ok(pedido);
            });

           app.MapPut("/api/pedidos/{id}/cancelar", (int id, HardwareRepository repo) =>
{
    var pedido = repo.ObterPedidoPorId(id);
    if (pedido is null)
        return Results.NotFound($"Pedido com ID {id} não encontrado.");

    try
    {
        pedido.Cancelar();

        // Devolve os itens ao estoque
        foreach (var item in pedido.Itens)
        {
            var produto = repo.ObterProdutoPorId(item.ProdutoId);
            if (produto is not null)
                produto.ReabastecerEstoque(item.Quantidade);
        }

        return Results.Ok(pedido);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
        }
    }

    public record ReabastecimentoRequest(int Quantidade);
    public record ObservacoesRequest(string Observacoes);
}