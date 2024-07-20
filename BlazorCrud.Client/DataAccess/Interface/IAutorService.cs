using BlazorCrud.Shared;

namespace BlazorCrud.Client.DataAccess.Interface;

public interface IAutorService
{
    Task<List<AutorDto>> Lista_Autores();
    Task<AutorDto> Buscar(int id);
    Task<int> Guardar(AutorDto autor);
    Task<int> Editar(AutorDto autor);
    Task<bool> Eliminar(int id);
}