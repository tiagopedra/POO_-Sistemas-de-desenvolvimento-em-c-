namespace ApiContratos.models
{// O enum define quais status um contrato pode ter.
    public enum StatusContrato
    {
        Ativo,
        PertoDeVencer,
        Vencido
    }
    // O enum define o tipo do contrato.
    public enum TipoFinanceiro
{
    Entrada,
    Saida
}
// Esta classe define quais informações um contrato terá na API.
    public class Contrato
    {
        public string Empresa { get;set;} = string.Empty; // Nome da empresa relacionada ao contrato.
        public int Id { get;set;} // Identificador único do contrato.
        public string Nome { get;set;} = string.Empty; // Nome do contrato ou nome do cliente/serviço contratado.
        public decimal Valor { get;set;} // Valor financeiro do contrato.
        public string Responsavel { get;set;} = string.Empty; // Pessoa responsável pelo contrato.
        public DateOnly DataInicio { get;set;} // Data de início do contrato.
        public DateOnly Validade { get;set;} //Data de vencimento do contrato.
        public TipoFinanceiro TipoFinanceiro { get; set; }//define se o contrato e de entrada ou saida de dinheiro

        // Calcula quantos dias faltam para o contrato vencer.
        public int DiasParaVencer { get 
            {// Pega a data de hoje do computador. // DateOnly.FromDateTime remove a parte da hora. 
                DateOnly hoje = DateOnly.FromDateTime(DateTime.Today);
                return Validade.DayNumber - hoje.DayNumber;
            }
        }
        // Esta propriedade calcula automaticamente o status do contrato.
        public StatusContrato Status{ get
        {
            DateOnly hoje = DateOnly.FromDateTime(DateTime.Today);
              
            int diasRestantes = Validade.DayNumber - hoje.DayNumber;

            if (diasRestantes < 0)
            {
                return StatusContrato.Vencido;
            }

            if (diasRestantes <= 7)
            {
                return StatusContrato.PertoDeVencer;
            }

            return StatusContrato.Ativo;
        }     

        } 
    }
    
  



}