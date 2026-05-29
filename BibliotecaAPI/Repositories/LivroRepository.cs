using System.Collections.Generic;
using System.Threading.Tasks;
using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Repositories;

public class LivroRepository
{
    private readonly AppDbContext _context;

    public LivroRepository(AppDbContext context)
    {
        _context = context;
    }

    public virtual async Task<List<Livro>> ObterTodosAsync() 
    {
        return await _context.Livros.ToListAsync();
    }
    
    public virtual async Task<Livro?> ObterPorIdAsync(int id) 
    {
        return await _context.Livros.FirstOrDefaultAsync(l => l.Id == id);
    }
    
    public virtual async Task CriarAsync(Livro novoLivro)
    {
        await _context.Livros.AddAsync(novoLivro);
        await _context.SaveChangesAsync();
    }
}