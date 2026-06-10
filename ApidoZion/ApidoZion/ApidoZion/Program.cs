using ApidoZion.Models;
using ApidoZion.Services;
using System.Text.Json.Serialization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.WriteIndented = true;
});

var fornecedoresList = new List<Fornecedor>
{
    new Fornecedor { Id = "f001", Nome_RazaoSocial = "Tech Components LTDA", CPF_CNPJ = "12.345.678/0001-01", Email = "contato@techcomponents.com", Telefone = "(11) 4000-1001", Endereco = "São Paulo - SP", Status = "Online", Pecas = "GPU", Valor = 850, Quantidade = 100 },
    new Fornecedor { Id = "f002", Nome_RazaoSocial = "Power Hardware", CPF_CNPJ = "22.345.678/0001-02", Email = "vendas@powerhardware.com", Telefone = "(21) 4000-1002", Endereco = "Rio de Janeiro - RJ", Status = "Offline", Pecas = "CPU", Valor = 700, Quantidade = 80 },
    new Fornecedor { Id = "f003", Nome_RazaoSocial = "Mega Chips", CPF_CNPJ = "32.345.678/0001-03", Email = "contato@megachips.com", Telefone = "(31) 4000-1003", Endereco = "Belo Horizonte - MG", Status = "Online", Pecas = "RAM", Valor = 180, Quantidade = 300 },
    new Fornecedor { Id = "f004", Nome_RazaoSocial = "Storage Brasil", CPF_CNPJ = "42.345.678/0001-04", Email = "contato@storage.com", Telefone = "(41) 4000-1004", Endereco = "Curitiba - PR", Status = "Online", Pecas = "SSD", Valor = 220, Quantidade = 250 },
    new Fornecedor { Id = "f005", Nome_RazaoSocial = "Coolers Pro", CPF_CNPJ = "52.345.678/0001-05", Email = "contato@coolerspro.com", Telefone = "(51) 4000-1005", Endereco = "Porto Alegre - RS", Status = "Online", Pecas = "Cooler", Valor = 90, Quantidade = 180 },
    new Fornecedor { Id = "f006", Nome_RazaoSocial = "Extreme Parts", CPF_CNPJ = "62.345.678/0001-06", Email = "vendas@extremeparts.com", Telefone = "(61) 4000-1006", Endereco = "Brasília - DF", Status = "Online", Pecas = "GPU", Valor = 950, Quantidade = 90 },
    new Fornecedor { Id = "f007", Nome_RazaoSocial = "Processadores Brasil", CPF_CNPJ = "72.345.678/0001-07", Email = "contato@processadores.com", Telefone = "(71) 4000-1007", Endereco = "Salvador - BA", Status = "Sleepy", Pecas = "CPU", Valor = 650, Quantidade = 120 },
    new Fornecedor { Id = "f008", Nome_RazaoSocial = "Memory Tech", CPF_CNPJ = "82.345.678/0001-08", Email = "contato@memorytech.com", Telefone = "(81) 4000-1008", Endereco = "Recife - PE", Status = "Online", Pecas = "CPU", Valor = 210, Quantidade = 280 },
    new Fornecedor { Id = "f009", Nome_RazaoSocial = "SSD Master", CPF_CNPJ = "92.345.678/0001-09", Email = "vendas@ssdmaster.com", Telefone = "(91) 4000-1009", Endereco = "Belém - PA", Status = "Online", Pecas = "Cooler", Valor = 250, Quantidade = 170 },
    new Fornecedor { Id = "f010", Nome_RazaoSocial = "Cooling Systems", CPF_CNPJ = "10.345.678/0001-10", Email = "contato@coolingsystems.com", Telefone = "(85) 4000-1010", Endereco = "Fortaleza - CE", Status = "Online", Pecas = "Cooler", Valor = 110, Quantidade = 200 },
    new Fornecedor { Id = "f011", Nome_RazaoSocial = "Titan Hardware", CPF_CNPJ = "11.345.678/0001-11", Email = "contato@titanhardware.com", Telefone = "(41) 4000-1111", Endereco = "Curitiba - PR", Status = "Online", Pecas = "GPU", Valor = 950, Quantidade = 180 },
    new Fornecedor { Id = "f012", Nome_RazaoSocial = "Memory Tech", CPF_CNPJ = "12.345.678/0001-12", Email = "vendas@memorytech.com", Telefone = "(11) 4000-1212", Endereco = "São Paulo - SP", Status = "Online", Pecas = "RAM", Valor = 220, Quantidade = 500 },
    new Fornecedor { Id = "f013", Nome_RazaoSocial = "Storage Masters", CPF_CNPJ = "13.345.678/0001-13", Email = "contato@storagemasters.com", Telefone = "(31) 4000-1313", Endereco = "Belo Horizonte - MG", Status = "Sleepy", Pecas = "SSD", Valor = 350, Quantidade = 320 },
    new Fornecedor { Id = "f014", Nome_RazaoSocial = "Power Supply Brasil", CPF_CNPJ = "14.345.678/0001-14", Email = "contato@psb.com", Telefone = "(21) 4000-1414", Endereco = "Rio de Janeiro - RJ", Status = "Online", Pecas = "Fonte", Valor = 420, Quantidade = 250 },
    new Fornecedor { Id = "f015", Nome_RazaoSocial = "Dragon Components", CPF_CNPJ = "15.345.678/0001-15", Email = "contato@dragoncomponents.com", Telefone = "(47) 4000-1515", Endereco = "Joinville - SC", Status = "Offline", Pecas = "Placa-Mãe", Valor = 670, Quantidade = 120 },
    new Fornecedor { Id = "f016", Nome_RazaoSocial = "Crystal Processors", CPF_CNPJ = "16.345.678/0001-16", Email = "vendas@crystalcpu.com", Telefone = "(51) 4000-1616", Endereco = "Porto Alegre - RS", Status = "Online", Pecas = "CPU", Valor = 890, Quantidade = 140 },
    new Fornecedor { Id = "f017", Nome_RazaoSocial = "Elite Cooling", CPF_CNPJ = "17.345.678/0001-17", Email = "contato@elitecooling.com", Telefone = "(85) 4000-1717", Endereco = "Fortaleza - CE", Status = "Online", Pecas = "Cooler", Valor = 140, Quantidade = 300 },
    new Fornecedor { Id = "f018", Nome_RazaoSocial = "Vision Graphics", CPF_CNPJ = "18.345.678/0001-18", Email = "vendas@visiongraphics.com", Telefone = "(62) 4000-1818", Endereco = "Goiânia - GO", Status = "Sleepy", Pecas = "GPU", Valor = 1200, Quantidade = 90 },
    new Fornecedor { Id = "f019", Nome_RazaoSocial = "Thunder Storage", CPF_CNPJ = "19.345.678/0001-19", Email = "contato@thunderstorage.com", Telefone = "(71) 4000-1919", Endereco = "Salvador - BA", Status = "Online", Pecas = "SSD", Valor = 390, Quantidade = 220 },
    new Fornecedor { Id = "f020", Nome_RazaoSocial = "Alpha Components", CPF_CNPJ = "20.345.678/0001-20", Email = "vendas@alphacomponents.com", Telefone = "(48) 4000-2020", Endereco = "Florianópolis - SC", Status = "Online", Pecas = "RAM", Valor = 240, Quantidade = 450 },
    new Fornecedor { Id = "f021", Nome_RazaoSocial = "Quantum Hardware", CPF_CNPJ = "21.345.678/0001-21", Email = "contato@quantumhardware.com", Telefone = "(11) 4000-2121", Endereco = "São Paulo - SP", Status = "Online", Pecas = "GPU", Valor = 1350, Quantidade = 110 },
    new Fornecedor { Id = "f022", Nome_RazaoSocial = "Prime SSD Solutions", CPF_CNPJ = "22.345.678/0001-22", Email = "vendas@primessd.com", Telefone = "(41) 4000-2222", Endereco = "Curitiba - PR", Status = "Online", Pecas = "SSD", Valor = 420, Quantidade = 280 },
    new Fornecedor { Id = "f023", Nome_RazaoSocial = "Ultra Memory Tech", CPF_CNPJ = "23.345.678/0001-23", Email = "contato@ultramemory.com", Telefone = "(31) 4000-2323", Endereco = "Belo Horizonte - MG", Status = "Sleepy", Pecas = "RAM", Valor = 260, Quantidade = 550 },
    new Fornecedor { Id = "f024", Nome_RazaoSocial = "Imperium Processadores", CPF_CNPJ = "24.345.678/0001-24", Email = "comercial@imperiumcpu.com", Telefone = "(51) 4000-2424", Endereco = "Porto Alegre - RS", Status = "Online", Pecas = "CPU", Valor = 980, Quantidade = 160 },
    new Fornecedor { Id = "f025", Nome_RazaoSocial = "Master Cooling Brasil", CPF_CNPJ = "25.345.678/0001-25", Email = "contato@mastercooling.com", Telefone = "(21) 4000-2525", Endereco = "Rio de Janeiro - RJ", Status = "Offline", Pecas = "Cooler", Valor = 155, Quantidade = 340 },
};
builder.Services.AddSingleton<List<Fornecedor>>(fornecedoresList);

