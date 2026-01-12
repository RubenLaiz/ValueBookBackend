using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers.Valoraciones;

[Route("Api/Valoracion")]
[ApiController]
public class UpdateValoracionesController : ControllerBase
{
    private ValoracionService _valoracionService;
    public UpdateValoracionesController(ValoracionService valoracionService)
    {
        _valoracionService = valoracionService;
    }

    [HttpPost("Actualizar")]
    public async Task<IActionResult> ActualizarValoracion([FromBody] Valoracion valoracion)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var valoracionRecibida = await _valoracionService.ActualizarValoracion(valoracion);

            if (valoracion != null)
            {
                return Ok(new
                {
                    Codigo = 200,
                    Valoracion = valoracionRecibida
                });
            }

            return BadRequest(new
            {
                Codigo = 404,
                Mensaje = "no se pudo actualizar la valoración"
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
