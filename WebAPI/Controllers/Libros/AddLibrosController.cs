using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers.Libros;

[Route("Api/Libro")]
[ApiController]
public class AddLibrosController : ControllerBase
{

    private LibroService _libroService;

    public AddLibrosController(LibroService libroService)
    {
        _libroService = libroService;
    }

    [HttpPost("Agregar")]
    public async Task<IActionResult> ObtenerLibro([FromBody] Libro nuevoLibro)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var libro = await _libroService.AgregarLibro(nuevoLibro);

            if (libro != null)
            {
                return Ok(new
                {
                    Codigo = 200,
                    libro
                });
            }

            return BadRequest(new
            {
                Codigo = 400,
                Mensaje = "no se agrego el libro"
            });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new
            {
                Error = 401,
                Mensaje = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                Error = 400,
                Mensaje = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}
