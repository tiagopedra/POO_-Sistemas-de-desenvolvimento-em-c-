// ============================================================
// Routes/ROTA_GET_PRODUTOS.cs — Rotas GET de Produtos (Serviços)
//
// Responsabilidade:
//   - Expor endpoints de consulta de produtos/serviços da barbearia
//
// Endpoints registrados:
//   GET /api/produtos       → Lista todos os produtos cadastrados
//   GET /api/produtos/{id}  → Retorna um produto específico pelo ID
//
// Respostas possíveis:
//   200 OK        → Produto(s) encontrado(s) e retornado(s)
//   404 Not Found → ID não existe na lista
// ============================================================

using ApiBarbearia;
using ApiBarbearia.Models;

namespace ApiBarbearia.Routes;

public static class ROTA_GET_PRODUTOS
{
    // Método de extensão chamado em Program.cs para registrar as rotas GET
    public static void MapGetProdutosRoutes(this WebApplication app)
    {
        // Endpoint: retorna a lista completa de produtos/serviços
        app.MapGet("/api/produtos", () =>
        {
            return Results.Ok(Dados.Produtos);
        });

        // Endpoint: busca um produto específico pelo ID informado na URL
        app.MapGet("/api/produtos/{id}", (int id) =>
        {
            var produto = Dados.Produtos.FirstOrDefault(p => p.Id == id);

            return produto != null
                ? Results.Ok(produto)
                : Results.NotFound("Produto não encontrado.");
        });
    }
}
