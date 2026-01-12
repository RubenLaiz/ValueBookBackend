using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using System.ComponentModel.DataAnnotations;
using WebAPI.Services;

namespace WebAPI.Controllers.Valoraciones;

[Route("Api/Valoracion")]
[ApiController]
public class GetValoracionesController : ControllerBase
{

    private readonly ValoracionService _valoracionService;
    public GetValoracionesController(ValoracionService valoracionService)
    {
        _valoracionService = valoracionService;
    }

    [HttpGet("Obtener")]
    public async Task<IActionResult> ObtenerValoracion([FromQuery] Guid usuarioId, [FromQuery] int libroId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var valoracion = await _valoracionService.ObtenerValoracion(usuarioId, libroId);

            if (valoracion != null)
            {
                return Ok(valoracion);
            }

            return BadRequest(new
            {
                Codigo = 404,
                Mensaje = "no existe la valoracion"
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


    [HttpGet("ObtenerValoracionesUsuario")]
    public async Task<IActionResult> ObtenerValoracionesUsuario([FromQuery] Guid usuarioId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var valoracion = await _valoracionService.ObtenerValoracionesUsuario(usuarioId);

            if (valoracion != null)
            {
                return Ok(valoracion);
            }

            return BadRequest(new
            {
                Codigo = 404,
                Mensaje = "no existe la valoracion"
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

    [HttpGet("ObtenerValoracionesUsuarioNombre")]
    public async Task<IActionResult> ObtenerValoracionesUsuarioNombre([FromQuery] string nombre)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var valoracion = await _valoracionService.ObtenerValoracionesUsuarioNombre(nombre);

            if (valoracion != null)
            {
                return Ok(valoracion);
            }

            return BadRequest(new
            {
                Codigo = 404,
                Mensaje = "no existe la valoracion"
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

    [HttpGet("ObtenerPuntuacionLibro")]
    public async Task<IActionResult> ObtenerValoracionesLibro([FromQuery] int libroId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var puntuacion = await _valoracionService.ObtenerValoracionesPorLibro(libroId);

            if (puntuacion != null)
            {
                return Ok(puntuacion);
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
