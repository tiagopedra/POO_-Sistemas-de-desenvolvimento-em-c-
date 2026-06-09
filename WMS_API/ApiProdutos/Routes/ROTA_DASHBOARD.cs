using ApiProdutos.Data;

namespace ApiProdutos.Routes;

public static class ROTA_DASHBOARD
{
    public static void MapDashboardRoutes(this WebApplication app)
    {
        app.MapGet("/api/dashboard", () =>
        {
            var materiais = BancoSimulado.Materiais;

            var totalMateriais = materiais.Count;
            var totalAtivos = materiais.Count(m => m.Ativo);
            var totalInativos = materiais.Count(m => !m.Ativo);

            var totalUnidadesEstoque = materiais
                .SelectMany(m => m.Lotes)
                .Sum(l => l.Quantidade);

            var valorInvestido = materiais
                .SelectMany(m => m.Lotes)
                .Sum(l => l.Quantidade * l.ValorCusto);

            var valorVendaEstimado = materiais
                .SelectMany(m => m.Lotes)
                .Sum(l => l.Quantidade * l.ValorVenda);

            var lucroEstimado = valorVendaEstimado - valorInvestido;

            var roiPercentual = valorInvestido > 0
                ? (lucroEstimado / valorInvestido) * 100
                : 0;

            var materiaisDetalhados = materiais.Select(m => new
            {
                m.Id,
                m.Nome,
                m.Categoria,
                m.Ativo,
                EstoqueTotal = m.Lotes.Sum(l => l.Quantidade),
                ValorInvestido = m.Lotes.Sum(l => l.Quantidade * l.ValorCusto),
                ValorVendaEstimado = m.Lotes.Sum(l => l.Quantidade * l.ValorVenda),
                LucroEstimado = m.Lotes.Sum(l => l.Quantidade * l.ValorVenda) -
                                m.Lotes.Sum(l => l.Quantidade * l.ValorCusto),
                RoiPercentual = m.Lotes.Sum(l => l.Quantidade * l.ValorCusto) > 0
                    ? (
                        (
                            m.Lotes.Sum(l => l.Quantidade * l.ValorVenda) -
                            m.Lotes.Sum(l => l.Quantidade * l.ValorCusto)
                        )
                        / m.Lotes.Sum(l => l.Quantidade * l.ValorCusto)
                      ) * 100
                    : 0
            });

            return Results.Ok(new
            {
                TotalMateriais = totalMateriais,
                TotalAtivos = totalAtivos,
                TotalInativos = totalInativos,
                TotalUnidadesEstoque = totalUnidadesEstoque,
                ValorInvestido = valorInvestido,
                ValorVendaEstimado = valorVendaEstimado,
                LucroEstimado = lucroEstimado,
                RoiPercentual = roiPercentual,
                Materiais = materiaisDetalhados
            });
        });
    }
}