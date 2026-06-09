# BEM WMS

## Warehouse Management System de Controle de Entrada e Saída de Estoque

## Descrição do Projeto

O **BEM WMS** é um sistema de controle de estoque desenvolvido para gerenciar materiais escolares, seus lotes, entradas, saídas e indicadores financeiros.

O projeto foi desenvolvido com **ASP.NET Core Minimal API** no back-end e **HTML, CSS e JavaScript** no front-end. A aplicação permite cadastrar materiais, controlar lotes, registrar saídas de estoque, editar dados cadastrados, remover materiais e visualizar informações consolidadas em um dashboard.

A interface foi construída em modo **dark**, com design minimalista, organizada para facilitar o uso das principais funcionalidades do sistema.

---

## Integrantes

* Bruno Batista Xavier
* Emily Kristin Garcia
* Maria Eduarda Martins de Souza

---

## Objetivo do Projeto

O objetivo do **BEM WMS** é simular um sistema de gerenciamento de estoque para materiais escolares, permitindo o controle de entrada e saída de itens de forma organizada.

O sistema busca atender às seguintes necessidades:

* Cadastrar materiais escolares;
* Controlar materiais por lote;
* Registrar entrada de estoque;
* Registrar saída de estoque;
* Consultar materiais ativos e inativos;
* Editar materiais cadastrados;
* Remover materiais do sistema;
* Visualizar dados financeiros;
* Calcular lucro estimado;
* Calcular ROI;
* Consultar uma tabela geral de itens, códigos e lotes;
* Apresentar uma interface web simples e funcional.

---

## Tecnologias Utilizadas

### Back-end

* C#
* ASP.NET Core
* Minimal API
* Swagger
* .NET 10.0
* CORS

### Front-end

* HTML
* CSS
* JavaScript
* Live Server

---

## Estrutura do Projeto

```text
ApiProdutos
├── Data
│   └── BancoSimulado.cs
│
├── Models
│   ├── MaterialEscolar.cs
│   ├── Lote.cs
│   └── SaidaEstoque.cs
│
├── Routes
│   ├── ROTA_GET.cs
│   ├── ROTA_GET_ATIVOS.cs
│   ├── ROTA_GET_INATIVOS.cs
│   ├── ROTA_POST.cs
│   ├── ROTA_PUT.cs
│   ├── ROTA_DELETE.cs
│   ├── ROTA_SAIDA.cs
│   └── ROTA_DASHBOARD.cs
│
├── frontend
│   ├── index.html
│   ├── style.css
│   └── script.js
│
├── Properties
│   └── launchSettings.json
│
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
└── ApiProdutos.csproj
```

---

## Entidades do Sistema

## MaterialEscolar

A entidade `MaterialEscolar` representa o item principal controlado pelo sistema.

| Atributo  | Tipo       | Descrição                                  |
| --------- | ---------- | ------------------------------------------ |
| Id        | int        | Identificador único do material            |
| Nome      | string     | Nome do material escolar                   |
| Categoria | string     | Categoria do material                      |
| Ativo     | bool       | Define se o material está ativo ou inativo |
| Lotes     | List<Lote> | Lista de lotes vinculados ao material      |

Exemplos de materiais:

* Caderno Universitário
* Caneta Azul
* Cola Branca
* Lápis Grafite
* Borracha
* Régua
* Tesoura Escolar
* Estojo Escolar
* Tinta Guache
* Massa de Modelar

---

## Lote

A entidade `Lote` representa os lotes vinculados aos materiais escolares.

| Atributo       | Tipo      | Descrição                                  |
| -------------- | --------- | ------------------------------------------ |
| Id             | int       | Identificador único do lote                |
| Codigo         | string    | Código do lote                             |
| Quantidade     | int       | Quantidade disponível no lote              |
| DataEntrada    | DateTime  | Data de entrada do lote no estoque         |
| DataVencimento | DateTime? | Data de vencimento do lote, quando existir |
| ValorCusto     | decimal   | Valor de custo unitário do item            |
| ValorVenda     | decimal   | Valor de venda unitário do item            |

