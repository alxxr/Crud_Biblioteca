using System.ComponentModel.DataAnnotations;

namespace BlazorCrud.Shared;

public class AutorDto
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El campo {0} es requerido.")]
    public string Nombre { get; set; } = null!;
    public ICollection<BookDto> Books { get; set; }= new List<BookDto>();
}