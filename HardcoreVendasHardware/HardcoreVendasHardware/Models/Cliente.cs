namespace ApiVendasHardware.Models
{
    // Entidade de cliente com validação de e-mail encapsulada
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;

        public bool EmailValido()
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(Email);
                return addr.Address == Email;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString() =>
            $"[Cliente #{Id}] {Nome} | {Email}";
    }
}
