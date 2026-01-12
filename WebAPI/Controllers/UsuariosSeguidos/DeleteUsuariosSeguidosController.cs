using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Services;

namespace WebAPI.Controllers.UsuariosSeguidos;

[Route("Api/UsuarioSeguido")]
[ApiController]
public class DeleteUsuariosSeguidosController : ControllerBase
{

    private readonly UsuarioSeguidoService _usuarioSeguidoService;
    public DeleteUsuariosSeguidosController(UsuarioSeguidoService usuarioSeguidoService)
    {
        _usuarioSeguidoService = usuarioSeguidoService;
    }

    [HttpGet("Borrar")]
    public async Task<IActionResult> EliminarUsuariosSeguidos([FromQuery] Guid usuarioId, string usuarioSeguido)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var usuarioSeguidoEliminado = await _usuarioSeguidoService.EliminarUsuarioSeguido(usuarioId, usuarioSeguido);

            if (usuarioSeguidoEliminado == true)
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

