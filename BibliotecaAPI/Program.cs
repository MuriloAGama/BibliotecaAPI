using BibliotecaAPI.Data;
using BibliotecaAPI.Services;
using BibliotecaAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔥 CONFIGURAÇÃO DO RENDER: Forçar o .NET a escutar na porta correta da nuvem
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

// ----------------------------------------------------
// 1. Injeção de Dependências e Configuração de Serviços
// ----------------------------------------------------

builder.Services.AddScoped<LivroService>();
builder.Services.AddControllers();

// 🔥 CONFIGURAÇÃO DO SWAGGER (OpenAPI): Injeta a URL correta com HTTPS para evitar erro de CORS no Execute
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        if (Environment.GetEnvironmentVariable("RENDER") != null)
        {
            // Limpa os servidores padrão locais e injeta o host seguro do Render usando inferência de tipo
            document.Servers.Clear();
            document.Servers.Add(new() { Url = "https://bibliotecapi-v5q7.onrender.com" });
        }
        return Task.CompletedTask;
    });
});

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
    
    // Se estiver no Render ou se não encontrar a Connection String local válida
    if (Environment.GetEnvironmentVariable("RENDER") != null || string.IsNullOrEmpty(connectionString) || connectionString.Contains("localhost") || connectionString.Contains("127.0.0.1"))
    {
        options.UseInMemoryDatabase("BibliotecaDev");
    }
    else
    {
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