var clientesList = new List<Cliente>
{
    new Cliente { Id ="cl001", Nome_RazaoSocial = "João Silva", CPF_CNPJ = "123.456.789-00", Email = "joao@email.com", Telefone = "(11) 99999-9999", Endereco = "São Paulo - SP", Status = "online", TotalGasto = 5000, TotalLucroBullDogs = 1500, QuantidadeCompras = 3, PecaMaisComprada = "GPU", ComprasNoUltimoMes = 2 },
    new Cliente { Id ="cl002", Nome_RazaoSocial = "Maria Santos", CPF_CNPJ = "987.654.321-00", Email = "maria@email.com", Telefone = "(21) 98888-8888", Endereco = "Rio de Janeiro - RJ", Status = "offline", TotalGasto = 3500, TotalLucroBullDogs = 1000, QuantidadeCompras = 2, PecaMaisComprada = "CPU", ComprasNoUltimoMes = 1 },
    new Cliente { Id="cl003", Nome_RazaoSocial="TechStore Curitiba", CPF_CNPJ="12.345.678/0001-01", Endereco="Curitiba - PR", Email="contato@techstore.com", Telefone="41999990001", Status="online", TotalGasto=85000, TotalLucroBullDogs=17000, QuantidadeCompras=12, PecaMaisComprada="RTX 4070", ComprasNoUltimoMes=2 },
    new Cliente { Id="cl004", Nome_RazaoSocial="Arena Gamer Sul", CPF_CNPJ="22.345.678/0001-02", Endereco="Porto Alegre - RS", Email="vendas@arenagamer.com", Telefone="51999990002", Status="online", TotalGasto=120000, TotalLucroBullDogs=24000, QuantidadeCompras=18, PecaMaisComprada="Ryzen 7 7800X3D", ComprasNoUltimoMes=4 },
    new Cliente { Id="cl005", Nome_RazaoSocial="Level Up Informática", CPF_CNPJ="32.345.678/0001-03", Endereco="Florianópolis - SC", Email="contato@levelup.com", Telefone="48999990003", Status="sleepy", TotalGasto=45000, TotalLucroBullDogs=9000, QuantidadeCompras=8, PecaMaisComprada="SSD Kingston 1TB", ComprasNoUltimoMes=0 },
    new Cliente { Id="cl006", Nome_RazaoSocial="Titan PCs", CPF_CNPJ="42.345.678/0001-04", Endereco="São Paulo - SP", Email="compras@titanpcs.com", Telefone="11999990004", Status="online", TotalGasto=200000, TotalLucroBullDogs=40000, QuantidadeCompras=25, PecaMaisComprada="RTX 5080", ComprasNoUltimoMes=5 },
    new Cliente { Id="cl007", Nome_RazaoSocial="Phoenix Hardware", CPF_CNPJ="52.345.678/0001-05", Endereco="Belo Horizonte - MG", Email="contato@phoenix.com", Telefone="31999990005", Status="offline", TotalGasto=25000, TotalLucroBullDogs=5000, QuantidadeCompras=4, PecaMaisComprada="Memória DDR5", ComprasNoUltimoMes=0 },
    new Cliente { Id="cl008", Nome_RazaoSocial="Master Games", CPF_CNPJ="62.345.678/0001-06", Endereco="Rio de Janeiro - RJ", Email="vendas@mastergames.com", Telefone="21999990006", Status="online", TotalGasto=95000, TotalLucroBullDogs=19000, QuantidadeCompras=13, PecaMaisComprada="RTX 4060", ComprasNoUltimoMes=2 },
    new Cliente { Id="cl009", Nome_RazaoSocial="Dragon Tech", CPF_CNPJ="72.345.678/0001-07", Endereco="Londrina - PR", Email="contato@dragontech.com", Telefone="43999990007", Status="online", TotalGasto=70000, TotalLucroBullDogs=14000, QuantidadeCompras=10, PecaMaisComprada="Ryzen 5 7600", ComprasNoUltimoMes=1 },
    new Cliente { Id="cl010", Nome_RazaoSocial="Infinity Store", CPF_CNPJ="82.345.678/0001-08", Endereco="Joinville - SC", Email="compras@infinity.com", Telefone="47999990008", Status="sleepy", TotalGasto=38000, TotalLucroBullDogs=7600, QuantidadeCompras=6, PecaMaisComprada="Cooler Master", ComprasNoUltimoMes=0 },
    new Cliente { Id="cl011", Nome_RazaoSocial="Elite Hardware", CPF_CNPJ="92.345.678/0001-09", Endereco="Campinas - SP", Email="elite@hardware.com", Telefone="19999990009", Status="online", TotalGasto=160000, TotalLucroBullDogs=32000, QuantidadeCompras=21, PecaMaisComprada="RTX 5070", ComprasNoUltimoMes=3 },
    new Cliente { Id="cl012", Nome_RazaoSocial="Ruby Informática", CPF_CNPJ="10.345.678/0001-10", Endereco="Maringá - PR", Email="contato@rubyinfo.com", Telefone="44999990010", Status="offline", TotalGasto=18000, TotalLucroBullDogs=3600, QuantidadeCompras=3, PecaMaisComprada="SSD NVMe", ComprasNoUltimoMes=0 },
    new Cliente { Id="cl013", Nome_RazaoSocial="Nebula Games", CPF_CNPJ="11.456.789/0001-11", Endereco="Curitiba - PR", Email="contato@nebula.com", Telefone="41988881111", Status="online", TotalGasto=24000, TotalLucroBullDogs=4800, QuantidadeCompras=4, PecaMaisComprada="RTX 4060 Ti", ComprasNoUltimoMes=1 },
    new Cliente { Id="cl014", Nome_RazaoSocial="Phoenix Tech", CPF_CNPJ="13.456.789/0001-13", Endereco="Campinas - SP", Email="vendas@phoenix.com", Telefone="19988881313", Status="online", TotalGasto=41000, TotalLucroBullDogs=8200, QuantidadeCompras=7, PecaMaisComprada="Ryzen 7", ComprasNoUltimoMes=2 },
    new Cliente { Id="cl015", Nome_RazaoSocial="Infinity Store 2", CPF_CNPJ="14.456.789/0001-14", Endereco="Joinville - SC", Email="contato@infinity2.com", Telefone="47988881414", Status="sleepy", TotalGasto=15000, TotalLucroBullDogs=3000, QuantidadeCompras=3, PecaMaisComprada="SSD Kingston", ComprasNoUltimoMes=0 },
    new Cliente { Id="cl016", Nome_RazaoSocial="Dragon PC Center", CPF_CNPJ="15.456.789/0001-15", Endereco="Londrina - PR", Email="vendas@dragonpc.com", Telefone="43988881515", Status="online", TotalGasto=58000, TotalLucroBullDogs=11600, QuantidadeCompras=9, PecaMaisComprada="RTX 4070", ComprasNoUltimoMes=3 },
    new Cliente { Id="cl017", Nome_RazaoSocial="Titan Games", CPF_CNPJ="16.456.789/0001-16", Endereco="São Paulo - SP", Email="compras@titangames.com", Telefone="11988881616", Status="online", TotalGasto=72000, TotalLucroBullDogs=14400, QuantidadeCompras=11, PecaMaisComprada="RTX 5080", ComprasNoUltimoMes=4 },
    new Cliente { Id="cl018", Nome_RazaoSocial="Arena Tech", CPF_CNPJ="17.456.789/0001-17", Endereco="Fortaleza - CE", Email="contato@arenatech.com", Telefone="85988881717", Status="offline", TotalGasto=12000, TotalLucroBullDogs=2400, QuantidadeCompras=2, PecaMaisComprada="Cooler Master", ComprasNoUltimoMes=0 },
    new Cliente { Id="cl019", Nome_RazaoSocial="Elite Hardware 2", CPF_CNPJ="18.456.789/0001-18", Endereco="Rio de Janeiro - RJ", Email="vendas@elitehw2.com", Telefone="21988881818", Status="online", TotalGasto=36000, TotalLucroBullDogs=7200, QuantidadeCompras=6, PecaMaisComprada="Memória DDR5", ComprasNoUltimoMes=1 },
    new Cliente { Id="cl020", Nome_RazaoSocial="Crystal Informática", CPF_CNPJ="19.456.789/0001-19", Endereco="Belo Horizonte - MG", Email="contato@crystalinfo.com", Telefone="31988881919", Status="sleepy", TotalGasto=22000, TotalLucroBullDogs=4400, QuantidadeCompras=4, PecaMaisComprada="SSD NVMe", ComprasNoUltimoMes=0 },
    new Cliente { Id="cl021", Nome_RazaoSocial="Omega Store", CPF_CNPJ="20.456.789/0001-20", Endereco="Porto Alegre - RS", Email="vendas@omegastore.com", Telefone="51988882020", Status="online", TotalGasto=95000, TotalLucroBullDogs=19000, QuantidadeCompras=14, PecaMaisComprada="RTX 5070", ComprasNoUltimoMes=5 },
    new Cliente { Id="cl022", Nome_RazaoSocial="SkyNet Informática", CPF_CNPJ="21.456.789/0001-21", Endereco="Curitiba - PR", Email="contato@skynetinfo.com", Telefone="41988882121", Status="online", TotalGasto=67000, TotalLucroBullDogs=13400, QuantidadeCompras=10, PecaMaisComprada="RTX 5070", ComprasNoUltimoMes=3 },
    new Cliente { Id="cl023", Nome_RazaoSocial="Alpha Gamer Store", CPF_CNPJ="22.456.789/0001-22", Endereco="São Paulo - SP", Email="vendas@alphagamer.com", Telefone="11988882222", Status="online", TotalGasto=89000, TotalLucroBullDogs=17800, QuantidadeCompras=13, PecaMaisComprada="RTX 5080", ComprasNoUltimoMes=4 },
    new Cliente { Id="cl024", Nome_RazaoSocial="Byte Solutions", CPF_CNPJ="23.456.789/0001-23", Endereco="Joinville - SC", Email="contato@bytesolutions.com", Telefone="47988882323", Status="offline", TotalGasto=14000, TotalLucroBullDogs=2800, QuantidadeCompras=2, PecaMaisComprada="SSD Kingston NV2 1TB", ComprasNoUltimoMes=0 },
    new Cliente { Id="cl025", Nome_RazaoSocial="Crystal Games", CPF_CNPJ="24.456.789/0001-24", Endereco="Maringá - PR", Email="compras@crystalgames.com", Telefone="44988882424", Status="sleepy", TotalGasto=26000, TotalLucroBullDogs=5200, QuantidadeCompras=5, PecaMaisComprada="DDR5 Corsair 16GB", ComprasNoUltimoMes=0 },
    new Cliente { Id="cl026", Nome_RazaoSocial="Omega Hardware Center", CPF_CNPJ="25.456.789/0001-25", Endereco="Florianópolis - SC", Email="contato@omegahardware.com", Telefone="48988882525", Status="online", TotalGasto=112000, TotalLucroBullDogs=22400, QuantidadeCompras=16, PecaMaisComprada="RTX 5080", ComprasNoUltimoMes=6 },
};
builder.Services.AddSingleton<List<Cliente>>(clientesList);

