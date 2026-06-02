using BibliotecaAPI.Models;

namespace BibliotecaAPI.Interfaces
{
    public interface ILivroService
    {
        /// <summary>
        /// Obtém uma lista com todos os livros cadastrados.
        /// </summary>
        Task<List<Livro>> ObterTodosAsync();

        /// <summary>
        /// Busca um livro específico pelo seu ID.
        /// </summary>
        /// <param name="id">ID do livro.</param>
        Task<Livro?> ObterPorIdAsync(int id);

        /// <summary>
        /// Cadastra um novo livro na biblioteca.
        /// </summary>
        /// <param name="livro">Objeto contendo os dados do livro.</param>
        Task CadastrarLivroAsync(Livro livro);
    }
}