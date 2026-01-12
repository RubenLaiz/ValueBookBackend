using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers.Valoraciones;

[Route("Api/Comentario")]
[ApiController]
public class AddComentariosController : ControllerBase
{

    private readonly ComentarioService _comentarioService;
    public AddComentariosController(ComentarioService comentarioService)
    {
        _comentarioService = comentarioService;
    }

    [HttpPost("Agregar")]
    public async Task<IActionResult> CrearComentario([FromBody] Comentario nuevoComentario)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var comentario = await _comentarioService.AgregarComentario(nuevoComentario);

            if (comentario == true)
            {
                return Ok(true);
            }

            return BadRequest(false);
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
