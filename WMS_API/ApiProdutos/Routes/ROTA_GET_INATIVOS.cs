using ApiProdutos.Data;

namespace ApiProdutos.Routes;

public static class ROTA_GET_INATIVOS
{
    public static void MapGetInativosRoutes(this WebApplication app)
    {
        app.MapGet("/api/materiais/inativos", () =>
        {
            var inativos = BancoSimulado.Materiais
                .Where(m => m.Ativo == false)
                .ToList();

            return Results.Ok(inativos);
        });
    }
}