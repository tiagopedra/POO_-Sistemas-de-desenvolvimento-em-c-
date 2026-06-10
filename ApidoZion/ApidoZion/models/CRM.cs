namespace ApidoZion.Models;

public class DadosGerais
{
    public string Id { get; set; }
    public string Nome_RazaoSocial { get; set; }
    public string CPF_CNPJ { get; set; }
    public string Endereco { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
}

public class Cliente : DadosGerais
{
    public string Status { get; set; }  // "online" // "offline" // "sleepy"
    public double TotalGasto { get; set; }
    public double TotalLucroBullDogs { get; set; }
    public int QuantidadeCompras { get; set; }
    public string PecaMaisComprada { get; set; }
    public int ComprasNoUltimoMes { get; set; }
}

public class Fornecedor : DadosGerais
{
    public string Status { get; set; }  // "online" , "offline" , "sleepy"
    public string Pecas { get; set; }  // tipo de peça que fornece
    public int Quantidade { get; set; }
    public double Valor { get; set; }
}

public class Peca
{
    public string Id { get; set; }
    public string NomePeca { get; set; }
    public string Categoria { get; set; }  // "GPU", "CPU", "RAM", "SSD", "Cooler"
    public string Marca { get; set; }
    public string FornecedorId { get; set; }
    public double CustoFornecedor { get; set; }
    public double PrecoVenda { get; set; }
    public double MargemLucro { get; set; }
    public int QuantidadeVendida { get; set; }
    public int QuantidadeReposicoes { get; set; }
    public double LucroTotal { get; set; }
    public int RankingVendas { get; set; }
    public bool EhMaisVendida { get; set; }
    public bool EhMenosVendida { get; set; }
}

public class Venda
{
    public string Id { get; set; }
    public string NumeroNota { get; set; }
    public string ClienteId { get; set; }
    public string PecaId { get; set; }
    public int QuantidadeVendida { get; set; }
    public double PrecoUnitarioVenda { get; set; }
    public double DescontoAplicado { get; set; }
    public double PrecoComDesconto { get; set; }
    public double ValorTotalVenda { get; set; }
    public double LucroLiquido { get; set; }
    public double PercentualLucro { get; set; }
}