A data de vencimento é opcional, pois alguns materiais possuem validade, como cola, tinta e massa de modelar, enquanto outros não possuem, como caderno, lápis e régua.

---

## SaidaEstoque

A entidade `SaidaEstoque` representa a requisição usada para retirar materiais do estoque.

| Atributo   | Tipo | Descrição                               |
| ---------- | ---- | --------------------------------------- |
| MaterialId | int  | ID do material que terá saída           |
| Quantidade | int  | Quantidade que será retirada do estoque |

---

## Funcionalidades do Sistema

## 1. Cadastro de Materiais

O sistema permite cadastrar novos materiais escolares informando:

* ID do material;
* Nome do material;
* Categoria;
* Status ativo ou inativo;
* ID do lote;
* Código do lote;
* Quantidade em estoque;
* Valor de custo unitário;
* Valor de venda unitário;
* Data de entrada;
* Data de vencimento, quando existir.

Essa funcionalidade utiliza a rota:

```text
POST /api/materiais
```

---

## 2. Listagem de Materiais

O sistema permite listar todos os materiais cadastrados, exibindo:

* Nome;
* ID;
* Categoria;
* Status;
* Estoque total;
* Valor investido;
* Venda estimada;
* Lucro estimado;
* ROI;
* Lotes vinculados;
* Quantidade por lote;
* Custo unitário;
* Venda unitária.

Essa funcionalidade utiliza a rota:

```text
GET /api/materiais
```

---

## 3. Consulta de Materiais Ativos

O sistema permite visualizar apenas os materiais ativos.

Essa funcionalidade utiliza a rota:

```text
GET /api/materiais/ativos
```

---

## 4. Consulta de Materiais Inativos

O sistema permite visualizar apenas os materiais inativos.

Essa funcionalidade utiliza a rota:

```text
GET /api/materiais/inativos
```

---

## 5. Consulta por ID

O sistema permite consultar um material específico pelo seu ID.

Essa funcionalidade utiliza a rota:

```text
GET /api/materiais/{id}
```

---

## 6. Consulta de Lotes por Material

O sistema permite consultar todos os lotes vinculados a um material específico.

Essa funcionalidade utiliza a rota:

```text
GET /api/materiais/{id}/lotes
```

---

## 7. Edição de Material

O sistema permite editar um material já cadastrado.

Ao clicar em **Editar material**, os dados do item são carregados no formulário. Depois da alteração, o sistema utiliza a rota `PUT` para atualizar as informações.

Essa funcionalidade permite editar:

* Nome;
* Categoria;
* Status;
* Dados do lote;
* Quantidade;
* Data de entrada;
* Data de vencimento;
* Valor de custo;
* Valor de venda.

Rota utilizada:

```text
PUT /api/materiais/{id}
```

---

## 8. Remoção de Material

O sistema permite remover materiais cadastrados.

Essa funcionalidade utiliza a rota:

```text
DELETE /api/materiais/{id}
```

---

## 9. Registro de Saída de Estoque

O sistema permite registrar a saída de materiais do estoque.

Na tela, o usuário seleciona o material desejado e informa a quantidade que será retirada.

Essa funcionalidade utiliza a rota:

```text
POST /api/materiais/saida
```

Exemplo de requisição:

```json
{
  "materialId": 1,
  "quantidade": 10
}
```

---

## 10. Controle de Saída por Lote

A saída de estoque considera os lotes disponíveis do material.

Quando o material possui data de vencimento, o sistema prioriza os lotes que vencem primeiro.

Essa regra é conhecida como:

```text
FEFO - First Expire, First Out
```

Ou seja:

```text
Primeiro que vence, primeiro que sai.
```

Para materiais sem data de vencimento, o sistema utiliza a data de entrada como critério de ordenação.

---

## 11. Dashboard Financeiro

O **BEM WMS** possui um dashboard financeiro que apresenta os principais indicadores do estoque.

