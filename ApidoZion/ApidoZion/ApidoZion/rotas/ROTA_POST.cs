using ApidoZion.Models;
namespace ApidoZion.Routes;

public static class ROTA_POST
{
    public static void MapPostRoutes(this WebApplication app)
    {
        // POST para cadastrar novo fornecedor
        app.MapPost("/api/fornecedores", (List<Fornecedor> fornecedores, Fornecedor novoFornecedor) =>
        {

            if (fornecedores.Any(f => f.Id == novoFornecedor.Id))
                return Results.Conflict($"Atenção! Já existe um fornecedor com o ID '{novoFornecedor.Id}, por favor, tente novamente'.");

            if (string.IsNullOrEmpty(novoFornecedor.Nome_RazaoSocial) || 
                string.IsNullOrEmpty(novoFornecedor.CPF_CNPJ))
                return Results.BadRequest("Nome/Razão Social e CNPJ são necessários para o cadastro.");

            fornecedores.Add(novoFornecedor);
            return Results.Created($"/api/fornecedores/{novoFornecedor.Id}", novoFornecedor);
        });


        app.MapPost("/api/clientes", (List<Cliente> clientes, Cliente novoCliente) =>
        {
            if (clientes.Any(c => c.Id == novoCliente.Id))
                return Results.Conflict($"Atenção! Já existe um cliente com o ID: '{novoCliente.Id}'.");

            if (string.IsNullOrEmpty(novoCliente.Nome_RazaoSocial) || 
                string.IsNullOrEmpty(novoCliente.CPF_CNPJ))
                return Results.BadRequest("Nome/Razão Social e CPF são obrigatórios.");

            novoCliente.TotalGasto = 0;
            novoCliente.TotalLucroBullDogs = 0;
            novoCliente.QuantidadeCompras = 0;
            novoCliente.ComprasNoUltimoMes = 0;
            novoCliente.Status = novoCliente.Status ?? "online";

            clientes.Add(novoCliente);
            return Results.Created($"/api/clientes/{novoCliente.Id}", novoCliente);
        });

        app.MapPost("/api/pecas", (List<Peca> pecas, Peca novaPeca) =>
        {
            if (pecas.Any(p => p.Id == novaPeca.Id))
                return Results.Conflict($"Já existe uma peça com o ID '{novaPeca.Id}'.");

            if (string.IsNullOrEmpty(novaPeca.NomePeca) || 
                string.IsNullOrEmpty(novaPeca.Categoria))
                return Results.BadRequest("Nome da peça e categoria são obrigatórios.");

            novaPeca.QuantidadeVendida = 0;
            novaPeca.QuantidadeReposicoes = 0;
            novaPeca.LucroTotal = 0;
            novaPeca.RankingVendas = 0;
            novaPeca.EhMaisVendida = false;
            novaPeca.EhMenosVendida = false;

            pecas.Add(novaPeca);
            return Results.Created($"/api/pecas/{novaPeca.Id}", novaPeca);
        });

        app.MapPost("/api/vendas", (List<Venda> vendas, List<Cliente> clientes, List<Peca> pecas, Venda novaVenda) =>
        {
            if (vendas.Any(v => v.Id == novaVenda.Id))
                return Results.Conflict($"Atenção! Já existe uma venda com esse ID: '{novaVenda.Id}'verifique as informações e tente novamente.");

            var cliente = clientes.FirstOrDefault(c => c.Id == novaVenda.ClienteId);
            if (cliente is null)
                return Results.NotFound($"Atenção! Cliente com ID '{novaVenda.ClienteId}' não encontrado.");

            var peca = pecas.FirstOrDefault(p => p.Id == novaVenda.PecaId);
            if (peca is null)
                return Results.NotFound($"Atenção! Peça com ID: '{novaVenda.PecaId}' não encontrada.");

            if (novaVenda.QuantidadeVendida <= 0)
                return Results.BadRequest("Por favor, informe um valor maior que 0.");

            novaVenda.PrecoComDesconto = novaVenda.PrecoUnitarioVenda * (1 - (novaVenda.DescontoAplicado / 100));
            novaVenda.ValorTotalVenda = novaVenda.PrecoComDesconto * novaVenda.QuantidadeVendida;
            novaVenda.LucroLiquido = (novaVenda.ValorTotalVenda) - (peca.CustoFornecedor * novaVenda.QuantidadeVendida);
            novaVenda.PercentualLucro = (novaVenda.LucroLiquido / novaVenda.ValorTotalVenda) * 100;

            cliente.TotalGasto += novaVenda.ValorTotalVenda;
            cliente.TotalLucroBullDogs += novaVenda.LucroLiquido;
            cliente.QuantidadeCompras += 1;
            cliente.ComprasNoUltimoMes += 1;

            peca.QuantidadeVendida += novaVenda.QuantidadeVendida;
            peca.LucroTotal += novaVenda.LucroLiquido;

            vendas.Add(novaVenda);
            return Results.Created($"/api/vendas/{novaVenda.Id}", novaVenda);
        });

        
    }
}

//Informações:

// A rota post permite a criação de novos recursos, ela também carrega um payload com dados a serem processados e salvos
// (de acordo com a Microsoft)
// L8 Sintaxe para cadastrar novo fornecedor 
// L12 Valida se o ID existe
// L15 Valida os campos obrigatórios 
//L23 sintaxe para cadastrar um novo fornecedor
//L33 sintaxe que define valores gastos pelo novo cliente
//L43 Sintaxe para cadastrar nova peça
//    L53 Ele define os novos valores 
//L63 sintaxe para registar uma nova venda
//L72 a peça existe?
//L76 valida campos obrigatórios