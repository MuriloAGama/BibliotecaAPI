using BibliotecaAPI.Data;
using BibliotecaAPI.Services;
using BibliotecaAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔥 CONFIGURAÇÃO DO RENDER: Forçar o .NET a escutar na porta que a nuvem mandar
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

// ----------------------------------------------------
// 1. Injeção de Dependências e Configuração de Serviços
// ----------------------------------------------------

builder.Services.AddScoped<LivroService>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Configuração da política de CORS para liberar o acesso ao Swagger/Frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("LiberarGeral", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 🔥 CONFIGURAÇÃO DO BANCO: SQL Server local vs Banco em Memória na nuvem
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    
    // Se estiver no Render (onde a variável RENDER existe) ou se não achar a Connection String local
    if (Environment.GetEnvironmentVariable("RENDER") != null || string.IsNullOrEmpty(connectionString) || connectionString.Contains("localhost") || connectionString.Contains("127.0.0.1"))
    {
        // Usa o banco em memória para o Render não quebrar tentando acessar o localhost
        options.UseInMemoryDatabase("BibliotecaDev");
    }
    else
    {
        // Mantém o SQL Server para quando você estiver rodando no seu ambiente local
        options.UseSqlServer(connectionString);
    }
});

builder.Services.AddScoped<LivroRepository>();

var app = builder.Build();

// ----------------------------------------------------
// 2. Middleware Pipeline (A ordem de execução importa)
// ----------------------------------------------------

app.UseCors("LiberarGeral");

app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Biblioteca API v1");
    options.RoutePrefix = "swagger"; // Swagger acessível em /swagger
});

app.UseAuthorization();
app.MapControllers();

// Inicialização automática do banco de dados (Popula a estrutura de tabelas)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        Console.WriteLine("🔄 Tentando aplicar/verificar o banco de dados...");
        await context.Database.EnsureCreatedAsync();
        Console.WriteLine("✅ Banco de dados inicializado com sucesso!");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "🚨 Falha crítica na criação automática do banco de dados.");
    }
}

app.Run();