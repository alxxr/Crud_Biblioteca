using BlazorCrud.Shared;
using System.Net.Http.Json;
using BlazorCrud.Client.DataAccess.Interface;

namespace BlazorCrud.Client.DataAccess.Service;

public class AutorService : IAutorService
{
    private readonly HttpClient _http;

    public AutorService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<AutorDto>> Lista_Autores()
    {
        var result = await _http.GetFromJsonAsync<ResponseApi<List<AutorDto>>>("api/Autor/lista_autores");

        if (result!.EsCorrecto)
        {
            return result.Valor;
        }
        else
        {
            throw new Exception(result.Mensaje);
        }
    }
    
    public async Task<AutorDto> Buscar(int id)
    {
        var result = await _http.GetFromJsonAsync<ResponseApi<AutorDto>>($"api/Autor/search/{id}");

        if (result!.EsCorrecto)
        {
            return result.Valor;
        }
        else
        {
            throw new Exception(result.Mensaje);
        }
    }
    
    public async Task<int> Guardar(AutorDto autor)
    {
        try
        {
            var result = await _http.PostAsJsonAsync("api/Autor/guardar", autor);
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

    
    public async Task<int> Editar(AutorDto autor)
    {
        var id = autor.Id; 
        var result = await _http.PostAsJsonAsync($"api/Autor/editar", autor);
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
    
    public async Task<bool> Eliminar(int id)
    {
        var result = await _http.DeleteAsync($"api/Autor/eliminar/{id}");
        var response = await result.Content.ReadFromJsonAsync<ResponseApi<int>>();

        if (response!.EsCorrecto)
        {
            return response.EsCorrecto;
        }
        else
        {
            throw new Exception(response.Mensaje);
        }
    }
}