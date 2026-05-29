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

    /// <summary>
    /// Gera uma lista de livros.
    /// </summary>
    /// <exception cref="ArgumentException">Lançada quando a lista de livros estiver vazia.</exception>
    public async Task<List<Livro>> ListarLivrosAsync()
    {
        var livros = await _repository.ObterTodosAsync();
        if (livros == null || livros.Count == 0)
        {
            throw new ArgumentException("Nenhum livro encontrado.");
        }
        return livros;
    }

    /// <summary>
    /// Busca um livro pelo seu ID.
    /// </summary>
    /// <param name="id">ID do livro a ser buscado.</param>
    /// <exception cref="ArgumentException">Lançada quando o livro não for encontrado.</exception>
    public async Task<Livro> BuscarPorIdAsync(int id)
    {
        var livro = await _repository.ObterPorIdAsync(id);
        if (livro == null)
        {
            // Corrigido para bater com o XML Comment
            throw new ArgumentException("Livro não encontrado no banco de dados."); 
        }
        return livro;
    }

    /// <summary>
    /// Cadastra um novo livro no sistema após aplicar as validações de regra de negócio.
    /// </summary>
    /// <param name="novoLivro">Objeto contendo os dados do livro a ser criado.</param>
    /// <exception cref="ArgumentException">Lançada quando o título está vazio ou o e-mail do autor é inválido.</exception>
    public async Task CadastrarLivroAsync(Livro novoLivro)
    {
        if (string.IsNullOrWhiteSpace(novoLivro.Titulo))
        {
            throw new ArgumentException("O título do livro é obrigatório.");
        }

        if (!novoLivro.EmailAutor.Contains("@") || !novoLivro.EmailAutor.Contains(".com"))
        {
            throw new ArgumentException("O e-mail do autor informado é inválido.");
        }

        await _repository.CriarAsync(novoLivro);
    }
}