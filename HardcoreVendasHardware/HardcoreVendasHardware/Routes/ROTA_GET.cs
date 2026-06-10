using ApiVendasHardware.Repositories;

namespace ApiVendasHardware.Routes
{
    // Rotas GET: leitura de clientes, produtos e pedidos
    public static class ROTA_GET
    {
        public static void MapGetRoutes(this WebApplication app)
        {
            app.MapGet("/", () => "API de Vendas e Pedidos de Hardware em funcionamento!");

            // ── Clientes ──────────────────────────────────────────────────────────

            app.MapGet("/api/clientes", (HardwareRepository repo) =>
                Results.Ok(repo.ObterTodosClientes()));

            app.MapGet("/api/clientes/{id}", (int id, HardwareRepository repo) =>
            {
                var cliente = repo.ObterClientePorId(id);
                return cliente is not null
                    ? Results.Ok(cliente)
                    : Results.NotFound($"Cliente com ID {id} não encontrado.");
            });

            // ── Produtos ──────────────────────────────────────────────────────────

            app.MapGet("/api/produtos", (HardwareRepository repo) =>
                Results.Ok(repo.ObterTodosProdutos()));

            app.MapGet("/api/produtos/{id:int}", (int id, HardwareRepository repo) =>
            {
                var produto = repo.ObterProdutoPorId(id);
                return produto is not null
                    ? Results.Ok(produto)
                    : Results.NotFound($"Produto com ID {id} não encontrado.");
            });

            // Filtra por categoria (ex: CPU, GPU, RAM)
            app.MapGet("/api/produtos/filtro/categoria/{categoria}", (string categoria, HardwareRepository repo) =>
            {
                var lista = repo.ObterTodosProdutos()
                    .Where(p => p.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                return lista.Any()
                    ? Results.Ok(lista)
                    : Results.NotFound($"Nenhum produto encontrado na categoria '{categoria}'.");
            });

            // Apenas produtos com estoque maior que zero
            app.MapGet("/api/produtos/filtro/disponiveis", (HardwareRepository repo) =>
                Results.Ok(repo.ObterTodosProdutos().Where(p => p.EstoqueDisponivel > 0)));

            // ── Pedidos ───────────────────────────────────────────────────────────

            app.MapGet("/api/pedidos", (HardwareRepository repo) =>
                Results.Ok(repo.ObterTodosPedidos()));

            app.MapGet("/api/pedidos/{id}", (int id, HardwareRepository repo) =>
            {
                var pedido = repo.ObterPedidoPorId(id);
                return pedido is not null
                    ? Results.Ok(pedido)
                    : Results.NotFound($"Pedido com ID {id} não encontrado.");
            });

            // Pedidos de um cliente específico
            app.MapGet("/api/pedidos/cliente/{clienteId:int}", (int clienteId, HardwareRepository repo) =>
            {
                var lista = repo.ObterTodosPedidos()
                    .Where(p => p.ClienteId == clienteId)
                    .ToList();

                return lista.Any()
                    ? Results.Ok(lista)
                    : Results.NotFound($"Nenhum pedido encontrado para o cliente ID {clienteId}.");
            });

            // Histórico de status de um pedido
            app.MapGet("/api/pedidos/{id}/historico", (int id, HardwareRepository repo) =>
            {
                var pedido = repo.ObterPedidoPorId(id);
                return pedido is not null
                    ? Results.Ok(pedido.Historico)
                    : Results.NotFound($"Pedido com ID {id} não encontrado.");
            });

            // Resumo financeiro do pedido
            app.MapGet("/api/pedidos/{id}/resumo", (int id, HardwareRepository repo) =>
            {
                var pedido = repo.ObterPedidoPorId(id);
                return pedido is not null
                    ? Results.Ok(pedido.ObterResumo())
                    : Results.NotFound($"Pedido com ID {id} não encontrado.");
            });
        }
    }
}
