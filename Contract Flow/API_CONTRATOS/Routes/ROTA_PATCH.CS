using ApiContratos.models;
using ApiContratos.Repositorio;

namespace ApiContratos.Routes;

public static class ROTA_PATCH
{
    public static void MapPatchRoutes(this WebApplication app)
    {
        app.MapPatch("/api/contratos/{id:int}", (int id, ContratoPatchDTO parcial) =>
        {
            var contrato = ContratoRepositorio.contratos
                .FirstOrDefault(c => c.Id == id);

            if (contrato == null)
            {
                return Results.NotFound("Contrato não encontrado.");
            }

            DateOnly novaDataInicio = parcial.DataInicio ?? contrato.DataInicio;
            DateOnly novaValidade = parcial.Validade ?? contrato.Validade;

            if (novaValidade < novaDataInicio)
            {
                return Results.BadRequest("A validade não pode ser menor que a data de início.");
            }

            if (!string.IsNullOrWhiteSpace(parcial.Empresa))
            {
                contrato.Empresa = parcial.Empresa.Trim();
            }

            if (!string.IsNullOrWhiteSpace(parcial.Nome))
            {
                contrato.Nome = parcial.Nome.Trim();
            }

            if (parcial.Valor.HasValue)
            {
                if (parcial.Valor.Value <= 0)
                {
                    return Results.BadRequest("O valor do contrato deve ser maior que zero.");
                }

                contrato.Valor = parcial.Valor.Value;
            }

            if (!string.IsNullOrWhiteSpace(parcial.Responsavel))
            {
                contrato.Responsavel = parcial.Responsavel.Trim();
            }

            if (parcial.DataInicio.HasValue)
            {
                contrato.DataInicio = parcial.DataInicio.Value;
            }

            if (parcial.Validade.HasValue)
            {
                contrato.Validade = parcial.Validade.Value;
            }

            if (parcial.TipoFinanceiro.HasValue)
            {
                contrato.TipoFinanceiro = parcial.TipoFinanceiro.Value;
            }

            return Results.Ok(contrato);
        });
    }
}