var pecasList = new List<Peca>
{
    new Peca { Id = "pc001", NomePeca = "RTX 4060 Ti", Categoria = "GPU", Marca = "NVIDIA", FornecedorId = "f001", CustoFornecedor = 850, PrecoVenda = 1200, MargemLucro = 41.2, QuantidadeVendida = 2, QuantidadeReposicoes = 1, LucroTotal = 700, RankingVendas = 1, EhMaisVendida = true, EhMenosVendida = false },
    new Peca { Id = "pc002", NomePeca = "Ryzen 7 5700X", Categoria = "CPU", Marca = "AMD", FornecedorId = "f002", CustoFornecedor = 700, PrecoVenda = 1000, MargemLucro = 42.9, QuantidadeVendida = 1, QuantidadeReposicoes = 1, LucroTotal = 300, RankingVendas = 2, EhMaisVendida = false, EhMenosVendida = true },
    new Peca { Id = "pc011", NomePeca = "RTX 5070", Categoria = "GPU", Marca = "NVIDIA", FornecedorId = "f011", CustoFornecedor = 1200, PrecoVenda = 1700, MargemLucro = 41.6, QuantidadeVendida = 18, QuantidadeReposicoes = 3, LucroTotal = 9000, RankingVendas = 3, EhMaisVendida = false, EhMenosVendida = false },
    new Peca { Id = "pc012", NomePeca = "Ryzen 7 7800X3D", Categoria = "CPU", Marca = "AMD", FornecedorId = "f016", CustoFornecedor = 900, PrecoVenda = 1400, MargemLucro = 55.5, QuantidadeVendida = 14, QuantidadeReposicoes = 2, LucroTotal = 7000, RankingVendas = 4, EhMaisVendida = false, EhMenosVendida = false },
    new Peca { Id = "pc013", NomePeca = "Kingston Fury 32GB", Categoria = "RAM", Marca = "Kingston", FornecedorId = "f012", CustoFornecedor = 250, PrecoVenda = 450, MargemLucro = 80, QuantidadeVendida = 22, QuantidadeReposicoes = 3, LucroTotal = 4400, RankingVendas = 2, EhMaisVendida = false, EhMenosVendida = false },
    new Peca { Id = "pc014", NomePeca = "SSD Kingston NV2 1TB", Categoria = "SSD", Marca = "Kingston", FornecedorId = "f013", CustoFornecedor = 280, PrecoVenda = 420, MargemLucro = 50, QuantidadeVendida = 17, QuantidadeReposicoes = 2, LucroTotal = 2380, RankingVendas = 5, EhMaisVendida = false, EhMenosVendida = false },
    new Peca { Id = "pc015", NomePeca = "Cooler Master Hyper 212", Categoria = "Cooler", Marca = "Cooler Master", FornecedorId = "f017", CustoFornecedor = 110, PrecoVenda = 220, MargemLucro = 100, QuantidadeVendida = 8, QuantidadeReposicoes = 1, LucroTotal = 880, RankingVendas = 8, EhMaisVendida = false, EhMenosVendida = false },
    new Peca { Id = "pc016", NomePeca = "ASUS B650M", Categoria = "Placa-Mãe", Marca = "ASUS", FornecedorId = "f015", CustoFornecedor = 650, PrecoVenda = 980, MargemLucro = 50.7, QuantidadeVendida = 12, QuantidadeReposicoes = 2, LucroTotal = 3960, RankingVendas = 6, EhMaisVendida = false, EhMenosVendida = false },
    new Peca { Id = "pc017", NomePeca = "Fonte Corsair 750W", Categoria = "Fonte", Marca = "Corsair", FornecedorId = "f014", CustoFornecedor = 300, PrecoVenda = 520, MargemLucro = 73.3, QuantidadeVendida = 15, QuantidadeReposicoes = 2, LucroTotal = 3300, RankingVendas = 7, EhMaisVendida = false, EhMenosVendida = false },
    new Peca { Id = "pc018", NomePeca = "RTX 5080", Categoria = "GPU", Marca = "NVIDIA", FornecedorId = "f018", CustoFornecedor = 2500, PrecoVenda = 3500, MargemLucro = 40, QuantidadeVendida = 6, QuantidadeReposicoes = 1, LucroTotal = 6000, RankingVendas = 9, EhMaisVendida = false, EhMenosVendida = false },
    new Peca { Id = "pc019", NomePeca = "SSD WD Black 2TB", Categoria = "SSD", Marca = "Western Digital", FornecedorId = "f019", CustoFornecedor = 450, PrecoVenda = 690, MargemLucro = 53.3, QuantidadeVendida = 11, QuantidadeReposicoes = 2, LucroTotal = 2640, RankingVendas = 10, EhMaisVendida = false, EhMenosVendida = true },
    new Peca { Id = "pc020", NomePeca = "DDR5 Corsair 16GB", Categoria = "RAM", Marca = "Corsair", FornecedorId = "f020", CustoFornecedor = 180, PrecoVenda = 340, MargemLucro = 88.8, QuantidadeVendida = 28, QuantidadeReposicoes = 4, LucroTotal = 4480, RankingVendas = 1, EhMaisVendida = true, EhMenosVendida = false },
};
builder.Services.AddSingleton<List<Peca>>(pecasList);

