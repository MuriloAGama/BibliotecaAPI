using BibliotecaAPI.Data;
using BibliotecaAPI.Services;
using BibliotecaAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

// ----------------------------------------------------
// 1. Injeção de Dependências e Configuração de Serviços
// ----------------------------------------------------

builder.Services.AddScoped<LivroService>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("LiberarGeral", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configuração do Banco (Lendo a string de conexão)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    // Se estiver no Render e não tiver string de conexão válida externa, evita quebrar usando o banco em memória para testes
    if (string.IsNullOrEmpty(connectionString) || connectionString.Contains("localhost") || connectionString.Contains("127.0.0.1"))
    {
        // Se quiser testar na nuvem sem banco configurado ainda, descomente a linha abaixo e instale o pacote: Microsoft.EntityFrameworkCore.InMemory
        // options.UseInMemoryDatabase("BibliotecaDev");
        
        options.UseSqlServer(connectionString); // Mantém o SQL Server, mas fique atento aos logs!
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
    options.RoutePrefix = "swagger"; // Acessível em /swagger
});

app.UseAuthorization();
app.MapControllers();

// Inicialização automática do banco de dados
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        Console.WriteLine("🔄 Tentando aplicar/verificar o banco de dados...");
        await context.Database.EnsureCreatedAsync();
        Console.WriteLine("✅ Banco de dados verificado com sucesso!");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "🚨 Falha crítica na criação automática do banco de dados. Verifique a Connection String!");
    }
}

app.Run();