using ApiVendasHardware.Repositories;
using ApiVendasHardware.Routes;
using ApiVendasHardware.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Configurações de CORS ──────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// ── Serviços (Dependency Injection) ────────────────────────────────────────
// Repositório singleton: estado compartilhado em memória
builder.Services.AddSingleton<HardwareRepository>();

// Serviço de pedidos: validações e regras de negócio
builder.Services.AddSingleton<PedidoService>();

// ── Construir a aplicação ──────────────────────────────────────────────────
var app = builder.Build();

// ── Middleware ─────────────────────────────────────────────────────────────
app.UseCors("Frontend");
app.UseStaticFiles(); // Servir arquivos da pasta wwwroot (index.html, CSS, JS, etc)

// ── Rotas ──────────────────────────────────────────────────────────────────
app.MapGet("/", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.SendFileAsync(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"));
});

app.MapGetRoutes();
app.MapPostRoutes();
app.MapPutRoutes();
app.MapPatchRoutes();
app.MapDeleteRoutes();

// ── Executar ───────────────────────────────────────────────────────────────
app.Run();