using ApiContratos.Repositorio;
using ApiContratos.models;

namespace ApiContratos.Routes;

public static class ROTA_PUT
{
    public static void MapPutRoutes(this WebApplication app)
    {

        app.MapPut("/api/contratos/{id:int}", (int id, Contrato atualizado) =>
        {
            var contrato = ContratoRepositorio.contratos
                .FirstOrDefault(c => c.Id == id);

            if (contrato == null)
            {
                return Results.NotFound("Contrato não encontrado.");
            }
            else
            {
                contrato.Empresa = atualizado.Empresa;
                contrato.Nome = atualizado.Nome;
                contrato.Valor = atualizado.Valor;
                contrato.Responsavel = atualizado.Responsavel;
                contrato.DataInicio = atualizado.DataInicio;
                contrato.Validade = atualizado.Validade;
                contrato.TipoFinanceiro = atualizado.TipoFinanceiro;
            return Results.Ok(contrato);
            }
        });
    }
}