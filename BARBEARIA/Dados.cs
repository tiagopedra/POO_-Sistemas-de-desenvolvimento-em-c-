// ============================================================
// Dados.cs — Repositório de dados em memória
//
// Responsabilidade:
//   - Armazenar os dados da aplicação durante a execução
//   - Funcionar como banco de dados temporário (in-memory)
//   - Compartilhar as listas entre todos os arquivos de rota
//
// Conteúdo:
//   Agendamentos → Lista de agendamentos de clientes
//   Produtos     → Lista de serviços oferecidos pela barbearia (com preço)
//   Despesas     → Lista de despesas fixas/variáveis da barbearia
//
// Importante:
//   Por ser uma classe estática com listas em memória, os dados
//   são resetados toda vez que o servidor reinicia.
// ============================================================

using ApiBarbearia.Models;

namespace ApiBarbearia
{
    // Classe estática com listas compartilhadas entre todas as rotas
    // Todos os arquivos de rota usam essas mesmas listas
    public static class Dados
    {
        public static List<Agendamento> Agendamentos { get; } = new List<Agendamento>
        {
            new Agendamento { Id = 1,   Cliente = "Carlos Silva",         Servico = "Corte Social",                Data = "2026-06-05", Horario = "09:00", Observacao = "Prefere tesoura" },
            new Agendamento { Id = 2,   Cliente = "João Pereira",         Servico = "Corte Degradê",               Data = "2026-06-05", Horario = "10:00", Observacao = "" },
            new Agendamento { Id = 3,   Cliente = "Pedro Souza",          Servico = "Corte Navalhado",             Data = "2026-06-05", Horario = "11:00", Observacao = "" },
            new Agendamento { Id = 4,   Cliente = "Lucas Almeida",        Servico = "Corte Americano",             Data = "2026-06-05", Horario = "13:00", Observacao = "" },
            new Agendamento { Id = 5,   Cliente = "Rafael Costa",         Servico = "Corte Cacheado",              Data = "2026-06-05", Horario = "14:00", Observacao = "" },
            new Agendamento { Id = 6,   Cliente = "Bruno Oliveira",       Servico = "Barba Completa",              Data = "2026-06-05", Horario = "15:00", Observacao = "" },
            new Agendamento { Id = 7,   Cliente = "Fernando Lima",        Servico = "Barba Manutenção",            Data = "2026-06-05", Horario = "16:00", Observacao = "" },
            new Agendamento { Id = 8,   Cliente = "Gustavo Rocha",        Servico = "Sobrancelha",                 Data = "2026-06-05", Horario = "17:00", Observacao = "" },
            new Agendamento { Id = 9,   Cliente = "Thiago Martins",       Servico = "Sobrancelha Premium",         Data = "2026-06-06", Horario = "09:00", Observacao = "" },
            new Agendamento { Id = 10,  Cliente = "Diego Fernandes",      Servico = "Luzes Capilar",               Data = "2026-06-06", Horario = "10:00", Observacao = "" },
            new Agendamento { Id = 11,  Cliente = "Ricardo Santos",       Servico = "Pigmentação Capilar",         Data = "2026-06-06", Horario = "11:00", Observacao = "" },
            new Agendamento { Id = 12,  Cliente = "Marcelo Alves",        Servico = "Tingimento Completo",         Data = "2026-06-06", Horario = "13:00", Observacao = "" },
            new Agendamento { Id = 13,  Cliente = "Felipe Souza",         Servico = "Nevou (Corte + Hidratação)",  Data = "2026-06-06", Horario = "14:00", Observacao = "" },
            new Agendamento { Id = 14,  Cliente = "André Luiz",           Servico = "Hidratação Capilar",          Data = "2026-06-06", Horario = "15:00", Observacao = "" },
            new Agendamento { Id = 15,  Cliente = "Gabriel Lima",         Servico = "Combo Clássico",              Data = "2026-06-06", Horario = "16:00", Observacao = "" },
            new Agendamento { Id = 16,  Cliente = "Vinicius Dias",        Servico = "Combo Premium",               Data = "2026-06-07", Horario = "09:00", Observacao = "" },
            new Agendamento { Id = 17,  Cliente = "Henrique Melo",        Servico = "Combo Completo",              Data = "2026-06-07", Horario = "10:00", Observacao = "" },
            new Agendamento { Id = 18,  Cliente = "Eduardo Castro",       Servico = "Combo Tratamento",            Data = "2026-06-07", Horario = "11:00", Observacao = "" },
            new Agendamento { Id = 19,  Cliente = "Leonardo Nunes",       Servico = "Corte Social",                Data = "2026-06-07", Horario = "13:00", Observacao = "" },
            new Agendamento { Id = 20,  Cliente = "Caio Ferreira",        Servico = "Barba Completa",              Data = "2026-06-07", Horario = "14:00", Observacao = "" },
            new Agendamento { Id = 21,  Cliente = "Lucas Mendes",         Servico = "Corte Degradê",               Data = "2026-06-07", Horario = "15:00", Observacao = "" },
            new Agendamento { Id = 22,  Cliente = "Matheus Silva",        Servico = "Sobrancelha",                 Data = "2026-06-07", Horario = "16:00", Observacao = "" },
            new Agendamento { Id = 23,  Cliente = "Daniel Oliveira",      Servico = "Combo Clássico",              Data = "2026-06-08", Horario = "09:00", Observacao = "" },
            new Agendamento { Id = 24,  Cliente = "Victor Santos",        Servico = "Nevou (Corte + Hidratação)",  Data = "2026-06-08", Horario = "10:00", Observacao = "" },
            new Agendamento { Id = 25,  Cliente = "Arthur Pereira",       Servico = "Corte Navalhado",             Data = "2026-06-08", Horario = "11:00", Observacao = "" },
            new Agendamento { Id = 26,  Cliente = "Samuel Costa",         Servico = "Pigmentação Capilar",         Data = "2026-06-08", Horario = "13:00", Observacao = "" },
            new Agendamento { Id = 27,  Cliente = "Igor Lima",            Servico = "Hidratação Capilar",          Data = "2026-06-08", Horario = "14:00", Observacao = "" },
            new Agendamento { Id = 28,  Cliente = "Lucas Rocha",          Servico = "Combo Premium",               Data = "2026-06-08", Horario = "15:00", Observacao = "" },
            new Agendamento { Id = 29,  Cliente = "Felipe Martins",       Servico = "Corte Americano",             Data = "2026-06-08", Horario = "16:00", Observacao = "" },
            new Agendamento { Id = 30,  Cliente = "Guilherme Fernandes",  Servico = "Combo Completo",              Data = "2026-06-09", Horario = "09:00", Observacao = "" },
            new Agendamento { Id = 31,  Cliente = "Lucas Souza",          Servico = "Corte Social",                Data = "2026-06-09", Horario = "10:00", Observacao = "" },
            new Agendamento { Id = 32,  Cliente = "João Victor",          Servico = "Barba Manutenção",            Data = "2026-06-09", Horario = "11:00", Observacao = "" },
            new Agendamento { Id = 33,  Cliente = "Pedro Henrique",       Servico = "Luzes Capilar",               Data = "2026-06-09", Horario = "13:00", Observacao = "" },
            new Agendamento { Id = 34,  Cliente = "Rafael Alves",         Servico = "Tingimento Completo",         Data = "2026-06-09", Horario = "14:00", Observacao = "" },
            new Agendamento { Id = 35,  Cliente = "Bruno Lima",           Servico = "Sobrancelha Premium",         Data = "2026-06-09", Horario = "15:00", Observacao = "" },
            new Agendamento { Id = 36,  Cliente = "Fernando Costa",       Servico = "Combo Tratamento",            Data = "2026-06-09", Horario = "16:00", Observacao = "" },
            new Agendamento { Id = 37,  Cliente = "Gustavo Silva",        Servico = "Corte Cacheado",              Data = "2026-06-10", Horario = "09:00", Observacao = "" },
            new Agendamento { Id = 38,  Cliente = "Thiago Pereira",       Servico = "Barba Completa",              Data = "2026-06-10", Horario = "10:00", Observacao = "" },
            new Agendamento { Id = 39,  Cliente = "Diego Souza",          Servico = "Corte Degradê",               Data = "2026-06-10", Horario = "11:00", Observacao = "" },
            new Agendamento { Id = 40,  Cliente = "Ricardo Rocha",        Servico = "Corte Navalhado",             Data = "2026-06-10", Horario = "13:00", Observacao = "" },
            new Agendamento { Id = 41,  Cliente = "Marcelo Martins",      Servico = "Combo Clássico",              Data = "2026-06-10", Horario = "14:00", Observacao = "" },
            new Agendamento { Id = 42,  Cliente = "Felipe Fernandes",     Servico = "Sobrancelha",                 Data = "2026-06-10", Horario = "15:00", Observacao = "" },
            new Agendamento { Id = 43,  Cliente = "André Santos",         Servico = "Nevou (Corte + Hidratação)",  Data = "2026-06-10", Horario = "16:00", Observacao = "" },
            new Agendamento { Id = 44,  Cliente = "Gabriel Alves",        Servico = "Combo Premium",               Data = "2026-06-11", Horario = "09:00", Observacao = "" },
            new Agendamento { Id = 45,  Cliente = "Vinicius Lima",        Servico = "Pigmentação Capilar",         Data = "2026-06-11", Horario = "10:00", Observacao = "" },
            new Agendamento { Id = 46,  Cliente = "Henrique Costa",       Servico = "Corte Americano",             Data = "2026-06-11", Horario = "11:00", Observacao = "" },
            new Agendamento { Id = 47,  Cliente = "Eduardo Rocha",        Servico = "Hidratação Capilar",          Data = "2026-06-11", Horario = "13:00", Observacao = "" },
            new Agendamento { Id = 48,  Cliente = "Leonardo Martins",     Servico = "Combo Completo",              Data = "2026-06-11", Horario = "14:00", Observacao = "" },
            new Agendamento { Id = 49,  Cliente = "Caio Fernandes",       Servico = "Luzes Capilar",               Data = "2026-06-11", Horario = "15:00", Observacao = "" },
            new Agendamento { Id = 50,  Cliente = "Lucas Santos",         Servico = "Tingimento Completo",         Data = "2026-06-11", Horario = "16:00", Observacao = "" },
            new Agendamento { Id = 51,  Cliente = "Matheus Souza",        Servico = "Corte Social",                Data = "2026-06-12", Horario = "09:00", Observacao = "" },
            new Agendamento { Id = 52,  Cliente = "Daniel Alves",         Servico = "Barba Completa",              Data = "2026-06-12", Horario = "10:00", Observacao = "" },
            new Agendamento { Id = 53,  Cliente = "Victor Lima",          Servico = "Sobrancelha Premium",         Data = "2026-06-12", Horario = "11:00", Observacao = "" },
            new Agendamento { Id = 54,  Cliente = "Arthur Costa",         Servico = "Combo Tratamento",            Data = "2026-06-12", Horario = "13:00", Observacao = "" },
            new Agendamento { Id = 55,  Cliente = "Samuel Rocha",         Servico = "Corte Degradê",               Data = "2026-06-12", Horario = "14:00", Observacao = "" },
            new Agendamento { Id = 56,  Cliente = "Igor Martins",         Servico = "Corte Navalhado",             Data = "2026-06-12", Horario = "15:00", Observacao = "" },
            new Agendamento { Id = 57,  Cliente = "Lucas Fernandes",      Servico = "Barba Manutenção",            Data = "2026-06-12", Horario = "16:00", Observacao = "" },
            new Agendamento { Id = 58,  Cliente = "Guilherme Santos",     Servico = "Combo Clássico",              Data = "2026-06-13", Horario = "09:00", Observacao = "" },
            new Agendamento { Id = 59,  Cliente = "Felipe Alves",         Servico = "Nevou (Corte + Hidratação)",  Data = "2026-06-13", Horario = "10:00", Observacao = "" },
            new Agendamento { Id = 60,  Cliente = "João Lima",            Servico = "Corte Cacheado",              Data = "2026-06-13", Horario = "11:00", Observacao = "" },
            new Agendamento { Id = 61,  Cliente = "Pedro Costa",          Servico = "Combo Premium",               Data = "2026-06-13", Horario = "13:00", Observacao = "" },
            new Agendamento { Id = 62,  Cliente = "Rafael Rocha",         Servico = "Pigmentação Capilar",         Data = "2026-06-13", Horario = "14:00", Observacao = "" },
            new Agendamento { Id = 63,  Cliente = "Bruno Martins",        Servico = "Corte Americano",             Data = "2026-06-13", Horario = "15:00", Observacao = "" },
            new Agendamento { Id = 64,  Cliente = "Fernando Fernandes",   Servico = "Hidratação Capilar",          Data = "2026-06-13", Horario = "16:00", Observacao = "" },
            new Agendamento { Id = 65,  Cliente = "Gustavo Santos",       Servico = "Combo Completo",              Data = "2026-06-14", Horario = "09:00", Observacao = "" },
            new Agendamento { Id = 66,  Cliente = "Thiago Alves",         Servico = "Sobrancelha",                 Data = "2026-06-14", Horario = "10:00", Observacao = "" },
            new Agendamento { Id = 67,  Cliente = "Diego Lima",           Servico = "Luzes Capilar",               Data = "2026-06-14", Horario = "11:00", Observacao = "" },
            new Agendamento { Id = 68,  Cliente = "Ricardo Costa",        Servico = "Tingimento Completo",         Data = "2026-06-14", Horario = "13:00", Observacao = "" },
            new Agendamento { Id = 69,  Cliente = "Marcelo Rocha",        Servico = "Combo Tratamento",            Data = "2026-06-14", Horario = "14:00", Observacao = "" },
            new Agendamento { Id = 70,  Cliente = "Felipe Martins",       Servico = "Corte Social",                Data = "2026-06-14", Horario = "15:00", Observacao = "" },
            new Agendamento { Id = 71,  Cliente = "André Fernandes",      Servico = "Barba Completa",              Data = "2026-06-14", Horario = "16:00", Observacao = "" },
            new Agendamento { Id = 72,  Cliente = "Gabriel Santos",       Servico = "Corte Degradê",               Data = "2026-06-15", Horario = "09:00", Observacao = "" },
            new Agendamento { Id = 73,  Cliente = "Vinicius Alves",       Servico = "Sobrancelha Premium",         Data = "2026-06-15", Horario = "10:00", Observacao = "" },
            new Agendamento { Id = 74,  Cliente = "Henrique Lima",        Servico = "Corte Navalhado",             Data = "2026-06-15", Horario = "11:00", Observacao = "" },
            new Agendamento { Id = 75,  Cliente = "Eduardo Costa",        Servico = "Barba Manutenção",            Data = "2026-06-15", Horario = "13:00", Observacao = "" },
            new Agendamento { Id = 76,  Cliente = "Leonardo Rocha",       Servico = "Combo Clássico",              Data = "2026-06-15", Horario = "14:00", Observacao = "" },
            new Agendamento { Id = 77,  Cliente = "Caio Martins",         Servico = "Nevou (Corte + Hidratação)",  Data = "2026-06-15", Horario = "15:00", Observacao = "" },
            new Agendamento { Id = 78,  Cliente = "Lucas Fernandes",      Servico = "Corte Cacheado",              Data = "2026-06-15", Horario = "16:00", Observacao = "" },
            new Agendamento { Id = 79,  Cliente = "Matheus Santos",       Servico = "Combo Premium",               Data = "2026-06-16", Horario = "09:00", Observacao = "" },
            new Agendamento { Id = 80,  Cliente = "Daniel Alves",         Servico = "Pigmentação Capilar",         Data = "2026-06-16", Horario = "10:00", Observacao = "" },
            new Agendamento { Id = 81,  Cliente = "Victor Lima",          Servico = "Corte Americano",             Data = "2026-06-16", Horario = "11:00", Observacao = "" },
            new Agendamento { Id = 82,  Cliente = "Arthur Costa",         Servico = "Hidratação Capilar",          Data = "2026-06-16", Horario = "13:00", Observacao = "" },
            new Agendamento { Id = 83,  Cliente = "Samuel Rocha",         Servico = "Combo Completo",              Data = "2026-06-16", Horario = "14:00", Observacao = "" },
            new Agendamento { Id = 84,  Cliente = "Igor Martins",         Servico = "Luzes Capilar",               Data = "2026-06-16", Horario = "15:00", Observacao = "" },
            new Agendamento { Id = 85,  Cliente = "Lucas Fernandes",      Servico = "Tingimento Completo",         Data = "2026-06-16", Horario = "16:00", Observacao = "" },
            new Agendamento { Id = 86,  Cliente = "Guilherme Santos",     Servico = "Combo Tratamento",            Data = "2026-06-17", Horario = "09:00", Observacao = "" },
            new Agendamento { Id = 87,  Cliente = "Felipe Alves",         Servico = "Corte Social",                Data = "2026-06-17", Horario = "10:00", Observacao = "" },
            new Agendamento { Id = 88,  Cliente = "João Lima",            Servico = "Barba Completa",              Data = "2026-06-17", Horario = "11:00", Observacao = "" },
            new Agendamento { Id = 89,  Cliente = "Pedro Costa",          Servico = "Corte Degradê",               Data = "2026-06-17", Horario = "13:00", Observacao = "" },
            new Agendamento { Id = 90,  Cliente = "Rafael Rocha",         Servico = "Sobrancelha",                 Data = "2026-06-17", Horario = "14:00", Observacao = "" },
            new Agendamento { Id = 91,  Cliente = "Bruno Martins",        Servico = "Corte Navalhado",             Data = "2026-06-17", Horario = "15:00", Observacao = "" },
            new Agendamento { Id = 92,  Cliente = "Fernando Fernandes",   Servico = "Barba Manutenção",            Data = "2026-06-17", Horario = "16:00", Observacao = "" },
            new Agendamento { Id = 93,  Cliente = "Gustavo Santos",       Servico = "Combo Clássico",              Data = "2026-06-18", Horario = "09:00", Observacao = "" },
            new Agendamento { Id = 94,  Cliente = "Thiago Alves",         Servico = "Nevou (Corte + Hidratação)",  Data = "2026-06-18", Horario = "10:00", Observacao = "" },
            new Agendamento { Id = 95,  Cliente = "Diego Lima",           Servico = "Corte Cacheado",              Data = "2026-06-18", Horario = "11:00", Observacao = "" },
            new Agendamento { Id = 96,  Cliente = "Ricardo Costa",        Servico = "Combo Premium",               Data = "2026-06-18", Horario = "13:00", Observacao = "" },
            new Agendamento { Id = 97,  Cliente = "Marcelo Rocha",        Servico = "Pigmentação Capilar",         Data = "2026-06-18", Horario = "14:00", Observacao = "" },
            new Agendamento { Id = 98,  Cliente = "Felipe Martins",       Servico = "Corte Americano",             Data = "2026-06-18", Horario = "15:00", Observacao = "" },
            new Agendamento { Id = 99,  Cliente = "André Fernandes",      Servico = "Hidratação Capilar",          Data = "2026-06-18", Horario = "16:00", Observacao = "" },
            new Agendamento { Id = 100, Cliente = "Gabriel Santos",       Servico = "Combo Completo",              Data = "2026-06-19", Horario = "09:00", Observacao = "Cliente preferencial" },
        };

