using ApidoZion.Models;
using ApidoZion.Services;
namespace ApidoZion.Services;

public static class ServicoDeVendas
{

    public static double CalcularFaturamentoTotal(List<Venda> vendas)
    {
        return vendas.Sum(v => v.ValorTotalVenda);
    }

    public static double CalcularLucroTotal(List<Venda> vendas)
    {
        return vendas.Sum(v => v.LucroLiquido);
    }


    public static double CalcularCustoTotal(List<Venda> vendas, List<Peca> pecas)
    {
        double custoTotal = 0;

        foreach (var venda in vendas)
        {
            var peca = pecas.FirstOrDefault(p => p.Id == venda.PecaId);
            if (peca != null)
            {
                custoTotal += peca.CustoFornecedor * venda.QuantidadeVendida;
            }
        }

        return custoTotal;
    }

    public static double CalcularMargemLucro(List<Venda> vendas)
    {
        double faturamento = CalcularFaturamentoTotal(vendas);

        if (faturamento == 0)
            return 0;

        double lucro = CalcularLucroTotal(vendas);
        return (lucro / faturamento) * 100;
    }


    public static double CalcularTicketMedio(List<Venda> vendas)
    {
        if (vendas.Count == 0)
            return 0;

        double faturamento = CalcularFaturamentoTotal(vendas);
        return faturamento / vendas.Count;
    }

    public static int CalcularVolumePecasVendidas(List<Venda> vendas)
    {
        return vendas.Sum(v => v.QuantidadeVendida);
    }

    public static string ObterProdutoMaisVendido(List<Venda> vendas, List<Peca> pecas)
    {
        if (vendas.Count == 0)
            return "N/A";

        var agrupadoPorPeca = vendas
            .GroupBy(v => v.PecaId)
            .OrderByDescending(g => g.Sum(v => v.QuantidadeVendida))
            .FirstOrDefault();

        if (agrupadoPorPeca == null)
            return "N/A";

        var peca = pecas.FirstOrDefault(p => p.Id == agrupadoPorPeca.Key);
        return peca?.NomePeca ?? "N/A";
    }

    public static string ObterProdutoMenosVendido(List<Venda> vendas, List<Peca> pecas)
    {
        if (vendas.Count == 0)
            return "N/A";

        var agrupadoPorPeca = vendas
            .GroupBy(v => v.PecaId)
            .OrderBy(g => g.Sum(v => v.QuantidadeVendida))
            .FirstOrDefault();

        if (agrupadoPorPeca == null)
            return "N/A";

        var peca = pecas.FirstOrDefault(p => p.Id == agrupadoPorPeca.Key);
        return peca?.NomePeca ?? "N/A";
    }


    public static int CalcularQuantidadeClientesAtivos(List<Cliente> clientes)
    {
        return clientes.Count(c => c.Status == "online");
    }


    public static Fornecedor ObterFornecedorMaisUtilizado(List<Venda> vendas, List<Peca> pecas, List<Fornecedor> fornecedores)
    {
        if (vendas.Count == 0)
            return null;

        // Agrupa vendas por fornecedor
        var fornecedorMaisUsado = vendas
            .Select(v => pecas.FirstOrDefault(p => p.Id == v.PecaId)?.FornecedorId)
            .Where(f => f != null)
            .GroupBy(f => f)
            .OrderByDescending(g => g.Count())
            .FirstOrDefault();

        if (fornecedorMaisUsado == null)
            return null;

        return fornecedores.FirstOrDefault(f => f.Id == fornecedorMaisUsado.Key);
    }

    public static double CalcularLucroPorPeca(List<Venda> vendas, string pecaId)
    {
        return vendas
            .Where(v => v.PecaId == pecaId)
            .Sum(v => v.LucroLiquido);
    }

    public static double CalcularLucroPorCliente(List<Venda> vendas, string clienteId)
    {
        return vendas
            .Where(v => v.ClienteId == clienteId)
            .Sum(v => v.LucroLiquido);
    }

    public static double CalcularFaturamentoPorFornecedor(List<Venda> vendas, List<Peca> pecas, string fornecedorId)
    {
        return vendas
            .Where(v => pecas.FirstOrDefault(p => p.Id == v.PecaId)?.FornecedorId == fornecedorId)
            .Sum(v => v.ValorTotalVenda);
    }

    public static string ObterPecaMaisLucrativa(List<Venda> vendas, List<Peca> pecas)
    {
        if (vendas.Count == 0)
            return "N/A";

        var pecaMaisLucrativa = vendas
            .GroupBy(v => v.PecaId)
            .OrderByDescending(g => g.Sum(v => v.LucroLiquido))
            .FirstOrDefault();

        if (pecaMaisLucrativa == null)
            return "N/A";

        var peca = pecas.FirstOrDefault(p => p.Id == pecaMaisLucrativa.Key);
        return peca?.NomePeca ?? "N/A";
    }

    public static double CalcularMargemVenda(Venda venda)
    {
        if (venda.ValorTotalVenda == 0)
            return 0;

        return (venda.LucroLiquido / venda.ValorTotalVenda) * 100;
    }

    public static Dictionary<string, object> GerarResumoPerformance(List<Venda> vendas, List<Peca> pecas, List<Cliente> clientes, List<Fornecedor> fornecedores)
    {
        return new Dictionary<string, object>
        {
            { "TotalFaturamento", CalcularFaturamentoTotal(vendas) },
            { "TotalLucro", CalcularLucroTotal(vendas) },
            { "TotalCusto", CalcularCustoTotal(vendas, pecas) },
            { "MargemLucro", CalcularMargemLucro(vendas) },
            { "TicketMedio", CalcularTicketMedio(vendas) },
            { "TotalVendas", vendas.Count },
            { "VolumePecas", CalcularVolumePecasVendidas(vendas) },
            { "ClientesAtivos", CalcularQuantidadeClientesAtivos(clientes) },
            { "ProdutoMaisVendido", ObterProdutoMaisVendido(vendas, pecas) },
            { "ProdutoMenosVendido", ObterProdutoMenosVendido(vendas, pecas) },
            { "PecaMaisLucrativa", ObterPecaMaisLucrativa(vendas, pecas) },
            { "FornecedorMaisUtilizado", ObterFornecedorMaisUtilizado(vendas, pecas, fornecedores)?.Nome_RazaoSocial ?? "N/A" }
        };
    }
}