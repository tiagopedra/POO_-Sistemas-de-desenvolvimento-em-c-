using ApiProdutos.Models;

namespace ApiProdutos.Data;

public static class BancoSimulado
{
    public static List<MaterialEscolar> Materiais = new()
    {
        new MaterialEscolar
        {
            Id = 1,
            Nome = "Caderno Universitário",
            Categoria = "Caderno",
            Ativo = true,
            Lotes = new List<Lote>
            {
                new Lote
                {
                    Id = 1,
                    Codigo = "CAD001",
                    Quantidade = 50,
                    DataEntrada = new DateTime(2026, 1, 10),
                    DataVencimento = null,
                    ValorCusto = 8.50m,
                    ValorVenda = 14.90m
                }
            }
        },

        new MaterialEscolar
        {
            Id = 2,
            Nome = "Cola Branca",
            Categoria = "Cola",
            Ativo = true,
            Lotes = new List<Lote>
            {
                new Lote
                {
                    Id = 2,
                    Codigo = "COL001",
                    Quantidade = 30,
                    DataEntrada = new DateTime(2026, 1, 15),
                    DataVencimento = new DateTime(2027, 5, 20),
                    ValorCusto = 3.20m,
                    ValorVenda = 6.50m
                }
            }
        },

        new MaterialEscolar
        {
            Id = 3,
            Nome = "Caneta Azul",
            Categoria = "Caneta",
            Ativo = true,
            Lotes = new List<Lote>
            {
                new Lote
                {
                    Id = 3,
                    Codigo = "CAN001",
                    Quantidade = 100,
                    DataEntrada = new DateTime(2026, 2, 10),
                    DataVencimento = null,
                    ValorCusto = 1.10m,
                    ValorVenda = 2.50m
                }
            }
        },

        new MaterialEscolar
        {
            Id = 4,
            Nome = "Lápis Grafite",
            Categoria = "Lápis",
            Ativo = true,
            Lotes = new List<Lote>
            {
                new Lote
                {
                    Id = 4,
                    Codigo = "LAP001",
                    Quantidade = 120,
                    DataEntrada = new DateTime(2026, 2, 18),
                    DataVencimento = null,
                    ValorCusto = 0.80m,
                    ValorVenda = 1.80m
                }
            }
        },

        new MaterialEscolar
        {
            Id = 5,
            Nome = "Borracha Branca",
            Categoria = "Borracha",
            Ativo = true,
            Lotes = new List<Lote>
            {
                new Lote
                {
                    Id = 5,
                    Codigo = "BOR001",
                    Quantidade = 75,
                    DataEntrada = new DateTime(2026, 3, 1),
                    DataVencimento = null,
                    ValorCusto = 0.90m,
                    ValorVenda = 2.00m
                }
            }
        },

        new MaterialEscolar
        {
            Id = 6,
            Nome = "Apontador Simples",
            Categoria = "Apontador",
            Ativo = true,
            Lotes = new List<Lote>
            {
                new Lote
                {
                    Id = 6,
                    Codigo = "APO001",
                    Quantidade = 60,
                    DataEntrada = new DateTime(2026, 3, 5),
                    DataVencimento = null,
                    ValorCusto = 1.20m,
                    ValorVenda = 3.00m
                }
            }
        },

        new MaterialEscolar
        {
            Id = 7,
            Nome = "Régua 30cm",
            Categoria = "Régua",
            Ativo = true,
            Lotes = new List<Lote>
            {
                new Lote
                {
                    Id = 7,
                    Codigo = "REG001",
                    Quantidade = 40,
                    DataEntrada = new DateTime(2026, 3, 10),
                    DataVencimento = null,
                    ValorCusto = 2.30m,
                    ValorVenda = 5.00m
                }
            }
        },

        new MaterialEscolar
        {
            Id = 8,
            Nome = "Tesoura Escolar",
            Categoria = "Tesoura",
            Ativo = true,
            Lotes = new List<Lote>
            {
                new Lote
                {
                    Id = 8,
                    Codigo = "TES001",
                    Quantidade = 35,
                    DataEntrada = new DateTime(2026, 3, 12),
                    DataVencimento = null,
                    ValorCusto = 4.80m,
                    ValorVenda = 9.90m
                }
            }
        },

        new MaterialEscolar
        {
            Id = 9,
            Nome = "Estojo Escolar",
            Categoria = "Estojo",
            Ativo = true,
            Lotes = new List<Lote>
            {
                new Lote
                {
                    Id = 9,
                    Codigo = "EST001",
                    Quantidade = 25,
                    DataEntrada = new DateTime(2026, 3, 15),
                    DataVencimento = null,
                    ValorCusto = 7.50m,
                    ValorVenda = 15.00m
                }
            }
        },

        new MaterialEscolar
        {
            Id = 10,
            Nome = "Tinta Guache",
            Categoria = "Tinta",
            Ativo = true,
            Lotes = new List<Lote>
            {
                new Lote
                {
                    Id = 10,
                    Codigo = "TIN001",
                    Quantidade = 45,
                    DataEntrada = new DateTime(2026, 3, 20),
                    DataVencimento = new DateTime(2027, 8, 30),
                    ValorCusto = 5.90m,
                    ValorVenda = 11.90m
                }
            }
        },

        new MaterialEscolar
        {
            Id = 11,
            Nome = "Massa de Modelar",
            Categoria = "Massa",
            Ativo = true,
            Lotes = new List<Lote>
            {
                new Lote
                {
                    Id = 11,
                    Codigo = "MAS001",
                    Quantidade = 55,
                    DataEntrada = new DateTime(2026, 3, 25),
                    DataVencimento = new DateTime(2027, 6, 15),
                    ValorCusto = 3.70m,
                    ValorVenda = 8.50m
                }
            }
        },

        new MaterialEscolar
        {
            Id = 12,
            Nome = "Caneta Vermelha",
            Categoria = "Caneta",
            Ativo = false,
            Lotes = new List<Lote>
            {
                new Lote
                {
                    Id = 12,
                    Codigo = "CANVER001",
                    Quantidade = 20,
                    DataEntrada = new DateTime(2026, 4, 1),
                    DataVencimento = null,
                    ValorCusto = 1.10m,
                    ValorVenda = 2.50m
                }
            }
        }
    };
}