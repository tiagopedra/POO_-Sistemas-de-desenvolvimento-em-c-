using ApiContratos.Routes;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Habilita CORS para o frontend funcionar
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

// Serve arquivos estáticos da pasta wwwroot (index.html, style.css, script.js)
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGetRoutes();
app.MapPostRoutes();
app.MapPutRoutes();
app.MapPatchRoutes();
app.MapDeleteRoutes();

app.Run();
