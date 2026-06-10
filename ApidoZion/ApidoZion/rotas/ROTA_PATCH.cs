using ApidoZion.Models;
namespace ApidoZion.Routes;

public static class ROTA_PATCH
{
    public static void MapPatchRoutes(this WebApplication app)
    {
        app.MapPatch("/api/fornecedores/{id}/status", (string id, List<Fornecedor> fornecedores, string novoStatus) =>
        {
            var fornecedor = fornecedores.FirstOrDefault(f => f.Id == id);

            if (fornecedor is null)
                return Results.NotFound($"Atenção! Fornecedor com ID '{id}' não encontrado, verifique as informações e tente novamente.");

            var statusValidos = new[] { "online", "offline", "sleepy" };
            if (!statusValidos.Contains(novoStatus.ToLower()))
                return Results.BadRequest("Status inválido. Use: 'online', 'offline' ou 'sleepy'.");

            fornecedor.Status = novoStatus.ToLower();

            return Results.Ok(new { mensagem = $"Status atualizado para '{novoStatus}'.", fornecedor });
        });

        app.MapPatch("/api/clientes/{id}/status", (string id, List<Cliente> clientes, string novoStatus) =>
        {
            var cliente = clientes.FirstOrDefault(c => c.Id == id);

            if (cliente is null)
                return Results.NotFound($"Atenção! Cliente com ID,'{id}' não encontrado.");

            var statusValidos = new[] { "online", "offline", "sleepy" };
            if (!statusValidos.Contains(novoStatus.ToLower()))
                return Results.BadRequest("Atenção! Status inválido. Use: 'online', 'offline' ou 'sleepy'.");

            cliente.Status = novoStatus.ToLower();

            return Results.Ok(new { mensagem = $"Status atualizado para: '{novoStatus}'.", cliente });
        });

        app.MapPatch("/api/pecas/{id}/estoque", (string id, List<Peca> pecas, int novaQuantidade) =>
        {
            var peca = pecas.FirstOrDefault(p => p.Id == id);

            if (peca is null)
                return Results.NotFound($"Atenção! Peça com ID, '{id}' não encontrada.");

            if (novaQuantidade < 0)
                return Results.BadRequest("Por favor, a quantidade de peças não pode ser inferior a 0");

            peca.QuantidadeVendida = novaQuantidade;

            return Results.Ok(new { mensagem = "Estoque atualizado!", peca });
        });
  
        app.MapPatch("/api/vendas/{id}/desconto", (string id, List<Venda> vendas, double novoDesconto) =>
        {
            var venda = vendas.FirstOrDefault(v => v.Id == id);

            if (venda is null)
                return Results.NotFound($"Atenção! Venda com ID '{id}' não encontrada.");

            if (novoDesconto < 0 || novoDesconto > 100)
                return Results.BadRequest("O desconto deve ser de 10% para PC gamer completo e 20% para as peças.");

            venda.DescontoAplicado = novoDesconto;
            venda.PrecoComDesconto = venda.PrecoUnitarioVenda * (1 - (novoDesconto / 100));

            return Results.Ok(new { mensagem = "Desconto atualizado!", venda });
        });
    }
}

//   A Rota Patch, é usada quando há desejo para alterar alguns elementos, 
//Neste cenário, foi escolhido a alteração do status do fornecedor e cliente. Ele está dividido em: 
//Online - mantém relaçoes com a empresa
//Offline - encerrou suas relações com a empresa
//Sleepy - está com tramite e documentação em andamento para integrar a empresa.

// L9 Sintaxe para atualizar fornecedor
//L24 sintaxe para alterar o status do cliente
//L40 sintaxe para alterar a quantidade 
//L51 Sintaxe para alterar as peças
//L71 Sintaxe para alterar o desconto
