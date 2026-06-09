using ApiProdutos.Data;
using ApiProdutos.Models;

namespace ApiProdutos.Routes;

public static class ROTA_SAIDA
{
    public static void MapSaidaRoutes(this WebApplication app)
    {
        app.MapPost("/api/materiais/saida", (SaidaEstoque saida) =>
        {
            var material = BancoSimulado.Materiais
                .FirstOrDefault(m => m.Id == saida.MaterialId);

            if (material == null)
            {
                return Results.NotFound("Material não encontrado.");
            }

            if (saida.Quantidade <= 0)
            {
                return Results.BadRequest("A quantidade de saída deve ser maior que zero.");
            }

            var estoqueTotal = material.Lotes.Sum(l => l.Quantidade);

            if (estoqueTotal < saida.Quantidade)
            {
                return Results.BadRequest("Estoque insuficiente para realizar a saída.");
            }

            var quantidadeRestante = saida.Quantidade;

            var lotesOrdenados = material.Lotes
                .Where(l => l.Quantidade > 0)
                .OrderBy(l => l.DataVencimento ?? DateTime.MaxValue)
                .ThenBy(l => l.DataEntrada)
                .ToList();

            foreach (var lote in lotesOrdenados)
            {
                if (quantidadeRestante == 0)
                {
                    break;
                }

                if (lote.Quantidade >= quantidadeRestante)
                {
                    lote.Quantidade -= quantidadeRestante;
                    quantidadeRestante = 0;
                }
                else
                {
                    quantidadeRestante -= lote.Quantidade;
                    lote.Quantidade = 0;
                }
            }

            return Results.Ok(new
            {
                Mensagem = "Saída realizada com sucesso.",
                Material = material.Nome,
                QuantidadeRetirada = saida.Quantidade,
                LotesAtualizados = material.Lotes
            });
        });
    }
}