O dashboard exibe:

* Total de materiais;
* Total de itens ativos;
* Total de itens inativos;
* Total de unidades em estoque;
* Valor investido;
* Venda estimada;
* Lucro estimado;
* ROI percentual.

Essa funcionalidade utiliza a rota:

```text
GET /api/dashboard
```

---

## 12. Dashboard Financeiro por Material

Além dos cards principais, o sistema apresenta uma tabela financeira por material.

A tabela exibe:

* Material;
* Categoria;
* Estoque;
* Valor investido;
* Venda estimada;
* Lucro;
* ROI.

Essa visualização permite analisar quais materiais possuem maior retorno financeiro estimado.

---

## 13. Tabela de Itens, Códigos e Lotes

O sistema também possui uma tabela de consulta rápida com todos os itens, seus códigos e lotes.

A tabela apresenta:

* ID do material;
* Nome do material;
* Categoria;
* Status;
* ID do lote;
* Código do lote;
* Quantidade;
* Data de entrada;
* Data de vencimento;
* Custo unitário;
* Venda unitária.

Essa funcionalidade facilita a conferência dos dados cadastrados e a visualização dos lotes vinculados a cada item.

---

## 14. Filtros no Front-end

A interface possui filtros para melhorar a navegação:

* Todos;
* Ativos;
* Inativos;
* Dashboard;
* Cadastro;
* Tabela de Itens;
* Materiais.

Esses botões permitem navegar rapidamente entre as seções da tela e filtrar os dados exibidos.

---

## Endpoints da API

| Método | Rota                        | Descrição                                      |
| ------ | --------------------------- | ---------------------------------------------- |
| GET    | `/`                         | Verifica se a API está funcionando             |
| GET    | `/api/materiais`            | Lista todos os materiais cadastrados           |
| GET    | `/api/materiais/{id}`       | Busca um material pelo ID                      |
| GET    | `/api/materiais/{id}/lotes` | Lista os lotes de um material                  |
| GET    | `/api/materiais/ativos`     | Lista materiais ativos                         |
| GET    | `/api/materiais/inativos`   | Lista materiais inativos                       |
| POST   | `/api/materiais`            | Cadastra um novo material                      |
| PUT    | `/api/materiais/{id}`       | Atualiza um material cadastrado                |
| DELETE | `/api/materiais/{id}`       | Remove um material pelo ID                     |
| POST   | `/api/materiais/saida`      | Registra saída de estoque                      |
| GET    | `/api/dashboard`            | Retorna indicadores financeiros e operacionais |

---

## Como Executar o Back-end

## 1. Acessar a pasta do projeto

No terminal, acesse a pasta onde está o arquivo `.csproj`:

```bash
cd C:\Users\emily.garcia\WMS_API\WAREHOUSESYSTEM_BACKEND\ApiProdutos
```

---

## 2. Restaurar dependências

```bash
dotnet restore
```

---

## 3. Executar a API

```bash
dotnet run
```

Ao executar, será exibida uma mensagem parecida com:

```text
Now listening on: http://localhost:5181
```

Essa será a URL base da API.

---

## 4. Acessar o Swagger

Com a API rodando, abra no navegador:

```text
http://localhost:5181/swagger
```

O Swagger permite testar todas as rotas da API diretamente pelo navegador.

---

## Como Executar o Front-end

O front-end está localizado na pasta:

```text
frontend
```

Arquivos principais:

```text
frontend
├── index.html
├── style.css
└── script.js
```

---

## 1. Conferir a URL da API

No arquivo `script.js`, verifique se a constante `API_URL` está apontando para a URL correta da API.

Exemplo:

```javascript
const API_URL = "http://localhost:5181";
```

Caso a API rode em outra porta, altere essa linha.

---

## 2. Abrir com Live Server

No VS Code:

1. Clique com o botão direito no arquivo `index.html`;
2. Clique em **Open with Live Server**;
3. O navegador abrirá uma URL parecida com:

