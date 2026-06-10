using ApiVendasHardware.Models;

namespace ApiVendasHardware.Repositories
{
    public class DashboardStats
    {
        public int TotalProdutos       { get; set; }
        public int ProdutosAtivos      { get; set; }
        public int ProdutosInativos    { get; set; }
        public int TotalEstoque        { get; set; }

        // Custo total do estoque atual (não vendido)
        public decimal ValorInvestido  { get; set; }

        // Receita = soma dos PrecoVenda dos itens em pedidos Confirmados
        public decimal ReceitaVendas   { get; set; }

        // Custo = soma dos PrecoCusto dos itens em pedidos Confirmados
        public decimal CustoVendidos   { get; set; }

        // Lucro = Receita - Custo dos vendidos
        public decimal LucroTotal => ReceitaVendas - CustoVendidos;
    }

    public class HardwareRepository
    {
        // ── Clientes ─────────────────────────────────────────────────────────────

        private readonly List<Cliente> _clientes = new()
        {
            new Cliente { Id = 1, Nome = "João Silva",    Email = "joao@email.com",   Telefone = "41999990001", Endereco = "Rua das Flores, 10 - Curitiba/PR" },
            new Cliente { Id = 2, Nome = "Maria Souza",   Email = "maria@email.com",  Telefone = "41999990002", Endereco = "Av. Brasil, 200 - Curitiba/PR"    },
            new Cliente { Id = 3, Nome = "Pedro Almeida", Email = "pedro@email.com",  Telefone = "41999990003", Endereco = "Rua XV, 50 - Curitiba/PR"         },
            new Cliente { Id = 4, Nome = "Lucas Ferreira",   Email = "lucas@email.com",    Telefone = "41999990004", Endereco = "Rua das Palmeiras, 120 - Curitiba/PR" },
            new Cliente { Id = 5, Nome = "Ana Beatriz",      Email = "ana@email.com",      Telefone = "41999990005", Endereco = "Av. Sete de Setembro, 350 - Curitiba/PR" },
            new Cliente { Id = 6, Nome = "Carlos Eduardo",   Email = "carlos@email.com",   Telefone = "41999990006", Endereco = "Rua Marechal Deodoro, 78 - Curitiba/PR" },
            new Cliente { Id = 7, Nome = "Fernanda Lima",    Email = "fernanda@email.com", Telefone = "41999990007", Endereco = "Rua Visconde de Guarapuava, 450 - Curitiba/PR" },
            new Cliente { Id = 8, Nome = "Ricardo Gomes",    Email = "ricardo@email.com",  Telefone = "41999990008", Endereco = "Av. República Argentina, 890 - Curitiba/PR" },
            new Cliente { Id = 9, Nome = "Juliana Martins",  Email = "juliana@email.com",  Telefone = "41999990009", Endereco = "Rua Padre Anchieta, 230 - Curitiba/PR" },
            new Cliente { Id = 10, Nome = "Gabriel Santos",  Email = "gabriel@email.com",  Telefone = "41999990010", Endereco = "Rua João Negrão, 560 - Curitiba/PR" },
            new Cliente { Id = 11, Nome = "Lucas Oliveira", Email = "lucas.oliveira@email.com", Telefone = "41999990011", Endereco = "Rua XV de Novembro, 120 - Curitiba/PR" },
            new Cliente { Id = 12, Nome = "Mariana Costa", Email = "mariana.costa@email.com", Telefone = "41999990012", Endereco = "Rua Marechal Deodoro, 310 - Curitiba/PR" },
            new Cliente { Id = 13, Nome = "Pedro Henrique", Email = "pedro.henrique@email.com", Telefone = "41999990013", Endereco = "Av. Sete de Setembro, 450 - Curitiba/PR" },
            new Cliente { Id = 14, Nome = "Juliana Martins", Email = "juliana.martins@email.com", Telefone = "41999990014", Endereco = "Rua Visconde de Nácar, 890 - Curitiba/PR" },
            new Cliente { Id = 15, Nome = "Rafael Almeida", Email = "rafael.almeida@email.com", Telefone = "41999990015", Endereco = "Rua Padre Anchieta, 154 - Curitiba/PR" },
            new Cliente { Id = 16, Nome = "Fernanda Rocha", Email = "fernanda.rocha@email.com", Telefone = "41999990016", Endereco = "Rua Chile, 420 - Curitiba/PR" },
            new Cliente { Id = 17, Nome = "Bruno Ferreira", Email = "bruno.ferreira@email.com", Telefone = "41999990017", Endereco = "Rua Itupava, 670 - Curitiba/PR" },
            new Cliente { Id = 18, Nome = "Camila Souza", Email = "camila.souza@email.com", Telefone = "41999990018", Endereco = "Rua Alferes Poli, 210 - Curitiba/PR" },
            new Cliente { Id = 19, Nome = "Gustavo Lima", Email = "gustavo.lima@email.com", Telefone = "41999990019", Endereco = "Rua Brigadeiro Franco, 510 - Curitiba/PR" },
            new Cliente { Id = 20, Nome = "Patricia Gomes", Email = "patricia.gomes@email.com", Telefone = "41999990020", Endereco = "Rua Nunes Machado, 340 - Curitiba/PR" },

            new Cliente { Id = 21, Nome = "Thiago Ribeiro", Email = "thiago.ribeiro@email.com", Telefone = "41999990021", Endereco = "Rua Emiliano Perneta, 180 - Curitiba/PR" },
            new Cliente { Id = 22, Nome = "Aline Barbosa", Email = "aline.barbosa@email.com", Telefone = "41999990022", Endereco = "Rua Mateus Leme, 790 - Curitiba/PR" },
            new Cliente { Id = 23, Nome = "Diego Carvalho", Email = "diego.carvalho@email.com", Telefone = "41999990023", Endereco = "Rua Conselheiro Laurindo, 150 - Curitiba/PR" },
            new Cliente { Id = 24, Nome = "Larissa Mendes", Email = "larissa.mendes@email.com", Telefone = "41999990024", Endereco = "Rua Ubaldino do Amaral, 230 - Curitiba/PR" },
            new Cliente { Id = 25, Nome = "Felipe Moraes", Email = "felipe.moraes@email.com", Telefone = "41999990025", Endereco = "Rua Schiller, 440 - Curitiba/PR" },
            new Cliente { Id = 26, Nome = "Tatiane Lopes", Email = "tatiane.lopes@email.com", Telefone = "41999990026", Endereco = "Rua Desembargador Motta, 560 - Curitiba/PR" },
            new Cliente { Id = 27, Nome = "André Silva", Email = "andre.silva@email.com", Telefone = "41999990027", Endereco = "Rua General Carneiro, 730 - Curitiba/PR" },
            new Cliente { Id = 28, Nome = "Renata Dias", Email = "renata.dias@email.com", Telefone = "41999990028", Endereco = "Rua Tibagi, 120 - Curitiba/PR" },
            new Cliente { Id = 29, Nome = "Vinicius Moreira", Email = "vinicius.moreira@email.com", Telefone = "41999990029", Endereco = "Rua Comendador Araújo, 250 - Curitiba/PR" },
            new Cliente { Id = 30, Nome = "Bianca Castro", Email = "bianca.castro@email.com", Telefone = "41999990030", Endereco = "Rua Benjamin Constant, 810 - Curitiba/PR" },

            new Cliente { Id = 31, Nome = "Leonardo Farias", Email = "leonardo.farias@email.com", Telefone = "41999990031", Endereco = "Rua Lamenha Lins, 370 - Curitiba/PR" },
            new Cliente { Id = 32, Nome = "Vanessa Teixeira", Email = "vanessa.teixeira@email.com", Telefone = "41999990032", Endereco = "Rua São Francisco, 220 - Curitiba/PR" },
            new Cliente { Id = 33, Nome = "Marcelo Araujo", Email = "marcelo.araujo@email.com", Telefone = "41999990033", Endereco = "Rua Treze de Maio, 460 - Curitiba/PR" },
            new Cliente { Id = 34, Nome = "Carolina Freitas", Email = "carolina.freitas@email.com", Telefone = "41999990034", Endereco = "Rua Presidente Faria, 540 - Curitiba/PR" },
            new Cliente { Id = 35, Nome = "Eduardo Nogueira", Email = "eduardo.nogueira@email.com", Telefone = "41999990035", Endereco = "Rua Doutor Muricy, 195 - Curitiba/PR" },
            new Cliente { Id = 36, Nome = "Priscila Cardoso", Email = "priscila.cardoso@email.com", Telefone = "41999990036", Endereco = "Rua Carlos de Carvalho, 320 - Curitiba/PR" },
            new Cliente { Id = 37, Nome = "Rodrigo Pereira", Email = "rodrigo.pereira@email.com", Telefone = "41999990037", Endereco = "Rua Augusto Stresser, 620 - Curitiba/PR" },
            new Cliente { Id = 38, Nome = "Beatriz Melo", Email = "beatriz.melo@email.com", Telefone = "41999990038", Endereco = "Rua Holanda, 150 - Curitiba/PR" },
            new Cliente { Id = 39, Nome = "Henrique Tavares", Email = "henrique.tavares@email.com", Telefone = "41999990039", Endereco = "Rua Almirante Tamandaré, 780 - Curitiba/PR" },
            new Cliente { Id = 40, Nome = "Natália Campos", Email = "natalia.campos@email.com", Telefone = "41999990040", Endereco = "Rua Castro Alves, 410 - Curitiba/PR" },

            new Cliente { Id = 41, Nome = "João Pedro Santos", Email = "joaopedro.santos@email.com", Telefone = "41999990041", Endereco = "Rua Anita Garibaldi, 530 - Curitiba/PR" },
            new Cliente { Id = 42, Nome = "Gabriela Fernandes", Email = "gabriela.fernandes@email.com", Telefone = "41999990042", Endereco = "Rua dos Funcionários, 260 - Curitiba/PR" },
            new Cliente { Id = 43, Nome = "Caio Rodrigues", Email = "caio.rodrigues@email.com", Telefone = "41999990043", Endereco = "Rua Bom Jesus, 490 - Curitiba/PR" },
            new Cliente { Id = 44, Nome = "Amanda Vieira", Email = "amanda.vieira@email.com", Telefone = "41999990044", Endereco = "Rua Paulo Graeser, 640 - Curitiba/PR" },
            new Cliente { Id = 45, Nome = "Matheus Cunha", Email = "matheus.cunha@email.com", Telefone = "41999990045", Endereco = "Rua Francisco Rocha, 210 - Curitiba/PR" },
            new Cliente { Id = 46, Nome = "Letícia Ribeiro", Email = "leticia.ribeiro@email.com", Telefone = "41999990046", Endereco = "Rua Professor Brandão, 320 - Curitiba/PR" },
            new Cliente { Id = 47, Nome = "Daniel Alves", Email = "daniel.alves@email.com", Telefone = "41999990047", Endereco = "Rua Chile, 710 - Curitiba/PR" },
            new Cliente { Id = 48, Nome = "Isabela Pinto", Email = "isabela.pinto@email.com", Telefone = "41999990048", Endereco = "Rua Padre Germano Mayer, 550 - Curitiba/PR" },
            new Cliente { Id = 49, Nome = "Alexandre Machado", Email = "alexandre.machado@email.com", Telefone = "41999990049", Endereco = "Rua Fagundes Varela, 430 - Curitiba/PR" },
            new Cliente { Id = 50, Nome = "Larissa Oliveira", Email = "larissa.oliveira@email.com", Telefone = "41999990050", Endereco = "Rua Nilo Peçanha, 820 - Curitiba/PR" },
                    };

