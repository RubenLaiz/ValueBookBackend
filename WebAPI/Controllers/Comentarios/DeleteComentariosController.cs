using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Services;

namespace WebAPI.Controllers.Valoraciones;

[Route("Api/Comentario")]
[ApiController]
public class DeleteComentariosController : ControllerBase
{

    private readonly ComentarioService _comentarioService;
    public DeleteComentariosController(ComentarioService comentarioService)
    {
        _comentarioService = comentarioService;
    }

    [HttpDelete("Eliminar")]
    public async Task<IActionResult> EliminarComentario([FromQuery] int libroId, string nombreUsuario)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var comentario = await _comentarioService.EliminarComentario(libroId, nombreUsuario);

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
