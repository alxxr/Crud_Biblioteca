using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using DB;
using BlazorCrud.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorCrud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly PostgresContext _dbContext;

        public AutorController(PostgresContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("lista_autores")]
        public async Task<IActionResult> AutoresList()
        {
            var responseApi = new ResponseApi<List<AutorDto>>();
            List<AutorDto>? autoresListDto;

            try
            {
                autoresListDto = await _dbContext.Autors
                    .Select(item => new AutorDto
                    {
                        Id = item.Id,
                        Nombre = item.Nombre,
                        Books = item.Books.Select(book => new BookDto
                        {
                            Id = book.Id,
                            Titulo = book.Titulo
                        }).ToList()
                        
                    })
                    .ToListAsync();

                responseApi.EsCorrecto = true;
                responseApi.Valor = autoresListDto;
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);

        }
        
        [HttpGet]
        [Route("search/{id}")]
        public async Task<IActionResult> Buscar(int id)
        {
            var responseApi = new ResponseApi<AutorDto>();

            try
            {
                var autorDto = await _dbContext.Autors
                    .Where(item => item.Id == id) 
                    .Select(item => new AutorDto
                    {
                        Id = item.Id,
                        Nombre = item.Nombre,
                        Books = item.Books.Select(book => new BookDto
                        {
                            Id = book.Id,
                            Titulo = book.Titulo
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (autorDto == null)
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Autor no encontrado";
                }
                else
                {
                    responseApi.EsCorrecto = true;
                    responseApi.Valor = autorDto;
                }
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);

        }
        
        [HttpPost]
        [Route("guardar")]
        public async Task<IActionResult> Guardar(AutorDto autor)
        {
            var responseApi = new ResponseApi<int>();

            try
            {
                var dbAutor = new Autor
                {
                    Nombre = autor.Nombre,
                };

                _dbContext.Autors.Add(dbAutor);
                await _dbContext.SaveChangesAsync();

                if (dbAutor.Id != 0)
                {
                    responseApi.EsCorrecto = true;
                    responseApi.Valor = dbAutor.Id;
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "No se pudo guardar el Autor";
                }


            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);

        }
        
        [HttpPost]
        [Route("editar")]
        public async Task<IActionResult> Editar(AutorDto autor)
        {
            var responseApi = new ResponseApi<int>();

            try
            {
                var dbAutor = await _dbContext.Autors
                    .Where(a => a.Id == autor.Id)
                    .FirstOrDefaultAsync();

                if (dbAutor != null)
                {
                    dbAutor.Nombre = autor.Nombre;
                    
                    _dbContext.Autors.Update(dbAutor);
                    await _dbContext.SaveChangesAsync();
                    
                    responseApi.EsCorrecto = true;
                    responseApi.Valor = dbAutor.Id;
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "No se pudo actualizar el Autor";
                }
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);
        }
        
        [HttpDelete]
        [Route("eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var responseApi = new ResponseApi<int>();

            try
            {
                var dbAutor = await _dbContext.Autors
                    .Where(a => a.Id == id)
                    .FirstOrDefaultAsync();

                if (dbAutor != null)
                {
                    _dbContext.Autors.Remove(dbAutor);
                    await _dbContext.SaveChangesAsync();
                    
                    responseApi.EsCorrecto = true;
                    responseApi.Mensaje = "Autor eliminado";
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "No se pudo actualizar el Autor";
                }
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);
        }
        
    }
}