        private int _proximoIdCliente = 11;

        // ── Produtos ──────────────────────────────────────────────────────────────

        private readonly List<Produto> _produtos = new()
        {
            new Produto { Id = 1,  Nome = "Processador Intel Core i9-13900K", Categoria = "CPU",
                          Fabricante = "Intel",    Modelo = "Core i9-13900K",
                          PrecoVenda = 3299.99m,   PrecoCusto = 2100.00m, EstoqueDisponivel = 15,
                          Descricao = "24 núcleos (8P+16E), boost até 5.8GHz." },

            new Produto { Id = 2,  Nome = "GPU NVIDIA GeForce RTX 4080 Super", Categoria = "GPU",
                          Fabricante = "NVIDIA",   Modelo = "RTX 4080 Super",
                          PrecoVenda = 5499.99m,   PrecoCusto = 3800.00m, EstoqueDisponivel = 8,
                          Descricao = "16GB GDDR6X, Ray Tracing, DLSS 3." },

            new Produto { Id = 3,  Nome = "Memória RAM DDR5 32GB 6000MHz", Categoria = "RAM",
                          Fabricante = "Corsair",  Modelo = "Vengeance DDR5-6000",
                          PrecoVenda = 899.90m,    PrecoCusto = 550.00m, EstoqueDisponivel = 30,
                          Descricao = "Kit 2x16GB, XMP 3.0, latência CL36." },

            new Produto { Id = 4,  Nome = "SSD NVMe 2TB PCIe 4.0", Categoria = "SSD",
                          Fabricante = "Samsung",  Modelo = "990 Pro 2TB",
                          PrecoVenda = 1149.90m,   PrecoCusto = 720.00m, EstoqueDisponivel = 22,
                          Descricao = "Leitura sequencial de 7.450 MB/s." },

            new Produto { Id = 5,  Nome = "Placa-Mãe Z790 ATX DDR5", Categoria = "Motherboard",
                          Fabricante = "ASUS",     Modelo = "ROG Strix Z790-E",
                          PrecoVenda = 2799.00m,   PrecoCusto = 1800.00m, EstoqueDisponivel = 10,
                          Descricao = "LGA1700, PCIe 5.0, Wi-Fi 6E integrado." },

            new Produto { Id = 6,  Nome = "Ryzen 7 7700X", Categoria = "Processador",
                          Fabricante = "AMD",      Modelo = "7700X",
                          PrecoVenda = 2299.00m,   PrecoCusto = 1650.00m, EstoqueDisponivel = 15,
                          Descricao = "Processador de 8 núcleos e 16 threads, soquete AM5, até 5.4 GHz." },

            new Produto { Id = 7,  Nome = "Core i7-14700K", Categoria = "Processador",
                          Fabricante = "Intel",    Modelo = "14700K",
                          PrecoVenda = 2899.00m,   PrecoCusto = 2150.00m, EstoqueDisponivel = 12,
                          Descricao = "Processador de alto desempenho com 20 núcleos, soquete LGA1700." },

            new Produto { Id = 8,  Nome = "B650M Aorus Elite", Categoria = "Placa-Mãe",
                          Fabricante = "Gigabyte", Modelo = "B650M",
                          PrecoVenda = 1299.00m,   PrecoCusto = 890.00m, EstoqueDisponivel = 18,
                          Descricao = "Placa-mãe AM5 com suporte a DDR5, PCIe 4.0 e M.2 NVMe." },

            new Produto { Id = 9,  Nome = "Kingston Fury Beast 32GB", Categoria = "Memória RAM",
                          Fabricante = "Kingston", Modelo = "DDR5 6000MHz",
                          PrecoVenda = 899.00m,    PrecoCusto = 620.00m, EstoqueDisponivel = 25,
                          Descricao = "Kit de memória DDR5 32GB (2x16GB), frequência de 6000 MHz." },

            new Produto { Id = 10, Nome = "Samsung 990 Pro 1TB", Categoria = "SSD",
                          Fabricante = "Samsung",  Modelo = "990 Pro",
                          PrecoVenda = 999.00m,    PrecoCusto = 710.00m, EstoqueDisponivel = 20,
                          Descricao = "SSD NVMe PCIe 4.0 de 1TB com velocidades de leitura de até 7450 MB/s." },

            new Produto { Id = 11, Nome = "Corsair RM750e", Categoria = "Fonte",
                          Fabricante = "Corsair",  Modelo = "RM750e",
                          PrecoVenda = 749.00m,    PrecoCusto = 520.00m, EstoqueDisponivel = 14,
                          Descricao = "Fonte ATX de 750W, certificação 80 Plus Gold e cabos modulares." },

            new Produto { Id = 12, Nome = "RTX 4060 Ti", Categoria = "GPU",
                          Fabricante = "ASUS",     Modelo = "Dual OC",
                          PrecoVenda = 3299.00m,   PrecoCusto = 2450.00m, EstoqueDisponivel = 9,
                          Descricao = "Placa de vídeo com 8GB GDDR6, suporte a Ray Tracing e DLSS 3." },

                          new Produto { Id = 13, Nome = "RTX 4070", Categoria = "GPU",
              Fabricante = "Gigabyte", Modelo = "Windforce OC",
              PrecoVenda = 4299.00m, PrecoCusto = 3350.00m, EstoqueDisponivel = 7,
              Descricao = "Placa de vídeo com 12GB GDDR6X para jogos em 1440p." },

            new Produto { Id = 14, Nome = "RTX 4070 Super", Categoria = "GPU",
              Fabricante = "MSI", Modelo = "Ventus 2X",
              PrecoVenda = 4999.00m, PrecoCusto = 3850.00m, EstoqueDisponivel = 6,
              Descricao = "GPU avançada com DLSS 3 e Ray Tracing aprimorado." },

            new Produto { Id = 15, Nome = "RX 7700 XT", Categoria = "GPU",
              Fabricante = "Sapphire", Modelo = "Pulse",
              PrecoVenda = 3599.00m, PrecoCusto = 2800.00m, EstoqueDisponivel = 8,
              Descricao = "Placa AMD com 12GB GDDR6 e excelente desempenho." },

            new Produto { Id = 16, Nome = "RX 7800 XT", Categoria = "GPU",
              Fabricante = "XFX", Modelo = "Merc 319",
              PrecoVenda = 4399.00m, PrecoCusto = 3450.00m, EstoqueDisponivel = 5,
              Descricao = "GPU AMD para jogos em alta resolução." },

            new Produto { Id = 17, Nome = "Core i5-14600K", Categoria = "Processador",
              Fabricante = "Intel", Modelo = "14ª Geração",
              PrecoVenda = 1899.00m, PrecoCusto = 1450.00m, EstoqueDisponivel = 14,
              Descricao = "Processador de 14 núcleos para alto desempenho." },

            new Produto { Id = 18, Nome = "Core i7-14700K", Categoria = "Processador",
              Fabricante = "Intel", Modelo = "14ª Geração",
              PrecoVenda = 2899.00m, PrecoCusto = 2250.00m, EstoqueDisponivel = 10,
              Descricao = "Processador premium para produtividade e jogos." },

            new Produto { Id = 19, Nome = "Ryzen 5 7600X", Categoria = "Processador",
              Fabricante = "AMD", Modelo = "Zen 4",
              PrecoVenda = 1499.00m, PrecoCusto = 1150.00m, EstoqueDisponivel = 16,
              Descricao = "CPU AMD AM5 com excelente custo-benefício." },

            new Produto { Id = 20, Nome = "Ryzen 7 7800X3D", Categoria = "Processador",
              Fabricante = "AMD", Modelo = "Zen 4 3D",
              PrecoVenda = 2699.00m, PrecoCusto = 2100.00m, EstoqueDisponivel = 9,
              Descricao = "Processador otimizado para jogos com 3D V-Cache." },

            new Produto { Id = 21, Nome = "B760M DS3H", Categoria = "Placa-Mãe",
              Fabricante = "Gigabyte", Modelo = "DDR5",
              PrecoVenda = 899.00m, PrecoCusto = 670.00m, EstoqueDisponivel = 18,
              Descricao = "Placa-mãe Intel LGA1700 compatível com DDR5." },

            new Produto { Id = 22, Nome = "TUF B650M Plus", Categoria = "Placa-Mãe",
              Fabricante = "ASUS", Modelo = "WiFi",
              PrecoVenda = 1299.00m, PrecoCusto = 980.00m, EstoqueDisponivel = 11,
              Descricao = "Placa-mãe AM5 robusta com conectividade Wi-Fi." },

            new Produto { Id = 23, Nome = "DDR5 Fury Beast 16GB", Categoria = "Memória RAM",
              Fabricante = "Kingston", Modelo = "5600MHz",
              PrecoVenda = 399.00m, PrecoCusto = 280.00m, EstoqueDisponivel = 30,
              Descricao = "Memória DDR5 de alta velocidade." },

            new Produto { Id = 24, Nome = "Vengeance RGB 32GB", Categoria = "Memória RAM",
              Fabricante = "Corsair", Modelo = "6000MHz",
              PrecoVenda = 899.00m, PrecoCusto = 670.00m, EstoqueDisponivel = 15,
              Descricao = "Kit DDR5 RGB de alto desempenho." },

            new Produto { Id = 25, Nome = "SSD NV2 1TB", Categoria = "SSD",
              Fabricante = "Kingston", Modelo = "M.2 NVMe",
              PrecoVenda = 379.00m, PrecoCusto = 260.00m, EstoqueDisponivel = 25,
              Descricao = "SSD NVMe Gen4 com leitura rápida." },

            new Produto { Id = 26, Nome = "SSD 980 Pro 1TB", Categoria = "SSD",
              Fabricante = "Samsung", Modelo = "PCIe 4.0",
              PrecoVenda = 699.00m, PrecoCusto = 520.00m, EstoqueDisponivel = 13,
              Descricao = "SSD premium para alto desempenho." },

            new Produto { Id = 27, Nome = "SSD SN850X 2TB", Categoria = "SSD",
              Fabricante = "Western Digital", Modelo = "Black",
              PrecoVenda = 1299.00m, PrecoCusto = 980.00m, EstoqueDisponivel = 8,
              Descricao = "SSD NVMe voltado para gamers e criadores." },

            new Produto { Id = 28, Nome = "HDD Barracuda 2TB", Categoria = "HD",
              Fabricante = "Seagate", Modelo = "7200RPM",
              PrecoVenda = 349.00m, PrecoCusto = 240.00m, EstoqueDisponivel = 20,
              Descricao = "Disco rígido para armazenamento geral." },

            new Produto { Id = 29, Nome = "HDD Blue 4TB", Categoria = "HD",
              Fabricante = "Western Digital", Modelo = "5400RPM",
              PrecoVenda = 549.00m, PrecoCusto = 390.00m, EstoqueDisponivel = 12,
              Descricao = "Armazenamento confiável para desktops." },

            new Produto { Id = 30, Nome = "Fonte RM750e", Categoria = "Fonte",
              Fabricante = "Corsair", Modelo = "750W Gold",
              PrecoVenda = 699.00m, PrecoCusto = 520.00m, EstoqueDisponivel = 14,
              Descricao = "Fonte certificada 80 Plus Gold." },

            new Produto { Id = 31, Nome = "Fonte GX850", Categoria = "Fonte",
              Fabricante = "Seasonic", Modelo = "850W Gold",
              PrecoVenda = 899.00m, PrecoCusto = 690.00m, EstoqueDisponivel = 10,
              Descricao = "Fonte modular de alta eficiência." },

            new Produto { Id = 32, Nome = "Gabinete Air 903", Categoria = "Gabinete",
              Fabricante = "Montech", Modelo = "Mid Tower",
              PrecoVenda = 399.00m, PrecoCusto = 270.00m, EstoqueDisponivel = 22,
              Descricao = "Gabinete com excelente fluxo de ar." },

            new Produto { Id = 33, Nome = "Gabinete H5 Flow", Categoria = "Gabinete",
              Fabricante = "NZXT", Modelo = "Mid Tower",
              PrecoVenda = 599.00m, PrecoCusto = 430.00m, EstoqueDisponivel = 15,
              Descricao = "Gabinete moderno com fluxo de ar otimizado." },

            new Produto { Id = 34, Nome = "Water Cooler H100", Categoria = "Cooler",
              Fabricante = "Corsair", Modelo = "240mm RGB",
              PrecoVenda = 549.00m, PrecoCusto = 390.00m, EstoqueDisponivel = 12,
              Descricao = "Water cooler com radiador de 240mm e iluminação RGB." },

            new Produto { Id = 35, Nome = "AG400", Categoria = "Cooler",
              Fabricante = "DeepCool", Modelo = "Air Cooler",
              PrecoVenda = 179.00m, PrecoCusto = 110.00m, EstoqueDisponivel = 24,
              Descricao = "Cooler a ar eficiente para processadores intermediários." },

            new Produto { Id = 36, Nome = "Monitor Odyssey G5", Categoria = "Monitor",
              Fabricante = "Samsung", Modelo = "27 Polegadas",
              PrecoVenda = 1499.00m, PrecoCusto = 1180.00m, EstoqueDisponivel = 9,
              Descricao = "Monitor gamer QHD com taxa de atualização de 165Hz." },

            new Produto { Id = 37, Nome = "Monitor UltraGear", Categoria = "Monitor",
              Fabricante = "LG", Modelo = "24GN600",
              PrecoVenda = 999.00m, PrecoCusto = 760.00m, EstoqueDisponivel = 14,
              Descricao = "Monitor Full HD gamer de 144Hz." },

            new Produto { Id = 38, Nome = "Teclado K552", Categoria = "Periférico",
              Fabricante = "Redragon", Modelo = "Mecânico RGB",
              PrecoVenda = 229.00m, PrecoCusto = 150.00m, EstoqueDisponivel = 30,
              Descricao = "Teclado mecânico compacto com iluminação RGB." },

            new Produto { Id = 39, Nome = "Teclado Alloy Origins", Categoria = "Periférico",
              Fabricante = "HyperX", Modelo = "RGB",
              PrecoVenda = 499.00m, PrecoCusto = 360.00m, EstoqueDisponivel = 11,
              Descricao = "Teclado mecânico premium para jogos." },

            new Produto { Id = 40, Nome = "Mouse G502 Hero", Categoria = "Periférico",
              Fabricante = "Logitech", Modelo = "Hero 25K",
              PrecoVenda = 299.00m, PrecoCusto = 210.00m, EstoqueDisponivel = 26,
              Descricao = "Mouse gamer com sensor de alta precisão." },

            new Produto { Id = 41, Nome = "Mouse DeathAdder V3", Categoria = "Periférico",
              Fabricante = "Razer", Modelo = "Pro",
              PrecoVenda = 549.00m, PrecoCusto = 410.00m, EstoqueDisponivel = 13,
              Descricao = "Mouse ergonômico para eSports." },

            new Produto { Id = 42, Nome = "Headset Cloud II", Categoria = "Periférico",
              Fabricante = "HyperX", Modelo = "7.1",
              PrecoVenda = 399.00m, PrecoCusto = 290.00m, EstoqueDisponivel = 17,
              Descricao = "Headset gamer com áudio surround virtual." },

            new Produto { Id = 43, Nome = "Headset G435", Categoria = "Periférico",
              Fabricante = "Logitech", Modelo = "Wireless",
              PrecoVenda = 449.00m, PrecoCusto = 330.00m, EstoqueDisponivel = 15,
              Descricao = "Headset sem fio leve e confortável." },

            new Produto { Id = 44, Nome = "Notebook Nitro V15", Categoria = "Notebook",
              Fabricante = "Acer", Modelo = "RTX 4050",
              PrecoVenda = 5499.00m, PrecoCusto = 4350.00m, EstoqueDisponivel = 5,
              Descricao = "Notebook gamer com GPU dedicada RTX 4050." },

            new Produto { Id = 45, Nome = "Notebook LOQ", Categoria = "Notebook",
              Fabricante = "Lenovo", Modelo = "RTX 4060",
              PrecoVenda = 6499.00m, PrecoCusto = 5150.00m, EstoqueDisponivel = 4,
              Descricao = "Notebook gamer com excelente desempenho." },

            new Produto { Id = 46, Nome = "Ryzen 9 7900X", Categoria = "Processador",
              Fabricante = "AMD", Modelo = "Zen 4",
              PrecoVenda = 2999.00m, PrecoCusto = 2400.00m, EstoqueDisponivel = 8,
              Descricao = "Processador de alto desempenho para multitarefa." },

            new Produto { Id = 47, Nome = "Core i9-14900K", Categoria = "Processador",
              Fabricante = "Intel", Modelo = "14ª Geração",
              PrecoVenda = 3999.00m, PrecoCusto = 3250.00m, EstoqueDisponivel = 5,
              Descricao = "Processador topo de linha para gamers e criadores." },

            new Produto { Id = 48, Nome = "RTX 4080 Super", Categoria = "GPU",
              Fabricante = "Gigabyte", Modelo = "Gaming OC",
              PrecoVenda = 8299.00m, PrecoCusto = 6900.00m, EstoqueDisponivel = 3,
              Descricao = "Placa de vídeo premium para 4K." },

            new Produto { Id = 49, Nome = "RTX 4090", Categoria = "GPU",
              Fabricante = "ASUS", Modelo = "ROG Strix",
              PrecoVenda = 12999.00m, PrecoCusto = 10900.00m, EstoqueDisponivel = 2,
              Descricao = "A GPU mais poderosa da geração RTX." },

            new Produto { Id = 50, Nome = "RX 7900 XTX", Categoria = "GPU",
              Fabricante = "Sapphire", Modelo = "Nitro+",
              PrecoVenda = 7499.00m, PrecoCusto = 6200.00m, EstoqueDisponivel = 4,
              Descricao = "GPU AMD para jogos em resolução 4K." },

            new Produto { Id = 51, Nome = "A520M-A Pro", Categoria = "Placa-Mãe",
              Fabricante = "MSI", Modelo = "AM4",
              PrecoVenda = 449.00m, PrecoCusto = 310.00m, EstoqueDisponivel = 21,
              Descricao = "Placa-mãe econômica para processadores AMD." },

        new Produto { Id = 52, Nome = "B550M Steel Legend", Categoria = "Placa-Mãe",
              Fabricante = "ASRock", Modelo = "AM4",
              PrecoVenda = 899.00m, PrecoCusto = 670.00m, EstoqueDisponivel = 12,
              Descricao = "Placa-mãe robusta para Ryzen série 5000." },
        };

