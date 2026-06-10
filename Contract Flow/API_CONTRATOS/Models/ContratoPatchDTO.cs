namespace ApiContratos.models
{
    public class ContratoPatchDTO
    {
        public string? Empresa { get; set; }

        public string? Nome { get; set; }

        public decimal? Valor { get; set; }

        public string? Responsavel { get; set; }

        public DateOnly? DataInicio { get; set; }

        public DateOnly? Validade { get; set; }

        public TipoFinanceiro? TipoFinanceiro { get; set; }
    }
}