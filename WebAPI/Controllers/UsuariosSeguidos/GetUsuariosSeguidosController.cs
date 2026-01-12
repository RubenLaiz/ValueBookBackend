using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Services;

namespace WebAPI.Controllers.UsuariosSeguidos;

[Route("Api/UsuarioSeguido")]
[ApiController]
public class GetUsuariosSeguidosController : ControllerBase
{

    private readonly UsuarioSeguidoService _usuarioSeguidoService;
    public GetUsuariosSeguidosController(UsuarioSeguidoService usuarioSeguidoService)
    {
        _usuarioSeguidoService = usuarioSeguidoService;
    }

    [HttpGet("ObtenerSeguidos")]
    public async Task<IActionResult> ObtenerUsuariosSeguidos([FromQuery] Guid usuarioId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var usuarioSeguidos = await _usuarioSeguidoService.ObtenerUsuariosSeguidosPorUsuario(usuarioId);

            if (usuarioSeguidos != null)
            {
                return Ok(usuarioSeguidos);
            }

            return BadRequest(new
            {
                Codigo = 400,
                Mensaje = "no se encontro usuarios seguidos"
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

    [HttpGet("SiguiendoUsuario")]
    public async Task<IActionResult> ObtenerUsuariosSeguidos([FromQuery] Guid usuarioId, string usuarioSeguido)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var siguiendoUsuario = await _usuarioSeguidoService.EstaSiguiendoUsuario(usuarioId, usuarioSeguido);

            if (siguiendoUsuario)
            {
                return Ok(siguiendoUsuario);
            }
            else
            {
                return BadRequest(siguiendoUsuario);
            }
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

