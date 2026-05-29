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
        // Arranjo (Arrange)
        var livrosFake = new List<Livro>
        {
            new Livro { Id = 1, Titulo = "Código Limpo", Autor = "Robert C. Martin", EmailAutor = "autor@teste.com" },
            new Livro { Id = 2, Titulo = "Arquitetura Limpa", Autor = "Robert C. Martin", EmailAutor = "autor@teste.com" }
        };

        // AJUSTE REAL: Agora o Moq simula o método correto do seu Repository!
        _repositoryMock.Setup(repo => repo.ObterTodosAsync()).ReturnsAsync(livrosFake);

        // Ação (Act)
        var resultado = await _service.ListarLivrosAsync();

        // Asserção (Assert)
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        Assert.Equal("Código Limpo", resultado[0].Titulo);
    }
}
