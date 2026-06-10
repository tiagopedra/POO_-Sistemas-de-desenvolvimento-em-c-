using ApidoZion.Models;
namespace ApidoZion.Routes;

public static class ROTA_DELETE
{
    public static void MapDeleteRoutes(this WebApplication app)
    {

        app.MapDelete("/api/fornecedores/{id}", (string id, List<Fornecedor> fornecedores) =>
        {
            var fornecedor = fornecedores.FirstOrDefault(f => f.Id == id);

            if (fornecedor is null)
                return Results.NotFound($"Atenção! Fornecedor com ID: '{id}' não encontrado, verifique as informações e tente novamente.");

            fornecedores.Remove(fornecedor);
            return Results.Ok(new { mensagem = $"Fornecedor '{fornecedor.Nome_RazaoSocial}' entrou em modo offline!" });
        });

        app.MapDelete("/api/clientes/{id}", (string id, List<Cliente> clientes) =>
        {
            var cliente = clientes.FirstOrDefault(c => c.Id == id);

            if (cliente is null)
                return Results.NotFound($"Cliente com ID '{id}' não encontrado, verifique as informações e tente novamente.");

            clientes.Remove(cliente);
            return Results.Ok(new { mensagem = $"O Cliente '{cliente.Nome_RazaoSocial}' entrou em modo offline." });
        });

        app.MapDelete("/api/pecas/{id}", (string id, List<Peca> pecas) =>
        {
            var peca = pecas.FirstOrDefault(p => p.Id == id);

            if (peca is null)
                return Results.NotFound($"Atenção! Peça com ID '{id}' não encontrada, por favor, verifique as informações e tente novamente.");

            pecas.Remove(peca);
            return Results.Ok(new { mensagem = $"Peça '{peca.NomePeca}' removida do estoque." });
        });

        app.MapDelete("/api/vendas/{id}", (string id, List<Venda> vendas) =>
        {
            var venda = vendas.FirstOrDefault(v => v.Id == id);

            if (venda is null)
                return Results.NotFound($"Venda com ID '{id}' não encontrada, verifique as informações e tente novamente.");

            vendas.Remove(venda);
            return Results.Ok(new { mensagem = $"Venda '{venda.NumeroNota}' removida." });
        });
    }
}


//   A Rota Delete serve para remover informações ele se comunica com o HTTP 
//através de "verbos"para indicar quando algo deve ser excluído

// L9 Sintaxe para remover por ID o Fornecedor
// L20 Remove o cliente pelo ID
// L31 Remove a peça pelo ID