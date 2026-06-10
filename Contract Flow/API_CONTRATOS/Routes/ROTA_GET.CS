using ApiContratos.Repositorio;
using ApiContratos.models;

namespace ApiContratos.Routes;

public static class ROTA_GET
{
    public static void MapGetRoutes(this WebApplication app)
    {
        // app.MapGet("/", () => "API de Contratos em funcionamento!"); 
        // ↑ comentada para o index.html funcionar

        app.MapGet("/api/contratos", () => {
            return Results.Ok(ContratoRepositorio.contratos);
        });

        app.MapGet("/api/contratos/ativos", () =>
        {
            var contratosAtivos = ContratoRepositorio.contratos.Where(c => c.Status == StatusContrato.Ativo || c.Status == StatusContrato.PertoDeVencer).ToList();
            return Results.Ok(contratosAtivos);
        });

        app.MapGet("/api/contratos/inativos", () =>
        {
            var contratosInativos = ContratoRepositorio.contratos.Where(c => c.Status == StatusContrato.Vencido).ToList();
            return Results.Ok(contratosInativos);
        });
        app.MapGet("/api/contratos/pertodevencer", () =>
        {
            var contratosInativos = ContratoRepositorio.contratos.Where(c => c.Status == StatusContrato.PertoDeVencer).ToList();
            return Results.Ok(contratosInativos);
        });

        app.MapGet("/api/contratos/{id:int}", (int id) =>
        {
            var contrato = ContratoRepositorio.contratos.FirstOrDefault(c => c.Id == id);

            if (contrato != null)
            {
                return Results.Ok(contrato);
            }
            else
            {
                return Results.NotFound("Contrato não encontrado.");
            }
        });
        app.MapGet("/api/contratos/entradas", () =>
{
    var entradas = ContratoRepositorio.contratos
        .Where(c => c.TipoFinanceiro == TipoFinanceiro.Entrada)
        .ToList();

    return Results.Ok(entradas);
});

app.MapGet("/api/contratos/saidas", () =>
{
    var saidas = ContratoRepositorio.contratos
        .Where(c => c.TipoFinanceiro == TipoFinanceiro.Saida)
        .ToList();

    return Results.Ok(saidas);
});

app.MapGet("/api/contratos/resumo-financeiro", () =>
{
    decimal totalEntradas = ContratoRepositorio.contratos
        .Where(c => c.TipoFinanceiro == TipoFinanceiro.Entrada)
        .Sum(c => c.Valor);

    decimal totalSaidas = ContratoRepositorio.contratos
        .Where(c => c.TipoFinanceiro == TipoFinanceiro.Saida)
        .Sum(c => c.Valor);

    decimal saldo = totalEntradas - totalSaidas;

    var resumo = new
    {
        TotalEntradas = totalEntradas,
        TotalSaidas = totalSaidas,
        Saldo = saldo
    };

    return Results.Ok(resumo);
});

    }
}