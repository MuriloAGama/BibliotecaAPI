using Microsoft.AspNetCore.Mvc;
using BibliotecaAPI.Models;
using BibliotecaAPI.Interfaces;
using BibliotecaAPI.Repositories;

namespace BibliotecaAPI.Services
{
    public class LivroService : ILivroService
    {
        private readonly LivroRepository _repository;
        
        public LivroService(LivroRepository repository) => _repository = repository;

        public async Task<List<Livro>> ObterTodosAsync() => await _repository.ObterTodosAsync();
        
        public async Task<Livro?> ObterPorIdAsync(int id) => await _repository.ObterPorIdAsync(id);
    
       public async Task CadastrarLivroAsync(Livro livro) 
        {
        await _repository.CadastrarLivroAsync(livro); 
        }
    }
}