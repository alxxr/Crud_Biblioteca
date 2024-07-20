using BlazorCrud.Shared;
using System.Net.Http.Json;
using BlazorCrud.Client.DataAccess.Interface;

namespace BlazorCrud.Client.DataAccess.Service;

public class BookService : IBookService
{
    private readonly HttpClient _http;

    public BookService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<BookDto>> Lista_Books()
    {
        var result = await _http.GetFromJsonAsync<ResponseApi<List<BookDto>>>("api/Book/lista_books");

        if (result!.EsCorrecto)
        {
            return result.Valor;
        }
        else
        {
            throw new Exception(result.Mensaje);
        }
    }
    
    public async Task<List<BookDto>> BuscarXAutorId(int idAutor)
    {
        var result = await _http.GetFromJsonAsync<ResponseApi<List<BookDto>>>($"api/Book/search/{idAutor}");

        if (result!.EsCorrecto)
        {
            return result.Valor;
        }
        else
        {
            throw new Exception(result.Mensaje);
        }
    }
    
    public async Task<int> GuardarBook(BookDto book)
    {
        try
        {
            var result = await _http.PostAsJsonAsync("api/Book/guardar", book);
            var response = await result.Content.ReadFromJsonAsync<ResponseApi<int>>();

            if (response!.EsCorrecto)
            {
                return response.Valor;
            }
            else
            {
                throw new Exception(response.Mensaje);
            }
        }
        catch (Exception ex)
        {
            // Agrega un registro para depuración
            Console.WriteLine($"Error en Guardar: {ex.Message}");
            throw;
        }
    }
}