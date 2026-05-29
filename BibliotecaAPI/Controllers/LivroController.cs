using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LivroController : ControllerBase
{
    private readonly LivroRepository _repository;
    private readonly LivroService _livroService; // 1. Declarado o Service aqui

    // 2. Injetando o LivroRepository e o LivroService juntos no construtor
    public LivroController(LivroRepository repository, LivroService livroService)
    {
        _repository = repository;
        _livroService = livroService;
    }

    /// <summary>
    /// Gera uma lista de livros.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Livro>>> Get()
    {
        var livros = await _repository.ObterTodosAsync();
        return Ok(livros);
    }

    /// <summary>
    /// Busca um livro pelo seu ID.
    /// </summary>
    /// <param name="id">ID do livro a ser buscado.</param>
    /// <returns>O livro encontrado com seu ID.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Livro>> GetById(int id)
    {
        var livro = await _repository.ObterPorIdAsync(id);
        
        if (livro == null)
        {
            return NotFound(new { mensagem = $"Livro com ID {id} não foi encontrado." });
        }

        return Ok(livro);
    }

    /// <summary>
    /// Cria um novo livro na biblioteca.
    /// </summary>
    /// <param name="novoLivro">Dados para criação do livro.</param>
    /// <returns>O livro recém-criado com seu ID gerado.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Livro>> Post([FromBody] Livro novoLivro)
    {
        if (novoLivro == null)
        {
            return BadRequest("Os dados do livro não podem ser nulos.");
        }

        try
        {
            // Agora sim o Service está injetado e pronto para rodar as validações!
            await _livroService.CadastrarLivroAsync(novoLivro);
            
            return CreatedAtAction(nameof(GetById), new { id = novoLivro.Id }, novoLivro);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocorreu um erro ao criar o livro: {ex.Message}");
        }
    }
}