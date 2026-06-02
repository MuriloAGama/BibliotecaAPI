using Microsoft.AspNetCore.Mvc;
using BibliotecaAPI.Models;
using BibliotecaAPI.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class LivroController : ControllerBase
{
    private readonly ILivroService _livroService;

    // Agora o Controller só conhece a Interface, respeitando o princípio de Inversão de Dependência
    public LivroController(ILivroService livroService)
    {
        _livroService = livroService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Livro>>> Get() => Ok(await _livroService.ObterTodosAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Livro>> GetById(int id)
    {
        var livro = await _livroService.ObterPorIdAsync(id);
        return livro == null ? NotFound() : Ok(livro);
    }

    [HttpPost]
    public async Task<ActionResult<Livro>> Post([FromBody] Livro novoLivro)
    {
        try {
            await _livroService.CadastrarLivroAsync(novoLivro);
            return CreatedAtAction(nameof(GetById), new { id = novoLivro.Id }, novoLivro);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }
}