        private int _proximoIdProduto => _produtos.Any() ? _produtos.Max(p => p.Id) + 1 : 1;

        // ── Pedidos ───────────────────────────────────────────────────────────────

        private readonly List<Pedido> _pedidos = new();
        private int _proximoIdPedido => _pedidos.Any() ? _pedidos.Max(p => p.Id) + 1 : 1;

        // ── Acesso: Clientes ──────────────────────────────────────────────────────

        public IReadOnlyList<Cliente> ObterTodosClientes() => _clientes.AsReadOnly();

        public Cliente? ObterClientePorId(int id) =>
            _clientes.FirstOrDefault(c => c.Id == id);

        public Cliente AdicionarCliente(Cliente cliente)
        {
            cliente.Id = _proximoIdCliente++;
            _clientes.Add(cliente);
            return cliente;
        }

        public Cliente? AtualizarCliente(int id, Cliente atualizado)
        {
            var cliente = ObterClientePorId(id);
            if (cliente is null) return null;

            cliente.Nome     = atualizado.Nome;
            cliente.Email    = atualizado.Email;
            cliente.Telefone = atualizado.Telefone;
            cliente.Endereco = atualizado.Endereco;

            return cliente;
        }

        public bool RemoverCliente(int id)
        {
            var cliente = ObterClientePorId(id);
            if (cliente is null) return false;
            _clientes.Remove(cliente);
            return true;
        }

