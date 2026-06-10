using ApiContratos.Repositorio;

namespace ApiContratos.Routes;

public static class ROTA_DELETE
{
    public static void MapDeleteRoutes(this WebApplication app)
    {

        app.MapDelete("/api/contratos/{id:int}", (int id) =>
        {
            var contrato = ContratoRepositorio.contratos
                .FirstOrDefault(c => c.Id == id);

            if (contrato is null)
            {
                return Results.NotFound("Contrato não encontrado.");
            }

            ContratoRepositorio.contratos.Remove(contrato);

            return Results.NoContent();
        });
    }
}