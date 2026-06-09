using ApiProdutos.Routes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("PermitirFrontend");

app.MapGetRoutes();
app.MapGetAtivosRoutes();
app.MapGetInativosRoutes();
app.MapPostRoutes();
app.MapPutRoutes();
app.MapDeleteRoutes();
app.MapSaidaRoutes();
app.MapDashboardRoutes();

app.Run();