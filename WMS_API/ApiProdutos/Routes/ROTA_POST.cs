using ApiProdutos.Data;
using ApiProdutos.Models;

namespace ApiProdutos.Routes;

public static class ROTA_POST
{
    public static void MapPostRoutes(this WebApplication app)
    {
        app.MapPost("/api/materiais", (MaterialEscolar material) =>
        {
            BancoSimulado.Materiais.Add(material);

            return Results.Created(
                $"/api/materiais/{material.Id}",
                material
            );
        });
    }
}