```text
http://127.0.0.1:5500/frontend/index.html
```

A API precisa permanecer rodando no terminal com:

```bash
dotnet run
```

---

## Como Usar o Sistema

## 1. Abrir o Sistema

Primeiro execute o back-end com:

```bash
dotnet run
```

Depois abra o front-end com Live Server.

---

## 2. Visualizar o Dashboard

Ao abrir o sistema, a área superior exibe os cards do dashboard.

Os cards mostram:

* Quantidade total de materiais;
* Quantidade de ativos;
* Quantidade de inativos;
* Total de unidades em estoque;
* Valor investido;
* Venda estimada;
* Lucro estimado;
* ROI.

---

## 3. Cadastrar um Material

Na seção **Cadastrar material escolar**, preencha os campos do material e do lote.

Depois clique em:

```text
Cadastrar material
```

O material será enviado para a API por meio da rota:

```text
POST /api/materiais
```

---

## 4. Editar um Material

Na seção **Materiais cadastrados**, clique em:

```text
Editar material
```

O sistema carrega os dados no formulário de cadastro.

Depois de alterar as informações, clique em:

```text
Salvar alterações
```

O sistema enviará os dados atualizados para a API por meio da rota:

```text
PUT /api/materiais/{id}
```

---

## 5. Cancelar uma Edição

Caso o usuário clique em editar e não queira salvar as alterações, basta clicar em:

```text
Cancelar edição
```

O formulário será limpo e voltará ao modo de cadastro.

---

## 6. Registrar Saída de Estoque

Na seção **Registrar saída de estoque**:

1. Selecione o material;
2. Informe a quantidade de saída;
3. Clique em **Registrar saída**.

O sistema atualiza a quantidade disponível em estoque e recalcula os indicadores financeiros.

---

## 7. Remover um Material

Na seção **Materiais cadastrados**, clique em:

```text
Remover material
```

Após confirmar, o sistema remove o material usando a rota:

```text
DELETE /api/materiais/{id}
```

---

## Exemplos de Requisições

## Cadastrar Material

```json
{
  "id": 13,
  "nome": "Mochila Escolar",
  "categoria": "Mochila",
  "ativo": true,
  "lotes": [
    {
      "id": 13,
      "codigo": "MOC001",
      "quantidade": 20,
      "dataEntrada": "2026-04-10T00:00:00",
      "dataVencimento": null,
      "valorCusto": 45.90,
      "valorVenda": 89.90
    }
  ]
}
```

---

## Editar Material

```json
{
  "id": 13,
  "nome": "Mochila Escolar Reforçada",
  "categoria": "Mochila",
  "ativo": true,
  "lotes": [
    {
      "id": 13,
      "codigo": "MOC001",
      "quantidade": 20,
      "dataEntrada": "2026-04-10T00:00:00",
      "dataVencimento": null,
      "valorCusto": 45.90,
      "valorVenda": 99.90
    }
  ]
}
```

Rota:

```text
PUT /api/materiais/13
```

---

## Registrar Saída

```json
{
  "materialId": 13,
  "quantidade": 5
}
```

Rota:

```text
POST /api/materiais/saida
```

---

## Cálculos Financeiros

## Valor Investido

O valor investido é calculado com base na quantidade em estoque e no valor de custo unitário.

```text
Valor Investido = Quantidade em Estoque × Valor de Custo Unitário
```

---

## Venda Estimada

A venda estimada é calculada com base na quantidade em estoque e no valor de venda unitário.

```text
Venda Estimada = Quantidade em Estoque × Valor de Venda Unitário
```

---

## Lucro Estimado

O lucro estimado representa a diferença entre a venda estimada e o valor investido.

```text
Lucro Estimado = Venda Estimada - Valor Investido
```

---

## ROI

O ROI representa o retorno sobre o investimento em percentual.

```text
ROI (%) = (Lucro Estimado / Valor Investido) × 100
```

---

## Organização do Código

## Program.cs

Arquivo principal da aplicação.

