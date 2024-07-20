using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using DB;
using BlazorCrud.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorCrud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly PostgresContext _dbContext;

        public BookController(PostgresContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet]
        [Route("lista_books")]
        public async Task<IActionResult> BooksList()
        {
            var responseApi = new ResponseApi<List<BookDto>>();
            var booksListDto = new List<BookDto>();

            try
            {
                booksListDto = await _dbContext.Books
                    .Include(b => b.Autor)
                    .Select(item => new BookDto
                    {
                        Id = item.Id,
                        Titulo = item.Titulo,
                        AutorId = item.AutorId,
                        Autor = new AutorDto
                        {
                            Id = item.Autor.Id,
                            Nombre = item.Autor.Nombre
                        }
                    })
                    .ToListAsync();

                responseApi.EsCorrecto = true;
                responseApi.Valor = booksListDto;
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }

            return Ok(responseApi);

        }
        
        [HttpGet]
        [Route("search/{idAutor}")]
        public async Task<IActionResult> BuscarXAutorId(int idAutor)
        {
            var responseApi = new ResponseApi<List<BookDto>>();
            var booksListDto = new List<BookDto>();

            try
            { 
                booksListDto = await _dbContext.Books
                    .Include(b => b.Autor)
                    .Where(item => item.AutorId == idAutor) 
                    .Select(item => new BookDto
                    {
                        Id = item.Id,
                        Titulo = item.Titulo,
                        Autor = new AutorDto
                        {
                            Id = item.Autor.Id,
                            Nombre = item.Autor.Nombre
                        }
                    })
                    .ToListAsync();
                
                responseApi.EsCorrecto = true;
                responseApi.Valor = booksListDto;
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
        public async Task<IActionResult> GuardarBook(BookDto book)
        {
            var responseApi = new ResponseApi<int>();
        
            try
            {
                var dbBook = new Book
                {
                    Titulo = book.Titulo,
                    AutorId = book.AutorId
                };
        
                _dbContext.Books.Add(dbBook);
                await _dbContext.SaveChangesAsync();
        
                if (dbBook.Id != 0)
                {
                    responseApi.EsCorrecto = true;
                    responseApi.Valor = dbBook.Id;
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "No se pudo guardar el Book";
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
