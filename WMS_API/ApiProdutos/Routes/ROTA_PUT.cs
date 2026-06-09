using ApiProdutos.Data;
using ApiProdutos.Models;

namespace ApiProdutos.Routes;

public static class ROTA_PUT
{
    public static void MapPutRoutes(this WebApplication app)
    {
        app.MapPut("/api/materiais/{id}", (int id, MaterialEscolar materialAtualizado) =>
        {
            var materialExistente = BancoSimulado.Materiais
                .FirstOrDefault(m => m.Id == id);

            if (materialExistente == null)
            {
                return Results.NotFound("Material não encontrado.");
            }

            materialExistente.Nome = materialAtualizado.Nome;
            materialExistente.Categoria = materialAtualizado.Categoria;
            materialExistente.Ativo = materialAtualizado.Ativo;
            materialExistente.Lotes = materialAtualizado.Lotes;

            return Results.Ok(new
            {
                Mensagem = "Material atualizado com sucesso.",
                Material = materialExistente
            });
        });
    }
}