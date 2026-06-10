using ApidoZion.Models;
namespace ApidoZion.Routes;
using ApidoZion.Services;  // ← ADICIONE NO TOPO

public static class ROTA_GET
{
    public static void MapGetRoutes(this WebApplication app)
    {
        app.MapGet("/api/fornecedores", (List<Fornecedor> fornecedores) =>
        {
            return Results.Ok(fornecedores);
        });

        app.MapGet("/api/fornecedores/{id}", (string id, List<Fornecedor> fornecedores) =>
        {
            var fornecedor = fornecedores.FirstOrDefault(f => f.Id == id);
            return fornecedor is null
                ? Results.NotFound($"Atenção! Fornecedor com ID '{id}' não encontrado, verifique as informações e tente novamente.")
                : Results.Ok(fornecedor);
        });

        app.MapGet("/api/fornecedores/status/{status}", (string status, List<Fornecedor> fornecedores) =>
        {
            var resultado = fornecedores.Where(f => f.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            return resultado.Count == 0
                ? Results.NotFound($"Atenção! Não encontramos nenhum fornecedor com status '{status}'.")
                : Results.Ok(resultado);
        });

        app.MapGet("/api/clientes", (List<Cliente> clientes) =>
        {
            return Results.Ok(clientes);
        });

        app.MapGet("/api/clientes/{id}", (string id, List<Cliente> clientes) =>
        {
            var cliente = clientes.FirstOrDefault(c => c.Id == id);
            return cliente is null
                ? Results.NotFound($"Atenção! Cliente com ID: '{id}' não encontrado.")
                : Results.Ok(cliente);
        });

        app.MapGet("/api/clientes/status/{status}", (string status, List<Cliente> clientes) =>
        {
            var resultado = clientes.Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            return resultado.Count == 0
                ? Results.NotFound($"Atenção! Não encontramos nenhum cliente com status '{status}' verifique as informações e tente novamente.")
                : Results.Ok(resultado);
        });

  
        app.MapGet("/api/pecas", (List<Peca> pecas) =>
        {
            return Results.Ok(pecas);
        });

        app.MapGet("/api/pecas/{id}", (string id, List<Peca> pecas) =>
        {
            var peca = pecas.FirstOrDefault(p => p.Id == id);
            return peca is null
                ? Results.NotFound($"Peça com ID: '{id}' não encontrada, verifique as informações e tente novamente.")
                : Results.Ok(peca);
        });

        app.MapGet("/api/pecas/categoria/{categoria}", (string categoria, List<Peca> pecas) =>
        {
            var resultado = pecas.Where(p => p.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase)).ToList();
            return resultado.Count == 0
                ? Results.NotFound($"Atenção! Nenhuma peça na categoria '{categoria}'.")
                : Results.Ok(resultado);
        });

        app.MapGet("/api/vendas", (List<Venda> vendas) =>
        {
            return Results.Ok(vendas);
        });

        // GET - Busca venda por ID
        app.MapGet("/api/vendas/{id}", (string id, List<Venda> vendas) =>
        {
            var venda = vendas.FirstOrDefault(v => v.Id == id);
            return venda is null
                ? Results.NotFound($"Atenção! Venda com ID '{id}' não encontrada, verifique as informações e tente novamente.")
                : Results.Ok(venda);
        });


app.MapGet("/api/vendas/relatorio/financeiro", (List<Venda> vendas, List<Peca> pecas, List<Cliente> clientes) =>
{
    double totalFaturamento = ServicoDeVendas.CalcularFaturamentoTotal(vendas);
    double totalLucro = ServicoDeVendas.CalcularLucroTotal(vendas);
    double totalCusto = ServicoDeVendas.CalcularCustoTotal(vendas, pecas);
    double margemLucro = ServicoDeVendas.CalcularMargemLucro(vendas);
    int totalVendas = vendas.Count;
    int totalClientes = clientes.Count;
    int volumePecas = ServicoDeVendas.CalcularVolumePecasVendidas(vendas);
    double ticketMedio = ServicoDeVendas.CalcularTicketMedio(vendas);
    string pecaMaisVendida = ServicoDeVendas.ObterProdutoMaisVendido(vendas, pecas);
    string pecaMenosVendida = ServicoDeVendas.ObterProdutoMenosVendido(vendas, pecas);
    
    var relatorio = new
    {
        TotalFaturamento = totalFaturamento,
        TotalLucro = totalLucro,
        TotalCusto = totalCusto,
        MargemLucro = margemLucro,
        TicketMedio = ticketMedio,
        TotalVendas = totalVendas,
        TotalClientes = totalClientes,
        VolumePecasVendidas = volumePecas,
        PecaMaisVendida = pecaMaisVendida,
        PecaMenosVendida = pecaMenosVendida
    };
    
    return Results.Ok(relatorio);
});
    }
}


//referências de construção:
// https://pt.stackoverflow.com/questions/178465/buscar-item-em-uma-lista-usando-linq acesso em 16/05/2026


//Sistema de Gestão de Vendas ele deve permitir o cadastro de cliente, controle de peças em estoque e sesus valores

//relatório sobre vendas contendo volume de peças vendidos faturamento total, lucro presumido e quantidade de clientes
//atendidos.
//mostrar o produto mais vendido e o menos vendido. 
//10% PC inteiro e 20% na peça

//  A Rota Get  usada para buscar, consultar ou recuperar dados. É uma EndPoint
//Neste cenário, foi escolhido a alteração do status do fornecedor e cliente. Ele está dividido em: 
//Online - mantém relaçoes com a empresa
// - encerrou suas relações com a empresa
//Sleepy - está com tramite e documentação em andamento para integrar a empresa.

// L8 Sintaxe para listar todos os fornecedores
// L13 buscar o fornecedor pelo ID
// L21 filtra o fornecedor por status
//L29 sintaxe para listar todos os cliente
//L34 sintaxe para procurar o cliente pelo ID cliente
//L43 Sintaxe para filtrar o cliente pelo status
//L50 Sintaxe que lista por todas as peças 
//L56 Sintaxe que busca por ID. 
//L64 Filtra a peça por categoria
//L72 Lista todas as vendas. 
//L77 Busca por ID 

//L86 - Código para relatório das vendas - solução e resumo financeiro
