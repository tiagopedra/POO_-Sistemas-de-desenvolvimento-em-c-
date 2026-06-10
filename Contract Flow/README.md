# Contract Flow — Sistema de Gerenciamento de Contratos

Sistema web completo para gerenciamento de contratos empresariais, desenvolvido como trabalho acadêmico. Composto por uma **API REST em C#** integrada a um **frontend em HTML, CSS e JavaScript**.

---

## Descrição do Projeto

O **Contract Flow** permite que empresas acompanhem e gerenciem seus contratos de forma centralizada. O sistema calcula automaticamente o status de cada contrato com base na data de validade e exibe alertas visuais para contratos próximos do vencimento.

### Funcionalidades

| Funcionalidade | Descrição |
|---|---|
| Listagem de contratos | Tabela completa com paginação e busca |
| Cadastro | Formulário para criar novos contratos |
| Edição | Atualização completa (PUT) ou parcial (PATCH) |
| Exclusão | Remoção com confirmação |
| Status automático | Calculado com base na data de validade |
| Filtros | Por status e período de validade |
| Exportação | Relatório em CSV e JSON |
| Impressão | Layout otimizado para impressão |

---

## Arquitetura do Sistema

```
API_CONTRATOS/
│
├── Models/
│   └── Contratos.cs               ← Estrutura de dados e status automático
│
├── Repositorio/
│   └── ContratoRepositorio.cs     ← Banco de dados em memória
│
├── Routes/
│   ├── ROTA_GET.cs                ← Leitura de contratos (GET)
│   ├── ROTA_POST.cs               ← Criação de contratos (POST)
│   ├── ROTA_PUT.cs                ← Atualização completa (PUT)
│   ├── ROTA_PATCH.cs              ← Atualização parcial (PATCH)
│   └── ROTA_DELETE.cs             ← Exclusão de contratos (DELETE)
│
├── wwwroot/
│   ├── index.html                 ← Estrutura da interface
│   ├── style.css                  ← Estilos visuais
│   └── script.js                  ← Lógica e integração com a API
│
├── Properties/
│   └── launchSettings.json        ← Porta e configurações de execução
│
└── Program.cs                     ← Configuração e inicialização do servidor
```

---

## Como Executar

### Requisitos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- Navegador moderno (Chrome, Firefox, Edge)

### Passo a passo

**1. Clone o repositório:**
```bash
git clone https://github.com/cristopher-almeida/API_CONTRATOS.git
cd API_CONTRATOS
```

**2. Execute a API:**
```bash
dotnet run
```

**3. Acesse no navegador:**
```
http://localhost:5043
```

O próprio servidor já serve o frontend automaticamente a partir da pasta `wwwroot`.

---

## Endpoints da API

Base URL: `http://localhost:5043`

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/api/contratos` | Lista todos os contratos |
| GET | `/api/contratos/{id}` | Busca contrato por ID |
| GET | `/api/contratos/ativos` | Lista contratos ativos e próximos do vencimento |
| GET | `/api/contratos/inativos` | Lista contratos vencidos |
| POST | `/api/contratos` | Cria um novo contrato |
| PUT | `/api/contratos/{id}` | Atualiza todos os campos de um contrato |
| PATCH | `/api/contratos/{id}` | Atualiza apenas os campos enviados |
| DELETE | `/api/contratos/{id}` | Remove um contrato |

### Exemplo de corpo da requisição (POST/PUT)

```json
{
  "empresa": "TechNova Sistemas",
  "nome": "Contrato de suporte técnico",
  "valor": 2500.00,
  "responsavel": "Carlos Mendes",
  "tipoFinanceiro": "Entrada",
  "dataInicio": "2026-01-10",
  "validade": "2026-05-20"
}
```

### Exemplo de resposta

```json
{
  "id": 1,
  "empresa": "TechNova Sistemas",
  "nome": "Contrato de suporte técnico",
  "valor": 2500.00,
  "responsavel": "Carlos Mendes",
  "dataInicio": "2026-01-10",
  "validade": "2026-05-20",
  "diasParaVencer": 5,
  "status": "PertoDeVencer"
}
```

---

## Modelo de Dados

### Status do Contrato

O status é calculado **automaticamente** com base nos dias restantes até a validade:

| Condição | Status |
|---|---|
| Dias restantes > 7 | `Ativo` |
| Dias restantes entre 0 e 7 | `PertoDeVencer` |
| Dias restantes < 0 | `Vencido` |

### Campos do Contrato

| Campo | Tipo | Descrição |
|---|---|---|
| `Id` | int | Identificador único (gerado automaticamente) |
| `Empresa` | string | Nome da empresa |
| `Nome` | string | Descrição do contrato |
| `Valor` | decimal | Valor financeiro em reais |
| `Responsavel` | string | Pessoa responsável |
| `DataInicio` | DateOnly | Data de início |
| `Validade` | DateOnly | Data de vencimento |
| `Diasparavencer` | int | Calculado automaticamente |
| `Status` | enum | Calculado automaticamente |

---

## Interface Web

O frontend é composto por três arquivos na pasta `wwwroot/`:

- **`index.html`** — estrutura da página (barra lateral, indicadores, tabela, formulários)
- **`style.css`** — visual do sistema com variáveis de cor e animações
- **`script.js`** — integração com a API via `fetch()`, filtros, paginação e exportação

### Tecnologias do frontend

- HTML5, CSS3, JavaScript (ES6+)
- [Font Awesome 6.5](https://fontawesome.com/) — ícones
- [Google Fonts (DM Sans)](https://fonts.google.com/specimen/DM+Sans) — tipografia

---

## Branches

| Branch | Responsável | Conteúdo |
|---|---|---|
| `main` | — | Código principal integrado |
| `back-cris` | Christopher | API C# (rotas, modelo, repositório) |
| `front-matheus` | Matheus | Frontend (HTML, CSS, JS) e rota PATCH |

---

## Autores

Desenvolvido como trabalho acadêmico na disciplina de **Programação Orientada a Objetos**

**FESP-PR — Análise e Desenvolvimento de Sistemas — 2026**

| Nome | Responsabilidade |
|---|---|
| Christopher | API REST em C# |
| Matheus | Frontend e integração |
| Talita | — |