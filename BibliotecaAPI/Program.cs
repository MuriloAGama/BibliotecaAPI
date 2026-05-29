using BibliotecaAPI.Data;
using BibliotecaAPI.Services;
using BibliotecaAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------
// 1. Injeção de Dependências e Configuração de Serviços
// ----------------------------------------------------

builder.Services.AddScoped<LivroService>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Configuração da política de CORS para permitir consumo do Swagger/Frontend externamente
builder.Services.AddCors(options =>
{
    options.AddPolicy("LiberarGeral", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<LivroRepository>();

var app = builder.Build();

// ----------------------------------------------------
// 2. Middleware Pipeline (A ordem de execução importa)
// ----------------------------------------------------

app.UseCors("LiberarGeral"); // Deve vir antes dos endpoints para evitar bloqueio no navegador

app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Biblioteca API v1");
    options.RoutePrefix = "swagger"; 
});

app.UseAuthorization();
app.MapControllers();

// Inicialização automática do banco de dados (crucial para o ambiente Docker)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        await context.Database.EnsureCreatedAsync(); // Cria o banco e as tabelas se não existirem
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Falha na criação automática do banco de dados.");
    }
}

app.Run();