var vendaList = new List<Venda>
{
    new Venda { Id = "vd001", NumeroNota = "NF-001", ClienteId = "cl001", PecaId = "pc001", QuantidadeVendida = 2, PrecoUnitarioVenda = 1200, DescontoAplicado = 0, PrecoComDesconto = 1200, ValorTotalVenda = 2400, LucroLiquido = 700, PercentualLucro = 29.2 },
    new Venda { Id = "vd002", NumeroNota = "NF-002", ClienteId = "cl002", PecaId = "pc002", QuantidadeVendida = 1, PrecoUnitarioVenda = 1000, DescontoAplicado = 0, PrecoComDesconto = 1000, ValorTotalVenda = 1000, LucroLiquido = 300, PercentualLucro = 30.0 },
    new Venda { Id = "vd003", NumeroNota = "NF-003", ClienteId = "cl001", PecaId = "pc002", QuantidadeVendida = 3, PrecoUnitarioVenda = 1000, DescontoAplicado = 5, PrecoComDesconto = 950, ValorTotalVenda = 2850, LucroLiquido = 900, PercentualLucro = 31.6 },
    new Venda { Id = "vd004", NumeroNota = "NF-004", ClienteId = "cl002", PecaId = "pc001", QuantidadeVendida = 1, PrecoUnitarioVenda = 1200, DescontoAplicado = 0, PrecoComDesconto = 1200, ValorTotalVenda = 1200, LucroLiquido = 350, PercentualLucro = 29.2 },
    new Venda { Id = "vd005", NumeroNota = "NF-005", ClienteId = "cl001", PecaId = "pc001", QuantidadeVendida = 4, PrecoUnitarioVenda = 1200, DescontoAplicado = 10, PrecoComDesconto = 1080, ValorTotalVenda = 4320, LucroLiquido = 1400, PercentualLucro = 32.4 },
    new Venda { Id = "vd006", NumeroNota = "NF-006", ClienteId = "cl002", PecaId = "pc002", QuantidadeVendida = 2, PrecoUnitarioVenda = 1000, DescontoAplicado = 0, PrecoComDesconto = 1000, ValorTotalVenda = 2000, LucroLiquido = 600, PercentualLucro = 30.0 },
    new Venda { Id = "vd007", NumeroNota = "NF-007", ClienteId = "cl001", PecaId = "pc001", QuantidadeVendida = 2, PrecoUnitarioVenda = 1200, DescontoAplicado = 3, PrecoComDesconto = 1164, ValorTotalVenda = 2328, LucroLiquido = 700, PercentualLucro = 30.1 },
    new Venda { Id = "vd008", NumeroNota = "NF-008", ClienteId = "cl002", PecaId = "pc002", QuantidadeVendida = 5, PrecoUnitarioVenda = 1000, DescontoAplicado = 8, PrecoComDesconto = 920, ValorTotalVenda = 4600, LucroLiquido = 1500, PercentualLucro = 32.6 },
};
builder.Services.AddSingleton<List<Venda>>(vendaList);

