using ApiProdutos.Models;

namespace ApiProdutos.Data;

public static class BancoSimulado
{
    public static List<MaterialEscolar> Materiais = GerarMateriais();

    private static List<MaterialEscolar> GerarMateriais()
    {
        var nomes = new[]
        {
            "Caderno Universitário",
            "Cola Branca",
            "Caneta Azul",
            "Lápis Grafite",
            "Borracha Branca",
            "Apontador Simples",
            "Régua 30cm",
            "Tesoura Escolar",
            "Estojo Escolar",
            "Tinta Guache",
            "Massa de Modelar",
            "Caneta Vermelha",
            "Caneta Preta",
            "Marca Texto Amarelo",
            "Marca Texto Rosa",
            "Lápis de Cor 12 Cores",
            "Giz de Cera",
            "Papel Sulfite A4",
            "Papel Cartolina",
            "Papel Color Set",
            "Pasta Plástica",
            "Pasta Catálogo",
            "Fichário Escolar",
            "Refil de Fichário",
            "Agenda Escolar",
            "Compasso Escolar",
            "Transferidor",
            "Esquadro",
            "Calculadora Simples",
            "Mochila Escolar",
            "Lancheira Escolar",
            "Garrafa Escolar",
            "Pincel Atômico",
            "Pincel Escolar",
            "Pincel para Quadro Branco",
            "Canetinha Hidrocor",
            "Corretivo Líquido",
            "Corretivo em Fita",
            "Grampeador Escolar",
            "Grampos",
            "Clips",
            "Envelope Pardo",
            "Etiqueta Adesiva",
            "Bloco de Anotações",
            "Caderno de Desenho",
            "Folha Almaço",
            "Papel Vegetal",
            "Papel Crepom",
            "Papel Dobradura",
            "EVA Colorido",
            "Cola Bastão",
            "Cola Colorida",
            "Cola Glitter",
            "Tinta Acrílica",
            "Tinta para Tecido",
            "Massa EVA",
            "Argila Escolar",
            "Barbante",
            "Fita Adesiva",
            "Fita Crepe",
            "Fita Dupla Face",
            "Tesoura Sem Ponta",
            "Estilete Escolar",
            "Prancheta",
            "Quadro Branco Pequeno",
            "Apagador de Quadro",
            "Pincel Permanente",
            "Caneta Gel",
            "Caneta Esferográfica",
            "Caderno Brochura",
            "Caderno Espiral",
            "Caderno Cartografia",
            "Livro Ata",
            "Livro Protocolo",
            "Pasta Sanfonada",
            "Pasta Suspensa",
            "Arquivo Morto",
            "Caixa Organizadora",
            "Porta Lápis",
            "Lapiseira 0.7",
            "Grafite 0.7",
            "Lapiseira 0.5",
            "Grafite 0.5",
            "Borracha Ponteira",
            "Régua Flexível",
            "Mini Grampeador",
            "Furador de Papel",
            "Planner Escolar",
            "Adesivos Escolares",
            "Carimbo Escolar",
            "Almofada para Carimbo",
            "Papel Fotográfico",
            "Plástico para Plastificação",
            "Capa para Encadernação",
            "Espiral para Encadernação",
            "Caneta Brush",
            "Aquarela Escolar",
            "Pincel Chanfrado",
            "Pincel Redondo",
            "Kit Geométrico"
        };

        var categorias = new[]
        {
            "Caderno",
            "Cola",
            "Caneta",
            "Lápis",
            "Borracha",
            "Apontador",
            "Régua",
            "Tesoura",
            "Estojo",
            "Tinta",
            "Massa",
            "Papelaria",
            "Pasta",
            "Organização",
            "Arte",
            "Escrita",
            "Acessório Escolar"
        };

        var materiais = new List<MaterialEscolar>();

        for (int i = 1; i <= 100; i++)
        {
            var nome = nomes[i - 1];
            var categoria = categorias[(i - 1) % categorias.Length];

            var quantidade = 20 + (i * 7) % 130;

            var valorCusto = Math.Round(0.80m + (i * 1.37m) % 25, 2);
            var valorVenda = Math.Round(valorCusto * 1.75m, 2);

            var possuiVencimento =
                categoria == "Cola" ||
                categoria == "Tinta" ||
                categoria == "Massa" ||
                nome.Contains("Corretivo") ||
                nome.Contains("Aquarela") ||
                nome.Contains("Guache");

            var dataEntrada = new DateTime(2026, 1, 1).AddDays(i * 3);

            var lote = new Lote
            {
                Id = i,
                Codigo = GerarCodigoLote(nome, i),
                Quantidade = quantidade,
                DataEntrada = dataEntrada,
                DataVencimento = possuiVencimento ? dataEntrada.AddMonths(14) : null,
                ValorCusto = valorCusto,
                ValorVenda = valorVenda
            };

            var material = new MaterialEscolar
            {
                Id = i,
                Nome = nome,
                Categoria = categoria,
                Ativo = i % 10 != 0,
                Lotes = new List<Lote>
                {
                    lote
                }
            };

            materiais.Add(material);
        }

        return materiais;
    }

    private static string GerarCodigoLote(string nome, int id)
    {
        var prefixo = new string(
            nome
                .Where(char.IsLetter)
                .Take(3)
                .Select(char.ToUpper)
                .ToArray()
        );

        return $"{prefixo}{id.ToString("000")}";
    }
}
