using ApidoZion.Models;
namespace ApidoZion.Routes;

public static class ROTA_PUT
{
    public static void MapPutRoutes(this WebApplication app)
    {

        app.MapPut("/api/fornecedores/{id}", (string id, List<Fornecedor> fornecedores, Fornecedor fornecedorAtualizado) =>
        {
            var fornecedor = fornecedores.FirstOrDefault(f => f.Id == id);

            if (fornecedor is null)
                return Results.NotFound($"Atenção! Fornecedor, '{id}' não encontrado, verifique as informações e tente novamente.");

            fornecedor.Nome_RazaoSocial = fornecedorAtualizado.Nome_RazaoSocial;
            fornecedor.CPF_CNPJ = fornecedorAtualizado.CPF_CNPJ;
            fornecedor.Email = fornecedorAtualizado.Email;
            fornecedor.Telefone = fornecedorAtualizado.Telefone;
            fornecedor.Endereco = fornecedorAtualizado.Endereco;
            fornecedor.Status = fornecedorAtualizado.Status;
            fornecedor.Pecas = fornecedorAtualizado.Pecas;
            fornecedor.Valor = fornecedorAtualizado.Valor;
            fornecedor.Quantidade = fornecedorAtualizado.Quantidade;

            return Results.Ok(new { mensagem = "Fornecedor atualizado!", fornecedor });
        });

        app.MapPut("/api/clientes/{id}", (string id, List<Cliente> clientes, Cliente clienteAtualizado) =>
        {
            var cliente = clientes.FirstOrDefault(c => c.Id == id);

            if (cliente is null)
                return Results.NotFound($"Atenção! Cliente, '{id}' não encontrado, verifique as informações e tente novamente.");

            cliente.Nome_RazaoSocial = clienteAtualizado.Nome_RazaoSocial;
            cliente.CPF_CNPJ = clienteAtualizado.CPF_CNPJ;
            cliente.Email = clienteAtualizado.Email;
            cliente.Telefone = clienteAtualizado.Telefone;
            cliente.Endereco = clienteAtualizado.Endereco;
            cliente.Status = clienteAtualizado.Status;
            cliente.TotalGasto = clienteAtualizado.TotalGasto;
            cliente.TotalLucroBullDogs = clienteAtualizado.TotalLucroBullDogs;
            cliente.QuantidadeCompras = clienteAtualizado.QuantidadeCompras;
            cliente.PecaMaisComprada = clienteAtualizado.PecaMaisComprada;
            cliente.ComprasNoUltimoMes = clienteAtualizado.ComprasNoUltimoMes;

            return Results.Ok(new { mensagem = "Cliente atualizado!", cliente });
        });

        app.MapPut("/api/pecas/{id}", (string id, List<Peca> pecas, Peca pecaAtualizada) =>
        {
            var peca = pecas.FirstOrDefault(p => p.Id == id);

            if (peca is null)
                return Results.NotFound($"Atenção! Peça com ID, '{id}' não encontrada, verifique as informações e tente novamente.");

            peca.NomePeca = pecaAtualizada.NomePeca;
            peca.Categoria = pecaAtualizada.Categoria;
            peca.Marca = pecaAtualizada.Marca;
            peca.FornecedorId = pecaAtualizada.FornecedorId;
            peca.CustoFornecedor = pecaAtualizada.CustoFornecedor;
            peca.PrecoVenda = pecaAtualizada.PrecoVenda;
            peca.MargemLucro = pecaAtualizada.MargemLucro;
            peca.QuantidadeVendida = pecaAtualizada.QuantidadeVendida;
            peca.QuantidadeReposicoes = pecaAtualizada.QuantidadeReposicoes;
            peca.LucroTotal = pecaAtualizada.LucroTotal;
            peca.RankingVendas = pecaAtualizada.RankingVendas;
            peca.EhMaisVendida = pecaAtualizada.EhMaisVendida;
            peca.EhMenosVendida = pecaAtualizada.EhMenosVendida;

            return Results.Ok(new { mensagem = "Peça atualizada com sucesso!", peca });
        });

        app.MapPut("/api/vendas/{id}", (string id, List<Venda> vendas, Venda vendaAtualizada) =>
        {
            var venda = vendas.FirstOrDefault(v => v.Id == id);

            if (venda is null)
                return Results.NotFound($"Venda com ID, '{id}' não encontrada, verifique as informações e tente novamente.");

            venda.NumeroNota = vendaAtualizada.NumeroNota;
            venda.ClienteId = vendaAtualizada.ClienteId;
            venda.PecaId = vendaAtualizada.PecaId;
            venda.QuantidadeVendida = vendaAtualizada.QuantidadeVendida;
            venda.PrecoUnitarioVenda = vendaAtualizada.PrecoUnitarioVenda;
            venda.DescontoAplicado = vendaAtualizada.DescontoAplicado;
            venda.PrecoComDesconto = vendaAtualizada.PrecoComDesconto;
            venda.ValorTotalVenda = vendaAtualizada.ValorTotalVenda;
            venda.LucroLiquido = vendaAtualizada.LucroLiquido;
            venda.PercentualLucro = vendaAtualizada.PercentualLucro;

            return Results.Ok(new { mensagem = "Venda atualizada!", venda });
        });
    }
}

// Informações
// L9 Sintaxe para atualizar todos os fornecedores
//L16 Todos os dados dos fornecedores sãoa tualizados conforme a demanda
//L30 Sintaxe para atualizar os dados do cliente
//L51 Sintaxe para atualizar as peças
//L75 Sintaxe para atualizar a venda completa. 
//    O if executa uma condição, se o ID não for encontrado no banco de memórias, os dados dos 
//fornecedores, clientes, peças e vendas, o sistema apresentará uma mensagem de erro.