var app = builder.Build();

app.UseCors("AllowAll");
app.UseStaticFiles();
app.UseDefaultFiles();

app.MapGet("/", () => "Bem-vindo ao Zion System - Gestão de Vendas para BullDogs PC");

app.MapGet("/api/clientes", () => app.Services.GetRequiredService<List<Cliente>>());

app.MapGet("/api/clientes/{id}", (string id) =>
{
    var clientes = app.Services.GetRequiredService<List<Cliente>>();
    var cliente = clientes.FirstOrDefault(c => c.Id == id);
    return cliente == null ? Results.NotFound() : Results.Ok(cliente);
});

app.MapPost("/api/clientes", (Cliente cliente) =>
{
    var clientes = app.Services.GetRequiredService<List<Cliente>>();
    if (clientes.Any(c => c.Id == cliente.Id))
        return Results.BadRequest("Cliente com este ID já existe");
    clientes.Add(cliente);
    return Results.Created($"/api/clientes/{cliente.Id}", cliente);
});

app.MapPut("/api/clientes/{id}", (string id, Cliente clienteAtualizado) =>
{
    var clientes = app.Services.GetRequiredService<List<Cliente>>();
    var cliente = clientes.FirstOrDefault(c => c.Id == id);
    if (cliente == null) return Results.NotFound();
    clientes.Remove(cliente);
    clienteAtualizado.Id = id;
    clientes.Add(clienteAtualizado);
    return Results.Ok(clienteAtualizado);
});

