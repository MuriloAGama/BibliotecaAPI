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
        // O CRIME: Esse Assert vai estourar na hora porque a lista NÃO é nula!
        var listaQueVaiQuebrar = new List<Livro>();
        Assert.Null(listaQueVaiQuebrar);

        var livrosFake = new List<Livro>
        {
            new Livro { Id = 1, Titulo = "Código Limpo", Autor = "Robert C. Martin", EmailAutor = "autor@teste.com" }
        };
    }
}
