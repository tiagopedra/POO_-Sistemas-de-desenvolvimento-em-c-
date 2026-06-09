using ApiProdutos.Data;

namespace ApiProdutos.Routes;

public static class ROTA_GET
{
    public static void MapGetRoutes(this WebApplication app)
    {
        app.MapGet("/", () =>
        {
            return "API de Materiais Escolares funcionando!";
        });

        app.MapGet("/api/materiais", () =>
        {
            var materiais = BancoSimulado.Materiais
                .OrderBy(m => m.Lotes
                    .Where(l => l.Quantidade > 0)
                    .OrderBy(l => l.DataVencimento ?? DateTime.MaxValue)
                    .Select(l => l.DataVencimento)
                    .FirstOrDefault())
                .ToList();

            return Results.Ok(materiais);
        });

        app.MapGet("/api/materiais/{id}", (int id) =>
        {
            var material = BancoSimulado.Materiais
                .FirstOrDefault(m => m.Id == id);

            if (material == null)
                return Results.NotFound("Material não encontrado.");

            return Results.Ok(material);
        });

        app.MapGet("/api/materiais/{id}/lotes", (int id) =>
        {
            var material = BancoSimulado.Materiais
                .FirstOrDefault(m => m.Id == id);

            if (material == null)
                return Results.NotFound("Material não encontrado.");

            return Results.Ok(material.Lotes);
        });
    }
}