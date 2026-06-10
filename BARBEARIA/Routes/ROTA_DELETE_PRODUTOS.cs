// ============================================================
// Routes/ROTA_DELETE_PRODUTOS.cs — Rota DELETE de Produtos
//
// Responsabilidade:
//   - Remover um produto/serviço da lista em memória
//
// Endpoint registrado:
//   DELETE /api/produtos/{id} → Exclui o produto com o ID informado
//
// Atenção:
//   Se o produto excluído estiver vinculado a agendamentos ativos,
//   o cálculo financeiro em /api/financeiro não conseguirá encontrar
//   o preço do serviço e irá considerar R$ 0,00 para esses agendamentos.
// ============================================================

using ApiBarbearia;
using ApiBarbearia.Models;

namespace ApiBarbearia.Routes;

public static class ROTA_DELETE_PRODUTOS
{
    // Método de extensão chamado em Program.cs para registrar a rota DELETE
    public static void MapDeleteProdutosRoutes(this WebApplication app)
    {
        // Endpoint: localiza e remove o produto com o ID informado
        app.MapDelete("/api/produtos/{id}", (int id) =>
        {
            var produto = Dados.Produtos.FirstOrDefault(p => p.Id == id);

            if (produto is null) return Results.NotFound("Produto não encontrado.");

            Dados.Produtos.Remove(produto);

            return Results.Ok("Produto excluído com sucesso.");
        });
    }
}