app.MapDelete("/api/clientes/{id}", (string id) =>
{
    var clientes = app.Services.GetRequiredService<List<Cliente>>();
    var cliente = clientes.FirstOrDefault(c => c.Id == id);
    if (cliente == null) return Results.NotFound();
    clientes.Remove(cliente);
    return Results.NoContent();
});

app.MapGet("/api/fornecedores", () => app.Services.GetRequiredService<List<Fornecedor>>());

app.MapGet("/api/fornecedores/{id}", (string id) =>
{
    var fornecedores = app.Services.GetRequiredService<List<Fornecedor>>();
    var fornecedor = fornecedores.FirstOrDefault(f => f.Id == id);
    return fornecedor == null ? Results.NotFound() : Results.Ok(fornecedor);
});

app.MapPost("/api/fornecedores", (Fornecedor fornecedor) =>
{
    var fornecedores = app.Services.GetRequiredService<List<Fornecedor>>();
    if (fornecedores.Any(f => f.Id == fornecedor.Id))
        return Results.BadRequest("Fornecedor com este ID já existe");
    fornecedores.Add(fornecedor);
    return Results.Created($"/api/fornecedores/{fornecedor.Id}", fornecedor);
});

app.MapPut("/api/fornecedores/{id}", (string id, Fornecedor fornecedorAtualizado) =>
{
    var fornecedores = app.Services.GetRequiredService<List<Fornecedor>>();
    var fornecedor = fornecedores.FirstOrDefault(f => f.Id == id);
    if (fornecedor == null) return Results.NotFound();
    fornecedores.Remove(fornecedor);
    fornecedorAtualizado.Id = id;
    fornecedores.Add(fornecedorAtualizado);
    return Results.Ok(fornecedorAtualizado);
});

