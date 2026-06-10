using ApiVendasHardware.Models;
using ApiVendasHardware.Repositories;
 
namespace ApiVendasHardware.Services
{
    // Centraliza validações e regras de negócio fora das rotas
    public class PedidoService
    {
        private readonly HardwareRepository _repo;
 
        public PedidoService(HardwareRepository repo)
        {
            _repo = repo;
        }
 
        // Valida cliente, estoque e popula preços; retorna erro ou null se ok
        public string? ValidarEPreparar(Pedido pedido)
        {
            var cliente = _repo.ObterClientePorId(pedido.ClienteId);
            if (cliente is null)
                return $"Cliente com ID {pedido.ClienteId} não encontrado.";
 
            if (!cliente.EmailValido())
                return $"E-mail do cliente '{cliente.Email}' é inválido.";
 
            if (pedido.Itens == null || !pedido.Itens.Any())
                return "O pedido deve conter ao menos um item.";
 
            int proximoItemId = 1;
 
            foreach (var item in pedido.Itens)
            {
                var produto = _repo.ObterProdutoPorId(item.ProdutoId);
 
                if (produto is null)
                    return $"Produto com ID {item.ProdutoId} não encontrado.";
 
                if (item.Quantidade <= 0)
                    return $"Quantidade inválida para o produto '{produto.Nome}'.";
 
                if (!produto.TemEstoque(item.Quantidade))
                    return $"Estoque insuficiente para '{produto.Nome}'. Disponível: {produto.EstoqueDisponivel}.";
 
                item.Id                  = proximoItemId++;
                item.NomeProduto         = produto.Nome;
                item.PrecoUnitario       = produto.PrecoVenda;   // preço de venda
                item.PrecoCustoUnitario  = produto.PrecoCusto;   // custo — salvo para cálculo de lucro
 
                produto.ReduzirEstoque(item.Quantidade);
            }
 
            return null;
        }
    }
}