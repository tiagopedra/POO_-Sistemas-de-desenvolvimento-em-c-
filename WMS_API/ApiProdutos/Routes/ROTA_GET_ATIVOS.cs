using ApiProdutos.Data;

namespace ApiProdutos.Routes;

public static class ROTA_GET_ATIVOS
{
    public static void MapGetAtivosRoutes(this WebApplication app)
    {
        app.MapGet("/api/materiais/ativos", () =>
        {
            var ativos = BancoSimulado.Materiais
                .Where(m => m.Ativo == true)
                .ToList();

            return Results.Ok(ativos);
        });
    }
}