app.MapDelete("/api/fornecedores/{id}", (string id) =>
{
    var fornecedores = app.Services.GetRequiredService<List<Fornecedor>>();
    var fornecedor = fornecedores.FirstOrDefault(f => f.Id == id);
    if (fornecedor == null) return Results.NotFound();
    fornecedores.Remove(fornecedor);
    return Results.NoContent();
});

app.MapGet("/api/pecas", () => app.Services.GetRequiredService<List<Peca>>());

app.MapGet("/api/pecas/{id}", (string id) =>
{
    var pecas = app.Services.GetRequiredService<List<Peca>>();
    var peca = pecas.FirstOrDefault(p => p.Id == id);
    return peca == null ? Results.NotFound() : Results.Ok(peca);
});

app.MapPost("/api/pecas", (Peca peca) =>
{
    var pecas = app.Services.GetRequiredService<List<Peca>>();
    if (pecas.Any(p => p.Id == peca.Id))
        return Results.BadRequest("Peça com este ID já existe");
    pecas.Add(peca);
    return Results.Created($"/api/pecas/{peca.Id}", peca);
});

app.MapPut("/api/pecas/{id}", (string id, Peca pecaAtualizada) =>
{
    var pecas = app.Services.GetRequiredService<List<Peca>>();
    var peca = pecas.FirstOrDefault(p => p.Id == id);
    if (peca == null) return Results.NotFound();
    pecas.Remove(peca);
    pecaAtualizada.Id = id;
    pecas.Add(pecaAtualizada);
    return Results.Ok(pecaAtualizada);
});

