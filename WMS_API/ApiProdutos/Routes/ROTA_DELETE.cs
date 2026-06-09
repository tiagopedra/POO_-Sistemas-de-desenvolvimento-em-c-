using ApiProdutos.Data;

namespace ApiProdutos.Routes;

public static class ROTA_DELETE
{
    public static void MapDeleteRoutes(this WebApplication app)
    {
        app.MapDelete("/api/materiais/{id}", (int id) =>
        {
            var material = BancoSimulado.Materiais
                .FirstOrDefault(m => m.Id == id);

            if (material == null)
            {
                return Results.NotFound("Material não encontrado.");
            }

            BancoSimulado.Materiais.Remove(material);

            return Results.Ok("Material removido com sucesso.");
        });
    }
}