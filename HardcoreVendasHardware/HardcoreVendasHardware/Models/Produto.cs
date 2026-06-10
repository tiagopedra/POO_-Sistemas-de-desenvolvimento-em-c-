namespace ApiVendasHardware.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Fabricante { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;

        // PrecoVenda = preço que o cliente paga (antigo "Preco")
        public decimal PrecoVenda { get; set; }

        // Alias para manter compatibilidade com código existente que usa "Preco"
        public decimal Preco
        {
            get => PrecoVenda;
            set => PrecoVenda = value;
        }

        // PrecoCusto = quanto custou para a empresa
        public decimal PrecoCusto { get; set; }

        public int EstoqueDisponivel { get; set; }
        public string Descricao { get; set; } = string.Empty;

        public bool Ativo => EstoqueDisponivel > 0;

        public decimal Margem => PrecoVenda > 0
            ? Math.Round((PrecoVenda - PrecoCusto) / PrecoVenda * 100, 2)
            : 0;

        public bool TemEstoque(int quantidade) => EstoqueDisponivel >= quantidade;

        public void ReduzirEstoque(int quantidade)
        {
            if (!TemEstoque(quantidade))
                throw new InvalidOperationException($"Estoque insuficiente para '{Nome}'.");
            EstoqueDisponivel -= quantidade;
        }

        public void ReabastecerEstoque(int quantidade)
        {
            if (quantidade <= 0)
                throw new ArgumentException("Quantidade de reabastecimento deve ser maior que zero.");
            EstoqueDisponivel += quantidade;
        }

        public override string ToString() =>
            $"[Produto #{Id}] {Nome} | {Categoria} | Venda: R$ {PrecoVenda:F2} | Custo: R$ {PrecoCusto:F2} | Estoque: {EstoqueDisponivel}";
    }
}