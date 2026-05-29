namespace BibliotecaAPI.Models;

public class Livro
{
    public int Id { get; set; }
    public required string Titulo { get; set; }
    public required string Autor { get; set; }
    public string EmailAutor { get; set; } = string.Empty;
    public string? ISBN { get; set; }
    public int AnoPublicacao { get; set; }
    public bool Disponivel { get; set; } = true; 
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
}