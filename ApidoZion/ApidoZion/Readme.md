**ZION: SISTEMA DE GESTÃO DE VENDAS.**
- Descrição: sistema de gestão de dados de vendas que permite o cadastro de clientes, controle de peças em destoque e seus valores, e o volume de peças vendidos. O objetivo é análise dos dados das vendas, através dos relatórios apresentados em Dashboard, para que a   empresa "BullDogs_PC" possa mensurar seus lucros, avaliar as peças mais vendidas - realizando investimentos, e conceder benefícios aos cliente de alto valor no negócio. Obs: a aplicação utiliza banco de memórias, ou seja, os dados ficam na memoria RAM - Volátil, e serve como teste da ferramenta.

**Contexto**: uma empresa de PC Gamer, responsável pela venda de peças para computadores gamers necessita de uma plataforma única para cadastrar seus clientes, fornecedores, dados de objetos vendidos e posteriormente, mensurar as métricas das vendas em relatórios. Entretanto, a empresa não disponibilizava de uma plataforma sistêmica que agrupasse todos esses dados, portanto, não avaliava seu crescimento por meio de diferentes caminhos, isso centralizava as ações da empresa sobre o cliente, e o deixava sem opções variadas.

**Gárgalo a resolver**: avaliar como a empresa mensura seu lucro mensal em relação ao cliente e ao produto vendido, e propor uma solução que unifique os 3, gerando à empresa novas ideias, novas abordagens, e investimentos em áreas específicas (ex: um desconto maior a quem compra uma quantidade X de peças.). 

**Tecnologias utilizadas**: .Net ASP Core 9.0. 
**Linguagens Utilizadas**: C# para aplicação do código-fonte (execução da aplicação), rotas, código base (contendo herança), código dos cálculos utilizados para o DashBoard; HTML (Para estrutura da página), CSS (para estilização, organização e uso de cores) JavaScript (para a interação entre as guias, botões, funcionamento do código integrado a API - BackEnd e FrontEnd).

**Principais Pastas e Guias Utilizadas** 
§ Bin \ Debug: encontra-se as versões .NET ASP Core 9.0 e 10.O;
§ Json: encontra-se os arquivos para armazenar, configurar e trocar informações entre a API e os códigos utilizados. Na guia "global.json" encontra-se o código criado para funcionamento da .NET ASP Core 9.0.
§ Models: encontra-se as guias de
  - CRM.cs: encontram-se as classes referentes ao sistema (Cliente, fornencedor, vendas e peças) foi utilizado uma herança para as classes cliente e fornecedor, tendo como base a classe pai "Dados Gerais". 
  - ServicoDeVenda.cs: encontra-se o código referente aos cálculos utilizados para a guia Dashboard, são eles: faturamento, custo total, margem de lucro, ticket médio, volume de peças vendidas, peça mais e menos vendida, quantidade de cliente ativo, fornecedor mais utilizado, lucro por peça, lucro por cliente, faturamento por fornecedor, peça mais lucrativa e margem de venda. 

§ Obj: contém os metadados, arquvios temporários do código e configurações referentes.
§ Properties: pasta automática
§ rotas: 
 ROTA_GET.cs: localiza-se o código para pesquisa e cadastro de fornecedores, através do ID ou "Fornecedores Ativo / Inativos / Aguardando Aprovação. 
    Online: O cliente ou fornecedor possui vínculo com a empresa.
    Offline: O Fornecedor encerrou seu vínculo com a empresa.
    Sleepy: tramite e documentação em andamento para o novo cliente unir-se a empresa. 
 ROTA_POST: localiza-se o código que permite o cadastro, através do ID;
 ROTA_DELETE: localiza-se o código que permite a exclusão de fornecedores, com base no ID.
 ROTA_ PATCH: localiza-se o código que permite alteração em apeas uma das informações, neste caso, o valor do produto.
 ROTA_PUT: localiza-se o código que permite a alteração de todos os dados - a rota está coerente na versão backend, entretanto nas funções wwwroot, os parâmetros de alteração para todos os dados, não conversaram com a API. 
§ wwwroot: 
  - index.html: encontra-se o código de estrutura da guia WEB, contendo também a integração sistêmica com a API, css, e JavaScript. 
  - stylezion.css: encontra-se o código de estilização da guia HTML. 
  - zion.js: código de interatividade do Java Script.
 §Program.cs: ponto incial da aplicação, contém as rotas criadas para o funcionamento, rota para api e integração com o Front do sistema. 

 Arquitetura Sistêmica
 |
 |---bin\Debug
    | - Net 9.0
    | - Net 10.0
 |
 |---Json
    | - appsettings.Development.json
    | - appsettings.json
    | - global.json
    | - launchSettings.json 
 |
 |---Models
    | - CRM.cs //código base
    | - ServicoDeVenda.cs // Código-fonte
 |
 |---Obj
    |-- Debug 
    | |-- Net 9.0
    | |-- Net 10.0
    |-- ApidoZion.csproj.nuget.dgspec.json
    |-- ApidoZion.csproj.nuget.g.props
    |-- ApidoZion.csproj.nuget.g.targets
    |-- project.assets.json
    |-- project.nuget.cache    
 |---Properties
 |
 |---rotas
    |-- ROTA_DELETE.cs
    |-- ROTA_GET.cs
    |-- ROTA_PATCH.cs
    |-- ROTA_POST.cs
    |-- ROTA_PUT.cs
 |
 |--- wwwroot 
    |--- index.HTML [Guia da WEB contendo código e API integrados]
    |--- stylezion.css [Código em CSS para estilização]
    |--- zion.jpg.png [Imagem logo da ferramenta]
    |--- zion.js  [Código em JavaScript]  
 |
 |--- ApidoZion.csproj
 |--- ApidoZion.sln
 |--- Program.cs
 |--- Readme.md 



- Como testar: 
 1° Abra o terminal "Toggle Panel" na barra superior ao lado de "Toggle Primary Side Bar", digite "cd ApidoZion" e aperte a tecla enter do seu desktop;
 2° Após o carregamento, digite "dotnet run", aguarde o novo caregamento das informações e procure a opção "http://localhost:5000" cole-a no seu navegador, use preferencialmente, microsoft Edge. 
 3° Após colar, utilize as extensões "index.html. Na aba cadastro, selecione qual classe e informação deseja incluir no sistema (Cliente, Fornecedor, Peças ou Venda); na aba listagem, busque um cliente pelo ID, exclua alguma informação cadastrada, ou altere a opção de valor vinculado ao cliente. Na aba Dashboard, verifique os relatórios disponíveis vinculados as abas de cadastro e listagem. Dúvidas? contate os desenvolvedores
 renancvs@gmail.com e savioduarte56@outlook.com, título: ApidoZion.

 Obrigado!
 
 Referências: 
 / Microsoft Learn — Tutorial: Criar uma Minimal API com ASP.NET Core. Acesso em: mai. 2026.
 / Protocolo HTTP - o que é uma API: Solução de problemas em software e Desenvolvimento de Api Modelo. Acesso em: mai.2026





