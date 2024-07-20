namespace BlazorCrud.Shared;

public class BookDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = null!;
    public int AutorId { get; set; }
    public AutorDto Autor { get; set; } = null!;
}