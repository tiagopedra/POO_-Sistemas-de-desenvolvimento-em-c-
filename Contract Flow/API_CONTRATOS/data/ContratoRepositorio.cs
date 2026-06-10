using ApiContratos.models;

namespace ApiContratos.Repositorio; // Lista compartilhada usada por todas as rotas.
// Funciona como um banco de dados temporário em memória.

public static class ContratoRepositorio
{
    public static List<Contrato> contratos = new List<Contrato>
        {
            new Contrato {Empresa = " TechNova Sistemas ", Id = 1, Nome = " Contrato de suporte técnico ", Valor = 2500.00m, Responsavel = " Carlos Mendes ", DataInicio = DateOnly.FromDateTime(DateTime.Today).AddDays(-120), Validade = DateOnly.FromDateTime(DateTime.Today).AddDays(-15),  TipoFinanceiro = TipoFinanceiro.Entrada },
            new Contrato {Empresa = " Alfa Contabilidade ", Id = 2, Nome = " Contrato de prestação de serviços contábeis ", Valor = 1800.00m, Responsavel = " Mariana Souza ", DataInicio = DateOnly.FromDateTime(DateTime.Today).AddDays(-90), Validade = DateOnly.FromDateTime(DateTime.Today).AddDays(-3), TipoFinanceiro = TipoFinanceiro.Saida },
            new Contrato {Empresa = " FastNet Telecom ", Id = 3, Nome = " Contrato de internet corporativa ", Valor = 799.90m, Responsavel = " João Pereira ", DataInicio = DateOnly.FromDateTime(DateTime.Today).AddDays(-60), Validade = DateOnly.FromDateTime(DateTime.Today), TipoFinanceiro = TipoFinanceiro.Saida },
            new Contrato {Empresa = " Clínica Vida Mais ", Id = 4, Nome = " Contrato de manutenção de sistema ", Valor = 1200.00m, Responsavel = " Ana Ribeiro ", DataInicio = DateOnly.FromDateTime(DateTime.Today).AddDays(-80), Validade = DateOnly.FromDateTime(DateTime.Today).AddDays(2), TipoFinanceiro = TipoFinanceiro.Entrada },
            new Contrato {Empresa = " Mercado Bom Preço ", Id = 5, Nome = " Contrato de fornecimento de software ", Valor = 3500.00m, Responsavel = " Lucas Almeida ", DataInicio = DateOnly.FromDateTime(DateTime.Today).AddDays(-45), Validade = DateOnly.FromDateTime(DateTime.Today).AddDays(6), TipoFinanceiro = TipoFinanceiro.Entrada },
            new Contrato {Empresa = " Escola Futuro Digital ", Id = 6, Nome = " Contrato de licença educacional ", Valor = 4200.00m, Responsavel = " Fernanda Lima ", DataInicio = DateOnly.FromDateTime(DateTime.Today).AddDays(-30), Validade = DateOnly.FromDateTime(DateTime.Today).AddDays(10), TipoFinanceiro = TipoFinanceiro.Entrada },
            new Contrato {Empresa = " Construtora Horizonte ", Id = 7, Nome = " Contrato de consultoria administrativa ", Valor = 5100.00m, Responsavel = " Roberto Martins ", DataInicio = DateOnly.FromDateTime(DateTime.Today).AddDays(-20), Validade = DateOnly.FromDateTime(DateTime.Today).AddDays(20), TipoFinanceiro = TipoFinanceiro.Entrada },
            new Contrato {Empresa = " Loja Estilo Urbano ", Id = 8, Nome = " Contrato de marketing digital ", Valor = 2200.00m, Responsavel = " Patrícia Gomes ",  DataInicio = DateOnly.FromDateTime(DateTime.Today).AddDays(-15), Validade = DateOnly.FromDateTime(DateTime.Today).AddDays(30), TipoFinanceiro = TipoFinanceiro.Saida },
            new Contrato {Empresa = " Auto Peças Brasil ", Id = 9, Nome = " Contrato de sistema de estoque ", Valor = 3100.00m, Responsavel = " Eduardo Costa ", DataInicio = DateOnly.FromDateTime(DateTime.Today).AddDays(-10), Validade = DateOnly.FromDateTime(DateTime.Today).AddDays(45), TipoFinanceiro = TipoFinanceiro.Entrada },
            new Contrato {Empresa = " Hotel Central Palace ", Id = 10, Nome = " Contrato de suporte e hospedagem web ", Valor = 1600.00m, Responsavel = " Juliana Rocha ", DataInicio = DateOnly.FromDateTime(DateTime.Today).AddDays(-5), Validade = DateOnly.FromDateTime(DateTime.Today).AddDays(60), TipoFinanceiro = TipoFinanceiro.Entrada }
            
        };
}