Responsável por:

* Criar a aplicação;
* Configurar Swagger;
* Configurar CORS;
* Registrar as rotas da API;
* Iniciar a execução do sistema.

Rotas registradas:

```csharp
app.MapGetRoutes();
app.MapGetAtivosRoutes();
app.MapGetInativosRoutes();
app.MapPostRoutes();
app.MapPutRoutes();
app.MapDeleteRoutes();
app.MapSaidaRoutes();
app.MapDashboardRoutes();
```

---

## Data/BancoSimulado.cs

Arquivo responsável por armazenar os dados simulados em memória.

Como o projeto não utiliza banco de dados, os dados cadastrados ficam armazenados temporariamente em uma lista.

---

## Models/MaterialEscolar.cs

Define a estrutura da entidade `MaterialEscolar`.

---

## Models/Lote.cs

Define a estrutura da entidade `Lote`.

---

## Models/SaidaEstoque.cs

Define a estrutura da requisição de saída de estoque.

---

## Routes/ROTA_GET.cs

Contém as rotas de consulta geral:

* Rota raiz;
* Listagem de materiais;
* Consulta por ID;
* Consulta de lotes por material.

---

## Routes/ROTA_GET_ATIVOS.cs

Contém a rota responsável por listar somente os materiais ativos.

---

## Routes/ROTA_GET_INATIVOS.cs

Contém a rota responsável por listar somente os materiais inativos.

---

## Routes/ROTA_POST.cs

Contém a rota responsável pelo cadastro de novos materiais.

---

## Routes/ROTA_PUT.cs

Contém a rota responsável pela edição de materiais cadastrados.

---

## Routes/ROTA_DELETE.cs

Contém a rota responsável pela remoção de materiais.

---

## Routes/ROTA_SAIDA.cs

Contém a rota responsável pela saída de estoque por lote.

---

## Routes/ROTA_DASHBOARD.cs

Contém a rota responsável por retornar os indicadores do dashboard.

---

## Observação Sobre Armazenamento

Este projeto utiliza dados simulados em memória.

Isso significa que os materiais cadastrados, editados ou removidos durante a execução serão perdidos quando a API for encerrada ou reiniciada.

Em uma versão futura, o sistema poderá ser integrado a um banco de dados, como:

* SQL Server;
* PostgreSQL;
* MySQL;
* SQLite.

---

## Possíveis Melhorias Futuras

* Integração com banco de dados;
* Histórico de movimentações;
* Registro detalhado de entradas e saídas;
* Cadastro de múltiplos lotes por material;
* Tela específica para editar lotes;
* Controle de usuários;
* Login e autenticação;
* Alertas para estoque baixo;
* Alertas para vencimento próximo;
* Relatórios exportáveis;
* Dashboard com gráficos;
* Integração com Power BI;
* Melhorias de acessibilidade;
* Controle de permissões por usuário.

---

## Justificativa Técnica

O **BEM WMS** foi desenvolvido utilizando **ASP.NET Core Minimal API**, pois essa abordagem permite criar endpoints de forma simples, rápida e objetiva.

A separação das rotas em arquivos diferentes facilita a organização, a leitura e a manutenção do código. Cada arquivo possui uma responsabilidade específica dentro do projeto.

O uso de listas em memória permite demonstrar o funcionamento da API sem a necessidade de configurar um banco de dados, tornando o projeto mais simples para fins acadêmicos e de aprendizagem.

O Swagger foi utilizado para documentar e testar os endpoints da API diretamente pelo navegador.

O front-end foi desenvolvido com HTML, CSS e JavaScript puro, consumindo os endpoints da API por meio de requisições `fetch`.

A inclusão do dashboard financeiro permite analisar o estoque não apenas pela quantidade de materiais, mas também pelo valor investido, venda estimada, lucro estimado e ROI.

A saída de estoque por lote aproxima o sistema de um cenário real de WMS, pois considera a organização dos materiais por lote e a priorização de itens com vencimento mais próximo.

---
