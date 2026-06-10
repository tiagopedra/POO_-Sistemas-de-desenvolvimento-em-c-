namespace ApiVendasHardware.Models
{
    // Item individual dentro de um pedido
    public class ItemPedido
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; } = string.Empty;
        public int Quantidade { get; set; }
 
        // Preço pelo qual o produto foi vendido ao cliente
        public decimal PrecoUnitario { get; set; }
 
        // Custo de aquisição do produto no momento da venda
        public decimal PrecoCustoUnitario { get; set; }
 
        // Subtotal de venda (receita deste item)
        public decimal Subtotal => PrecoUnitario * Quantidade;
 
        // Custo total deste item
        public decimal CustoTotal => PrecoCustoUnitario * Quantidade;
 
        // Lucro deste item
        public decimal LucroItem => Subtotal - CustoTotal;
 
        public override string ToString() =>
            $"[Item #{Id}] {NomeProduto} x{Quantidade} | Venda: R$ {PrecoUnitario:F2} | Custo: R$ {PrecoCustoUnitario:F2} = Lucro: R$ {LucroItem:F2}";
    }
}
 