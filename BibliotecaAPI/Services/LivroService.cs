using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories;

namespace BibliotecaAPI.Services;

public class LivroService
{
    private readonly LivroRepository _repository;

    public LivroService(LivroRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Livro>> ListarLivrosAsync()
    {
        // O CRIME REAL: Em vez de buscar os livros, vamos retornar NULL de propósito!
        // Isso vai fazer o teste "DeveRetornarListaDeLivros" estourar um NullReferenceException na hora!
        return null!;
    }
}
