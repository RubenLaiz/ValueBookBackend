using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using System.ComponentModel.DataAnnotations;
using WebAPI.Services;

namespace WebAPI.Controllers.Valoraciones;

[Route("Api/Comentario")]
[ApiController]
public class GetComentariosController : ControllerBase
{

    private readonly ComentarioService _comentarioService;
    public GetComentariosController(ComentarioService comentarioService)
    {
        _comentarioService = comentarioService;
    }

    [HttpGet("ObtenerComentariosLibro")]
    public async Task<IActionResult> ObtenerComentarios([FromQuery] int libroId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var comentarios = await _comentarioService.ObtenerComentariosLibro(libroId);

            if (comentarios != null)
            {
                return Ok(comentarios);
            }

            return BadRequest(new
            {
                Codigo = 404,
                Mensaje = "no existen comentarios"
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
