using ApiContratos.Repositorio;
using ApiContratos.models;

namespace ApiContratos.Routes;

public static class ROTA_POST
{
    public static void MapPostRoutes(this WebApplication app)
    {

        app.MapPost("/api/contratos", (Contrato novo) =>
        {

                if (ContratoRepositorio.contratos.Any())
            {
                // Se já existir contrato, pega o maior Id da lista e soma +1
                novo.Id = ContratoRepositorio.contratos.Max(c => c.Id) + 1;
            }
            else
            {
                // Se a lista estiver vazia, o primeiro contrato recebe Id 1
                novo.Id = 1;
            }

                ContratoRepositorio.contratos.Add(novo);
            return Results.Created($"/api/contratos/{novo.Id}", novo);
        });
    }
}