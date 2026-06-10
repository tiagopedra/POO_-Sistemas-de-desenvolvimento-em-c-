// ============================================================
// Models/Agendamento.cs — Modelo de dados: Agendamento
//
// Responsabilidade:
//   - Representar um agendamento de cliente na barbearia
//   - Ser usado como tipo nas listas de Dados.cs e nas rotas
//
// Campos:
//   Id         → Identificador único gerado automaticamente
//   Cliente    → Nome do cliente que realizou o agendamento
//   Servico    → Nome do serviço contratado (deve existir em Produtos)
//   Data       → Data do atendimento (DateOnly, serializado como "yyyy-MM-dd")
//   Horario    → Hora do atendimento (TimeOnly, serializado como "HH:mm")
//   Observacao → Informações adicionais (alergias, preferências, etc.)
// ============================================================

namespace ApiBarbearia.Models
{
    public class Agendamento
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public string Servico { get; set; }
        public DateOnly Data { get; set; }
        public TimeOnly Horario { get; set; }
        public string Observacao { get; set; }
    }
}
