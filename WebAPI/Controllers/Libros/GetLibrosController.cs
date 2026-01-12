using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Services;

namespace WebAPI.Controllers.Libros;

[Route("Api/Libro")]
[ApiController]
public class GetLibrosController : ControllerBase
{

    private LibroService _libroService;
    
    public GetLibrosController(LibroService libroService) 
    {
        _libroService = libroService;
    }

    [HttpGet("ObtenerId")]
    public async Task<IActionResult> ObtenerLibroId([FromQuery] int libroId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var libro = await _libroService.ObtenerLibroId(libroId);

            if (libro != null)
            {
                return Ok(libro);
            }

            return BadRequest(new
            {
                Codigo = 404,
                Mensaje = "no existe el libro"
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

    [HttpGet("Obtener")]
    public async Task<IActionResult> ObtenerLibro([FromQuery] string? titulo, [FromQuery] string? autor, [FromQuery] string? anio)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var libro = await _libroService.ObtenerLibros(titulo, autor, anio);

            if (libro != null)
            {
                return Ok(libro);
            }

            return BadRequest(new
            {
                Codigo = 404,
                Mensaje = "no existe el libro"
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
