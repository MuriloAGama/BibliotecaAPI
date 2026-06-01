using BibliotecaAPI.Models; // Certifique-se de que o namespace do modelo Livro está correto aqui
using BibliotecaAPI.Repositories;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivroController : ControllerBase
    {
        private readonly LivroRepository _repository;
        private readonly LivroService _livroService;

        // Construtor injetando o Repository e o Service juntos
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
                // Executa as regras de negócio antes de salvar
                await _livroService.CadastrarLivroAsync(novoLivro);

                // Retorna o Status 201 chamando o método GetById
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
}