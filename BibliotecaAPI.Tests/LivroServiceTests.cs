using System.Collections.Generic;
using System.Threading.Tasks;
using System;
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
        _repositoryMock = new Mock<LivroRepository>(null!); 
        _service = new LivroService(_repositoryMock.Object);
    }

    [Fact]
    public async Task ListarLivrosAsync_DeveRetornarListaDeLivros_QuandoExistiremLivros()
    {
        var livrosFake = new List<Livro>
        {
            new Livro { Id = 1, Titulo = "Código Limpo", Autor = "Robert C. Martin", EmailAutor = "autor@teste.com" },
            new Livro { Id = 2, Titulo = "Arquitetura Limpa", Autor = "Robert C. Martin", EmailAutor = "autor@teste.com" }
        };

        _repositoryMock.Setup(repo => repo.ObterTodosAsync())
                       .ReturnsAsync(livrosFake);

        var resultado = await _service.ListarLivrosAsync();

        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        Assert.Equal("Código Limpo", resultado[0].Titulo);
    }

    [Fact]
    public async Task BuscarPorIdAsync_DeveLancarArgumentException_QuandoLivroNaoExistir()
    {
        int idInexistente = 999;
        
        _repositoryMock.Setup(repo => repo.ObterPorIdAsync(idInexistente))
                       .ReturnsAsync((Livro)null!);

        // Ajustado para ThrowsAsync<ArgumentException> refletindo o Service corrigido!
        var excecao = await Assert.ThrowsAsync<ArgumentException>(() => 
            _service.BuscarPorIdAsync(idInexistente)
        );

        Assert.Equal("Livro não encontrado no banco de dados.", excecao.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task CadastrarLivroAsync_DeveLancarArgumentException_QuandoTituloForInvalido(string? tituloInvalido)
    {
        var libroInvalido = new Livro 
        { 
            Id = 3, 
            Titulo = tituloInvalido!, 
            Autor = "Autor Teste",
            EmailAutor = "autor@teste.com"
        };

        await Assert.ThrowsAsync<ArgumentException>(() => 
            _service.CadastrarLivroAsync(libroInvalido)
        );
    }

    [Theory]
    [InlineData("autorsemarroba.com")] // Sem arroba
    [InlineData("autor@com")]           // Sem o ponto antes do com
    [InlineData("   ")]                 // Apenas espaços
    [InlineData("")]                    // Vazio
    public async Task CadastrarLivroAsync_DeveLancarArgumentException_QuandoEmailAutorForInvalido(string emailInvalido)
    {
        var livroComEmailRuim = new Livro 
        { 
            Id = 4, 
            Titulo = "Design Patterns", 
            Autor = "GoF",
            EmailAutor = emailInvalido 
        };

        var excecao = await Assert.ThrowsAsync<ArgumentException>(() => 
            _service.CadastrarLivroAsync(livroComEmailRuim)
        );

        Assert.Equal("O e-mail do autor informado é inválido.", excecao.Message);
    }
}
// Teste criminoso para quebrar o pipeline
public class TesteSabotagem
{
    [Xunit.Fact]
    public void Teste_Deve_Falhar_De_Proposito()
    {
        Xunit.Assert.Equal("Sucesso", "Erro Total");
    }
}
