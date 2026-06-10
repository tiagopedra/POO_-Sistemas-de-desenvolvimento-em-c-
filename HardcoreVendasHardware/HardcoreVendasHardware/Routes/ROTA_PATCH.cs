using ApiVendasHardware.Models;
using ApiVendasHardware.Repositories;

namespace ApiVendasHardware.Routes
{
    // Rotas PATCH: atualizações parciais de clientes, produtos e pedidos
    public static class ROTA_PATCH
    {
        public static void MapPatchRoutes(this WebApplication app)
        {
            // ── Clientes ──────────────────────────────────────────────────────────

            // Atualiza apenas os campos informados (não-nulos) do cliente
            app.MapPatch("/api/clientes/{id}", (int id, PatchClienteRequest req, HardwareRepository repo) =>
            {
                var cliente = repo.ObterClientePorId(id);
                if (cliente is null)
                    return Results.NotFound($"Cliente com ID {id} não encontrado.");

                if (req.Nome is not null)
                {
                    if (string.IsNullOrWhiteSpace(req.Nome))
                        return Results.BadRequest("O nome do cliente não pode ser vazio.");
                    cliente.Nome = req.Nome;
                }

                if (req.Email is not null)
                {
                    var temp = new Cliente { Email = req.Email };
                    if (!temp.EmailValido())
                        return Results.BadRequest($"E-mail '{req.Email}' é inválido.");
                    cliente.Email = req.Email;
                }

                if (req.Telefone is not null)
                    cliente.Telefone = req.Telefone;

                if (req.Endereco is not null)
                    cliente.Endereco = req.Endereco;

                return Results.Ok(cliente);
            });

            // ── Produtos ──────────────────────────────────────────────────────────

            // Atualiza apenas os campos informados (não-nulos) do produto
            app.MapPatch("/api/produtos/{id}", (int id, PatchProdutoRequest req, HardwareRepository repo) =>
            {
                var produto = repo.ObterProdutoPorId(id);
                if (produto is null)
                    return Results.NotFound($"Produto com ID {id} não encontrado.");

                if (req.Nome is not null)
                {
                    if (string.IsNullOrWhiteSpace(req.Nome))
                        return Results.BadRequest("O nome do produto não pode ser vazio.");
                    produto.Nome = req.Nome;
                }

                if (req.PrecoVenda is not null)
                {
                    if (req.PrecoVenda <= 0)
                        return Results.BadRequest("O preço de venda deve ser maior que zero.");
                    produto.PrecoVenda = req.PrecoVenda.Value;
                }

                if (req.PrecoCusto is not null)
                {
                    if (req.PrecoCusto < 0)
                        return Results.BadRequest("O preço de custo não pode ser negativo.");
                    produto.PrecoCusto = req.PrecoCusto.Value;
                }

                if (req.Categoria is not null)
                    produto.Categoria = req.Categoria;

                if (req.Fabricante is not null)
                    produto.Fabricante = req.Fabricante;

                if (req.Modelo is not null)
                    produto.Modelo = req.Modelo;

                if (req.Descricao is not null)
                    produto.Descricao = req.Descricao;

                return Results.Ok(produto);
            });

            // ── Pedidos ───────────────────────────────────────────────────────────

            // Atualiza apenas as observações do pedido
            app.MapPatch("/api/pedidos/{id}/observacoes", (int id, PatchObservacoesRequest req, HardwareRepository repo) =>
            {
                var pedido = repo.ObterPedidoPorId(id);
                if (pedido is null)
                    return Results.NotFound($"Pedido com ID {id} não encontrado.");

                if (pedido.Status == StatusPedido.Cancelado || pedido.Status == StatusPedido.Entregue)
                    return Results.BadRequest("Não é possível editar pedidos cancelados ou já entregues.");

                pedido.Observacoes = req.Observacoes;
                return Results.Ok(pedido);
            });
        }
    }

    // Todos os campos são nullable: só os informados na requisição serão alterados
    public record PatchClienteRequest(
        string? Nome,
        string? Email,
        string? Telefone,
        string? Endereco
    );

    public record PatchProdutoRequest(
        string? Nome,
        decimal? PrecoVenda,
        decimal? PrecoCusto,
        string? Categoria,
        string? Fabricante,
        string? Modelo,
        string? Descricao
    );

    public record PatchObservacoesRequest(string? Observacoes);
}
