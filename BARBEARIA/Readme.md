# BarberPro — API de Gestão de Barbearia

API REST desenvolvida em **C# com ASP.NET Core (Minimal API)**, acompanhada de um front-end estático servido pelo próprio servidor. O sistema gerencia agendamentos, serviços e despesas de uma barbearia, com um painel financeiro integrado.

---

## 🚀 Como executar

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Passos

```bash
# Clone ou extraia o projeto
cd BARBEARIA

# Execute a API
dotnet run --launch-profile http
```

Acesse no navegador: **[http://localhost:5000](http://localhost:5000)**

> A interface web (front-end) é servida automaticamente pelo servidor na rota `/`.

---

## 📁 Estrutura do Projeto

```
BARBEARIA/
├── Program.cs                    → Ponto de entrada: configura e inicia o servidor
├── Dados.cs                      → Repositório de dados em memória (listas estáticas)
│
├── Models/
│   ├── Agendamento.cs            → Modelo: agendamento de cliente
│   ├── Produtos.cs               → Modelo: serviço oferecido (com preço)
│   └── Despesas.cs               → Modelo: despesa da barbearia
│
├── Routes/
│   ├── ROTA_GET_AGENDAMENTO.cs   → GET  /api/agendamentos e /api/agendamentos/{id}
│   ├── ROTA_POST_AGENDAMENTO.cs  → POST /api/agendamentos | PUT /api/agendamentos/{id}
│   ├── ROTA_DELETE_AGENDAMENTO.cs→ DELETE /api/agendamentos/{id}
│   ├── ROTA_GET_PRODUTOS.cs      → GET  /api/produtos e /api/produtos/{id}
│   ├── ROTA_POST_PRODUTOS.cs     → POST /api/produtos | PUT /api/produtos/{id}
│   ├── ROTA_DELETE_PRODUTOS.cs   → DELETE /api/produtos/{id}
│   ├── ROTA_GET_DESPESAS.cs      → GET  /api/despesas e /api/despesas/{id}
│   ├── ROTA_POST_DEPESAS.CS      → POST /api/despesas | PUT /api/despesas/{id}
│   ├── ROTA_DELETE_DESPESAS.cs   → DELETE /api/despesas/{id}
│   └── ROTA_FINCANCEIRO.cs       → GET  /api/financeiro (resumo financeiro)
│
├── wwwroot/
│   ├── index.html                → Interface web (layout com sidebar)
│   ├── style.css                 → Estilos (Inter font, variáveis CSS, responsivo)
│   └── app.js                    → Lógica front-end: fetch, render, modais, toast
│
└── Properties/
    └── launchSettings.json       → Perfil HTTP na porta 5000
```

---

## 🔌 Endpoints da API

### Agendamentos

| Método | Rota                        | Descrição                          |
|--------|-----------------------------|------------------------------------|
| GET    | `/api/agendamentos`         | Lista todos os agendamentos        |
| GET    | `/api/agendamentos/{id}`    | Retorna um agendamento pelo ID     |
| POST   | `/api/agendamentos`         | Cria novo agendamento              |
| PUT    | `/api/agendamentos/{id}`    | Atualiza agendamento existente     |
| DELETE | `/api/agendamentos/{id}`    | Exclui um agendamento              |

**Corpo POST/PUT:**
```json
{
  "cliente":    "João Silva",
  "servico":    "Corte Degradê",
  "data":       "2026-06-10",
  "horario":    "14:00",
  "observacao": "Prefere tesoura"
}
```

---

### Serviços (Produtos)

| Método | Rota                   | Descrição                      |
|--------|------------------------|--------------------------------|
| GET    | `/api/produtos`        | Lista todos os serviços        |
| GET    | `/api/produtos/{id}`   | Retorna um serviço pelo ID     |
| POST   | `/api/produtos`        | Cria novo serviço              |
| PUT    | `/api/produtos/{id}`   | Atualiza serviço existente     |
| DELETE | `/api/produtos/{id}`   | Exclui um serviço              |

**Corpo POST/PUT:**
```json
{
  "nome":      "Corte Degradê",
  "descricao": "Corte moderno com máquina e tesoura",
  "preco":     45.00
}
```

---

### Despesas

| Método | Rota                    | Descrição                      |
|--------|-------------------------|--------------------------------|
| GET    | `/api/despesas`         | Lista todas as despesas        |
| GET    | `/api/despesas/{id}`    | Retorna uma despesa pelo ID    |
| POST   | `/api/despesas`         | Registra nova despesa          |
| PUT    | `/api/despesas/{id}`    | Atualiza despesa existente     |
| DELETE | `/api/despesas/{id}`    | Exclui uma despesa             |

**Corpo POST/PUT:**
```json
{
  "descricao": "Aluguel",
  "valor":     1800.00
}
```

---

### Financeiro

| Método | Rota              | Descrição                              |
|--------|-------------------|----------------------------------------|
| GET    | `/api/financeiro` | Resumo de receitas, despesas e lucro   |

**Resposta:**
```json
{
  "totalReceitas": 12500.00,
  "totalDespesas": 3200.00,
  "lucro": 9300.00,
  "situacao": "Positivo",
  "detalhes": {
    "totalAgendamentos": 95,
    "despesas": [
      { "descricao": "Aluguel", "valor": 1800.00 }
    ]
  }
}
```

> **Cálculo:** Para cada agendamento, o sistema busca o serviço correspondente em `Produtos` pelo campo `Nome` e soma o `Preco`. O total de despesas é a soma de todos os registros em `Dados.Despesas`.

---

## 🧠 Conceitos aplicados

| Conceito               | Onde é aplicado                                      |
|------------------------|------------------------------------------------------|
| Minimal API            | `Program.cs` + métodos de extensão em cada rota      |
| Métodos de extensão    | `MapGetAgendamentosRoutes(this WebApplication app)`  |
| Dados em memória       | `Dados.cs` com listas estáticas compartilhadas       |
| CRUD completo          | GET, POST, PUT, DELETE em todas as entidades         |
| Arquivos estáticos     | `UseStaticFiles()` + `UseDefaultFiles()` no servidor |
| Front-end integrado    | `wwwroot/` servido pelo próprio ASP.NET Core         |
| Separação por camadas  | Models / Routes / Dados / wwwroot                    |

---

## ⚠️ Observações

- **Dados em memória:** os dados são reiniciados a cada execução. Em um sistema real, seria necessário um banco de dados (ex: SQL Server, SQLite).
- **Vínculo serviço–agendamento:** o campo `Agendamento.Servico` deve corresponder exatamente ao `Produtos.Nome` para o cálculo financeiro funcionar corretamente.
- **CORS:** se acessar o front-end de outra origem, pode ser necessário configurar CORS no `Program.cs`.

---

## 👨‍💻 Tecnologias

- **C# / .NET 10** — backend
- **ASP.NET Core Minimal API** — roteamento HTTP
- **HTML5 + CSS3 + JavaScript (ES6)** — front-end
- **Google Fonts (Inter)** — tipografia
