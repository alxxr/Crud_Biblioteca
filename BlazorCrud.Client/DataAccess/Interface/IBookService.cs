using BlazorCrud.Shared;

namespace BlazorCrud.Client.DataAccess.Interface;

public interface IBookService
{
    Task<List<BookDto>> Lista_Books();
    
    Task<List<BookDto>> BuscarXAutorId(int id);
    
    Task<int> GuardarBook(BookDto book);
}