app.MapDelete("/api/pecas/{id}", (string id) =>
{
    var pecas = app.Services.GetRequiredService<List<Peca>>();
    var peca = pecas.FirstOrDefault(p => p.Id == id);
    if (peca == null) return Results.NotFound();
    pecas.Remove(peca);
    return Results.NoContent();
});

app.MapGet("/api/vendas", () => app.Services.GetRequiredService<List<Venda>>());

app.MapGet("/api/vendas/{id}", (string id) =>
{
    var vendas = app.Services.GetRequiredService<List<Venda>>();
    var venda = vendas.FirstOrDefault(v => v.Id == id);
    return venda == null ? Results.NotFound() : Results.Ok(venda);
});

app.MapPost("/api/vendas", (Venda venda) =>
{
    var vendas = app.Services.GetRequiredService<List<Venda>>();
    if (vendas.Any(v => v.Id == venda.Id))
        return Results.BadRequest("Venda com este ID já existe");
    venda.PrecoComDesconto = venda.PrecoUnitarioVenda * (1 - venda.DescontoAplicado / 100);
    venda.ValorTotalVenda = venda.PrecoComDesconto * venda.QuantidadeVendida;
    vendas.Add(venda);
    return Results.Created($"/api/vendas/{venda.Id}", venda);
});

app.MapPut("/api/vendas/{id}", (string id, Venda vendaAtualizada) =>
{
    var vendas = app.Services.GetRequiredService<List<Venda>>();
    var venda = vendas.FirstOrDefault(v => v.Id == id);
    if (venda == null) return Results.NotFound();
    vendas.Remove(venda);
    vendaAtualizada.Id = id;
    vendaAtualizada.PrecoComDesconto = vendaAtualizada.PrecoUnitarioVenda * (1 - vendaAtualizada.DescontoAplicado / 100);
    vendaAtualizada.ValorTotalVenda = vendaAtualizada.PrecoComDesconto * vendaAtualizada.QuantidadeVendida;
    vendas.Add(vendaAtualizada);
    return Results.Ok(vendaAtualizada);
});

app.MapDelete("/api/vendas/{id}", (string id) =>
{
    var vendas = app.Services.GetRequiredService<List<Venda>>();
    var venda = vendas.FirstOrDefault(v => v.Id == id);
    if (venda == null) return Results.NotFound();
    vendas.Remove(venda);
    return Results.NoContent();
});

app.MapGet("/api/vendas/relatorio/financeiro", () =>
{
    var vendas = app.Services.GetRequiredService<List<Venda>>();
    var pecas = app.Services.GetRequiredService<List<Peca>>();
    var clientes = app.Services.GetRequiredService<List<Cliente>>();
    var fornecedores = app.Services.GetRequiredService<List<Fornecedor>>();

    var resultado = new
    {
        totalFaturamento = Math.Round(ServicoDeVendas.CalcularFaturamentoTotal(vendas), 2),
        totalLucro = Math.Round(ServicoDeVendas.CalcularLucroTotal(vendas), 2),
        totalCusto = Math.Round(ServicoDeVendas.CalcularCustoTotal(vendas, pecas), 2),
        totalVendas = vendas.Count,
        clientesAtivos = ServicoDeVendas.CalcularQuantidadeClientesAtivos(clientes),
        volumePecas = ServicoDeVendas.CalcularVolumePecasVendidas(vendas),
        ticketMedio = Math.Round(ServicoDeVendas.CalcularTicketMedio(vendas), 2),
        produtoMaisVendido = ServicoDeVendas.ObterProdutoMaisVendido(vendas, pecas),
        produtoMenosVendido = ServicoDeVendas.ObterProdutoMenosVendido(vendas, pecas),
        pecaMaisLucrativa = ServicoDeVendas.ObterPecaMaisLucrativa(vendas, pecas),
        fornecedorMaisUtilizado = ServicoDeVendas.ObterFornecedorMaisUtilizado(vendas, pecas, fornecedores)?.Nome_RazaoSocial ?? "N/A",
        margemLucro = Math.Round(ServicoDeVendas.CalcularMargemLucro(vendas), 2)
    };

    return Results.Ok(resultado);
});

Console.WriteLine("===========================================");
Console.WriteLine(" Zion System iniciando...");
Console.WriteLine(" API disponível em: http://localhost:5000");
Console.WriteLine(" Dashboard: http://localhost:5000/index.html");
Console.WriteLine("===========================================");

app.Run();