        // ── Acesso: Produtos ──────────────────────────────────────────────────────

        public IReadOnlyList<Produto> ObterTodosProdutos() => _produtos.AsReadOnly();

        public Produto? ObterProdutoPorId(int id) =>
            _produtos.FirstOrDefault(p => p.Id == id);

        public Produto AdicionarProduto(Produto produto)
        {
            produto.Id = _proximoIdProduto;
            _produtos.Add(produto);
            return produto;
        }

        public Produto? AtualizarProduto(int id, Produto atualizado)
        {
            var produto = ObterProdutoPorId(id);
            if (produto is null) return null;

            produto.Nome              = atualizado.Nome;
            produto.Categoria         = atualizado.Categoria;
            produto.Fabricante        = atualizado.Fabricante;
            produto.Modelo            = atualizado.Modelo;
            produto.PrecoVenda        = atualizado.PrecoVenda;
            produto.PrecoCusto        = atualizado.PrecoCusto;
            produto.EstoqueDisponivel = atualizado.EstoqueDisponivel;
            produto.Descricao         = atualizado.Descricao;

            return produto;
        }

        public bool RemoverProduto(int id)
        {
            var produto = ObterProdutoPorId(id);
            if (produto is null) return false;
            _produtos.Remove(produto);
            return true;
        }

