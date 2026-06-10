// ============================================================
// Routes/ROTA_POST_PRODUTOS.cs — Rotas POST e PUT de Produtos
//
// Responsabilidade:
//   - Criar novos produtos/serviços (POST)
//   - Atualizar produtos/serviços existentes (PUT)
//
// Endpoints registrados:
//   POST /api/produtos       → Cria um novo produto/serviço
//   PUT  /api/produtos/{id}  → Atualiza Nome, Descrição e Preço de um produto
//
// Observação:
//   O campo "Nome" do produto é usado como chave de vínculo com
//   agendamentos no cálculo financeiro. Alterá-lo pode afetar
//   o cálculo de receitas na rota /api/financeiro.
// ============================================================

using ApiBarbearia;
using ApiBarbearia.Models;

namespace ApiBarbearia.Routes;

public static class ROTA_POST_PRODUTOS
{
    // Método de extensão chamado em Program.cs para registrar as rotas POST e PUT
    public static void MapPostProdutosRoutes(this WebApplication app)
    {
        // Endpoint: cria um novo produto com os dados do corpo da requisição (JSON)
        app.MapPost("/api/produtos", (Produtos novo) =>
        {
            // Gera o próximo ID disponível
            novo.Id = Dados.Produtos.Count > 0 ? Dados.Produtos.Max(p => p.Id) + 1 : 1;

            Dados.Produtos.Add(novo);

            return Results.Created($"/api/produtos/{novo.Id}", novo);
        });

        // Endpoint: atualiza os dados de um produto existente pelo ID
        app.MapPut("/api/produtos/{id}", (int id, Produtos atualizado) =>
        {
            var produto = Dados.Produtos.FirstOrDefault(p => p.Id == id);
            if (produto is null) return Results.NotFound("Produto não encontrado.");

            // Atualiza os campos editáveis do produto
            produto.Nome      = atualizado.Nome;
            produto.Descricao = atualizado.Descricao;
            produto.Preco     = atualizado.Preco;

            return Results.Ok(produto);
        });
    }
}
