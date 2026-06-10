// ============================================================
// Program.cs — Ponto de entrada da aplicação
// 
// Responsabilidade:
//   - Configurar e inicializar o servidor web ASP.NET Core
//   - Registrar todos os grupos de rotas da API
//   - Servir o front-end estático (HTML/CSS/JS)
//
// Fluxo de inicialização:
//   1. Cria o WebApplication builder
//   2. Define o ambiente como "Production"
//   3. Configura a pasta wwwroot como raiz dos arquivos estáticos
//   4. Registra middlewares de arquivos estáticos
//   5. Mapeia todas as rotas da API
//   6. Inicia o servidor
// ============================================================

using ApiBarbearia.Routes;

var builder = WebApplication.CreateBuilder(args);

// Define o ambiente de execução
builder.Environment.EnvironmentName = "Production";

// Define a pasta que contém o front-end (index.html, style.css, app.js)
builder.WebHost.UseWebRoot("wwwroot");

var app = builder.Build();

// -------------------------------------------------------
// Middlewares de arquivos estáticos
// Deve vir ANTES das rotas para que o index.html seja
// servido corretamente ao acessar "/"
// -------------------------------------------------------
app.UseDefaultFiles();  // Serve index.html como padrão
app.UseStaticFiles();   // Permite acesso a CSS, JS e outros arquivos

// -------------------------------------------------------
// Rotas de Agendamentos
//   GET    /api/agendamentos
//   GET    /api/agendamentos/{id}
//   POST   /api/agendamentos
//   PUT    /api/agendamentos/{id}
//   DELETE /api/agendamentos/{id}
// -------------------------------------------------------
app.MapGetAgendamentosRoutes();
app.MapPostAgendamentosRoutes();
app.MapDeleteAgendamentosRoutes();

// -------------------------------------------------------
// Rotas de Produtos (Serviços da Barbearia)
//   GET    /api/produtos
//   GET    /api/produtos/{id}
//   POST   /api/produtos
//   PUT    /api/produtos/{id}
//   DELETE /api/produtos/{id}
// -------------------------------------------------------
app.MapGetProdutosRoutes();
app.MapPostProdutosRoutes();
app.MapDeleteProdutosRoutes();

// -------------------------------------------------------
// Rotas de Despesas
//   GET    /api/despesas
//   GET    /api/despesas/{id}
//   POST   /api/despesas
//   PUT    /api/despesas/{id}
//   DELETE /api/despesas/{id}
// -------------------------------------------------------
app.MapGetDespesas();
app.MapPostDespesas();
app.MapDeleteDespesas();

// -------------------------------------------------------
// Rota do Resumo Financeiro
//   GET    /api/financeiro
// -------------------------------------------------------
app.MapFinanceiroRoutes();

// Inicia o servidor HTTP na porta configurada no launchSettings.json
app.Run();