        // ── Acesso: Pedidos ───────────────────────────────────────────────────────

        public IReadOnlyList<Pedido> ObterTodosPedidos() => _pedidos.AsReadOnly();

        public Pedido? ObterPedidoPorId(int id) =>
            _pedidos.FirstOrDefault(p => p.Id == id);

        public Pedido AdicionarPedido(Pedido pedido)
        {
            pedido.Id         = _proximoIdPedido;
            pedido.DataPedido = DateTime.UtcNow;
            _pedidos.Add(pedido);
            return pedido;
        }

        public bool RemoverPedido(int id)
        {
            var pedido = ObterPedidoPorId(id);
            if (pedido is null) return false;
            _pedidos.Remove(pedido);
            return true;
        }

        // ── Estatísticas para o Dashboard ────────────────────────────────────────

        public DashboardStats ObterEstatisticasDashboard()
        {
            var produtos = _produtos;

            // Apenas pedidos Confirmados entram nos cálculos financeiros
            var pedidosConfirmados = _pedidos
                .Where(p => p.Status == StatusPedido.Confirmado)
                .ToList();

            var receitaVendas = pedidosConfirmados
                .SelectMany(p => p.Itens)
                .Sum(i => i.Subtotal);

            var custoVendidos = pedidosConfirmados
                .SelectMany(p => p.Itens)
                .Sum(i => i.CustoTotal);

            return new DashboardStats
            {
                TotalProdutos    = produtos.Count,
                ProdutosAtivos   = produtos.Count(p => p.EstoqueDisponivel > 0),
                ProdutosInativos = produtos.Count(p => p.EstoqueDisponivel == 0),
                TotalEstoque     = produtos.Sum(p => p.EstoqueDisponivel),
                ValorInvestido   = produtos.Sum(p => p.PrecoCusto * p.EstoqueDisponivel),
                ReceitaVendas    = receitaVendas,
                CustoVendidos    = custoVendidos
            };
        }
    }
}