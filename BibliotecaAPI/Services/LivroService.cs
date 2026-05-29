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
        // Usa o nome real do seu Repository!
        return await _repository.ObterTodosAsync();
    }

    public async Task<Livro> CadastrarLivroAsync(Livro livro)
    {
        // Mapeia para o CriarAsync que o seu Repository tem
        await _repository.CriarAsync(livro);
        return livro;
    }
}