        public static List<Despesa> Despesas { get; } = new List<Despesa>
        {
            new Despesa { Id = 1, Descricao = "Aluguel do espaço",          Valor = 1500.00m },
            new Despesa { Id = 2, Descricao = "Produtos e insumos",         Valor =  850.00m },
            new Despesa { Id = 3, Descricao = "Energia elétrica",           Valor =  320.00m },
            new Despesa { Id = 4, Descricao = "Água",                       Valor =  150.00m },
            new Despesa { Id = 5, Descricao = "Internet e Telefone",        Valor =  180.00m },
            new Despesa { Id = 6, Descricao = "Material de limpeza",        Valor =  120.00m },
            new Despesa { Id = 7, Descricao = "Manutenção de equipamentos", Valor =  200.00m },
            new Despesa { Id = 8, Descricao = "Publicidade e Marketing",    Valor =  300.00m },
        };

        public static List<Produtos> Produtos { get; } = new List<Produtos>
        {
            // ── Cortes ──────────────────────────────────────────────────────────────────
            new Produtos { Id = 1,  Nome = "Corte Social",               Descricao = "Corte clássico com tesoura, acabamento refinado",              Preco =  35.00m },
            new Produtos { Id = 2,  Nome = "Corte Degradê",              Descricao = "Degradê progressivo nas laterais com finalização no topo",     Preco =  45.00m },
            new Produtos { Id = 3,  Nome = "Corte Navalhado",            Descricao = "Laterais raspadas na navalha com contraste marcado",           Preco =  50.00m },
            new Produtos { Id = 4,  Nome = "Corte Americano",            Descricao = "Estilo clássico americano com lateral curta e topo longo",     Preco =  40.00m },
            new Produtos { Id = 5,  Nome = "Corte Cacheado",             Descricao = "Especializado em cabelos cacheados e crespos",                 Preco =  45.00m },

            // ── Barba ───────────────────────────────────────────────────────────────────
            new Produtos { Id = 6,  Nome = "Barba Completa",             Descricao = "Modelagem completa da barba com navalha e toalha quente",      Preco =  35.00m },
            new Produtos { Id = 7,  Nome = "Barba Manutenção",           Descricao = "Alinhamento e aparado da barba",                              Preco =  25.00m },

            // ── Sobrancelha ─────────────────────────────────────────────────────────────
            new Produtos { Id = 8,  Nome = "Sobrancelha",                Descricao = "Design e alinhamento de sobrancelha masculina",                Preco =  20.00m },
            new Produtos { Id = 9,  Nome = "Sobrancelha Premium",        Descricao = "Sobrancelha com coloração e design premium",                   Preco =  35.00m },

            // ── Tratamentos Capilares ────────────────────────────────────────────────────
            new Produtos { Id = 10, Nome = "Luzes Capilar",              Descricao = "Aplicação de luzes para efeito desbotado",                     Preco =  85.00m },
            new Produtos { Id = 11, Nome = "Pigmentação Capilar",        Descricao = "Cobertura de falhas com pigmento natural",                     Preco =  60.00m },
            new Produtos { Id = 12, Nome = "Tingimento Completo",        Descricao = "Tingimento de cabelo com cores modernas",                      Preco =  95.00m },
            new Produtos { Id = 13, Nome = "Nevou (Corte + Hidratação)", Descricao = "Tratamento especial para cabelo ressecado e espanado",         Preco =  75.00m },
            new Produtos { Id = 14, Nome = "Hidratação Capilar",         Descricao = "Tratamento profundo para cabelos ressecados",                  Preco =  55.00m },

            // ── Combos ──────────────────────────────────────────────────────────────────
            new Produtos { Id = 15, Nome = "Combo Clássico",             Descricao = "Corte + Barba completa",                                       Preco =  65.00m },
            new Produtos { Id = 16, Nome = "Combo Premium",              Descricao = "Corte Degradê + Barba Completa + Sobrancelha",                 Preco =  95.00m },
            new Produtos { Id = 17, Nome = "Combo Completo",             Descricao = "Corte + Barba + Sobrancelha + Luzes",                          Preco = 155.00m },
            new Produtos { Id = 18, Nome = "Combo Tratamento",           Descricao = "Corte + Hidratação + Pigmentação",                             Preco = 135.00m },

            // ── Produtos para venda ──────────────────────────────────────────────────────
            new Produtos { Id = 19, Nome = "Gel de Cabelo",              Descricao = "Gel forte para fixação de penteado",                           Preco =  25.00m },
            new Produtos { Id = 20, Nome = "Pomada Brilho",              Descricao = "Pomada com brilho natural",                                    Preco =  30.00m },
            new Produtos { Id = 21, Nome = "Espuma para Barba",          Descricao = "Espuma de qualidade para barbear",                             Preco =  28.00m },
            new Produtos { Id = 22, Nome = "Óleo de Barba",              Descricao = "Óleo hidratante para barba",                                   Preco =  35.00m },
            new Produtos { Id = 23, Nome = "Shampoo Barba",              Descricao = "Shampoo específico para limpeza de barba",                     Preco =  32.00m },
            new Produtos { Id = 24, Nome = "Condicionador Barba",        Descricao = "Condicionador para maciez da barba",                           Preco =  32.00m },
            new Produtos { Id = 25, Nome = "Tônico Capilar",             Descricao = "Tônico para fortalecimento capilar",                           Preco =  40.00m },
            new Produtos { Id = 26, Nome = "Loção Pós-Barba",            Descricao = "Loção desinfetante pós-barbear",                               Preco =  28.00m },
            new Produtos { Id = 27, Nome = "Máscara Capilar",            Descricao = "Máscara profunda para hidratação",                             Preco =  38.00m },

            // ── Equipamentos ─────────────────────────────────────────────────────────────
            new Produtos { Id = 28, Nome = "Tesoura de Corte",           Descricao = "Tesoura profissional para barbeiro",                           Preco = 120.00m },
            new Produtos { Id = 29, Nome = "Navalha Clássica",           Descricao = "Navalha afiada para acabamento",                               Preco =  85.00m },
        };
    }
}
