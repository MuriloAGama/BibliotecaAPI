using System.Collections.Generic;
using System.Threading.Tasks;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories;
using BibliotecaAPI.Services;
using Moq;
using Xunit;

namespace BibliotecaAPI.Tests;

public class LivroServiceTests
{
    private readonly Mock<LivroRepository> _repositoryMock;
    private readonly LivroService _service;

    public LivroServiceTests()
    {
        // Criamos o mock do repositório. Passamos null no construtor porque 
        // o Moq vai interceptar as chamadas antes de precisar do DbContext real.
        _repositoryMock = new Mock<LivroRepository>(null!); 
        _service = new LivroService(_repositoryMock.Object);
    }

    [Fact]
    public async Task ListarLivrosAsync_DeveRetornarListaDeLivros_QuandoExistiremLivros()
    {
        // 1. Arrange (Preparar o cenário com dados falsos)
        var livrosFake = new List<Livro>
        {
            new Livro { Id = 1, Titulo = "Código Limpo", Autor = "Robert C. Martin" },
            new Livro { Id = 2, Titulo = "Arquitetura Limpa", Autor = "Robert C. Martin" }
        };

        // Configura o Mock para retornar a lista fake quando o Service chamar o método
        _repositoryMock.Setup(repo => repo.ObterTodosAsync())
                       .ReturnsAsync(livrosFake);

        // 2. Act (Executar a ação do Service)
        var resultado = await _service.ListarLivrosAsync();

        // 3. Assert (Verificar se tudo deu certo)
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        Assert.Equal("Código Limpo", resultado[0].Titulo);
    }

    [Fact]

    public async Task BuscarPorIdAsync_DeveLancarException_QuandoLivroNaoExistir()
    {
        // 1. Arrange (Cenário: o ID 999 não existe)
        int idInexistente = 999;
        
        // O Mock simula o repositório devolvendo null (banco vazio)
        _repositoryMock.Setup(repo => repo.ObterPorIdAsync(idInexistente))
                    .ReturnsAsync((Livro)null!);

        // 2. Act & Assert (Tudo junto!)
        // O xUnit vai rodar a ação e verificar se ela estoura a Exception esperada
        var excecao = await Assert.ThrowsAsync<System.Exception>(() => 
            _service.BuscarPorIdAsync(idInexistente)
        );

        // 3. Opcional: Garante que a mensagem do erro é exatamente a que você escreveu
        Assert.Equal("Livro não encontrado no banco de dados.", excecao.Message);
    }

    [Theory] // Indica que este teste aceita múltiplos cenários (dados de entrada)
    [InlineData("")]        // Cenário 1: Título vazio
    [InlineData("   ")]     // Cenário 2: Título cheio de espaços
    [InlineData(null)]      // Cenário 3: Título nulo
    public async Task CadastrarLivroAsync_DeveLancarArgumentException_QuandoTituloForInvalido(string? tituloInvalido)
    {
        // 1. Arrange
        // Criamos um livro com o título inválido que o xUnit vai injetar a cada rodada
        var livroInvalido = new Livro 
        { 
            Id = 3, 
            Titulo = tituloInvalido!, 
            Autor = "Autor Teste" 
        };

        // 2. Act & Assert
        // O xUnit vai rodar esse método 3 VEZES. Em cada uma, ele espera que o Service barre e jogue um erro.
        await Assert.ThrowsAsync<System.ArgumentException>(() => 
            _service.CadastrarLivroAsync(livroInvalido)
        );
    }
}