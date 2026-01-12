using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers.Usuarios;


[Route("Api/Usuario")]
[ApiController]
public class AddUsuariosController : ControllerBase
{

    private UsuariosService _usuariosService;

    public AddUsuariosController(UsuariosService usuariosService)
    {
        _usuariosService = usuariosService;
    }

    [HttpPost("RegistrarUsuario")]
    public async Task<IActionResult> RegistrarUsuario([FromBody] Usuario.UsuarioInput Usuario)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var resultado = await _usuariosService.RegistrarUsuario(Usuario);

            if (resultado != null)
            {
                return Ok(new
                {
                    Mensaje = "Usuario registrado con exito"
                });
            }

            return BadRequest(new { Mensaje = "Error de registro" });
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
