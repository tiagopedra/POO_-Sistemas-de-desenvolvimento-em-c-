namespace ApiVendasHardware.Models
{
    public enum StatusPedido
    {
        Pendente,
        Confirmado,
        EmProcessamento,
        Enviado,
        Entregue,
        Cancelado
    }

    // Registro de cada transição de status do pedido
    public class HistoricoStatus
    {
        public StatusPedido Status { get; set; }
        public DateTime AlteradoEm { get; set; } = DateTime.UtcNow;
    }

    // Resumo financeiro calculado sob demanda
    public class ResumoFinanceiro
    {
        public decimal ValorBruto { get; set; }
        public decimal Desconto { get; set; }
        public decimal ValorFinal => ValorBruto - Desconto;
    }

    // Pedido de compra vinculado a um cliente
    public class Pedido
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public DateTime DataPedido { get; set; } = DateTime.UtcNow;
        public StatusPedido Status { get; set; } = StatusPedido.Pendente;
        public List<ItemPedido> Itens { get; set; } = new();
        public List<HistoricoStatus> Historico { get; set; } = new();
        public string? Observacoes { get; set; }

        // Valor total calculado a partir dos itens
        public decimal ValorTotal => Itens.Sum(i => i.Subtotal);

        public ResumoFinanceiro ObterResumo(decimal desconto = 0) => new()
        {
            ValorBruto = ValorTotal,
            Desconto   = desconto
        };

        public void Confirmar()
        {
            if (Status != StatusPedido.Pendente)
                throw new InvalidOperationException("Apenas pedidos pendentes podem ser confirmados.");
            AlterarStatus(StatusPedido.Confirmado);
        }

        public void Cancelar()
        {
            if (Status == StatusPedido.Enviado || Status == StatusPedido.Entregue)
                throw new InvalidOperationException("Não é possível cancelar um pedido já enviado ou entregue.");
            AlterarStatus(StatusPedido.Cancelado);
        }

        public void AvancarStatus()
        {
            var proximo = Status switch
            {
                StatusPedido.Pendente        => StatusPedido.Confirmado,
                StatusPedido.Confirmado      => StatusPedido.EmProcessamento,
                StatusPedido.EmProcessamento => StatusPedido.Enviado,
                StatusPedido.Enviado         => StatusPedido.Entregue,
                _ => throw new InvalidOperationException($"Não é possível avançar o status '{Status}'.")
            };
            AlterarStatus(proximo);
        }

        // Centraliza mudança de status e registra no histórico
        private void AlterarStatus(StatusPedido novoStatus)
        {
            Status = novoStatus;
            Historico.Add(new HistoricoStatus { Status = novoStatus });
        }

        public override string ToString() =>
            $"[Pedido #{Id}] Cliente #{ClienteId} | {Status} | R$ {ValorTotal:F2} | {DataPedido:dd/MM/yyyy}";
    }
}
