using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers.Usuarios;


[Route("Api/Usuario")]
[ApiController]
public class GetUsuariosController : ControllerBase
{

    private UsuariosService _usuariosService;

    public GetUsuariosController(UsuariosService usuariosService) 
    {
        _usuariosService = usuariosService;
    }

    [HttpGet("ObtenerPorId")]
    public async Task<IActionResult> ObtenerUsuarioPorId([FromQuery] string usuarioId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var usuario = await _usuariosService.ObtenerUsuarioPorId(usuarioId);

            if (usuario != null)
            {
                return Ok(usuario);
            }

            return BadRequest(new 
            {
                Codigo = 404,
                Mensaje = "no existe el usuario" 
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

    [HttpGet("ObtenerPorNombre")]
    public async Task<IActionResult> ObtenerUsuarioPorNombre([FromQuery] string nombre)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var usuario = await _usuariosService.ObtenerUsuarioPorNombre(nombre);

            if (usuario != null)
            {
                return Ok(new
                {
                    Codigo = 200,
                    Usuario = usuario
                });
            }

            return BadRequest(new
            {
                Codigo = 404,
                Mensaje = "no existe el usuario"
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


    [HttpPost("Login")]    
    public async Task<IActionResult> Login([FromBody]Usuario.UsuarioInput Usuario)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var token = await _usuariosService.Login(Usuario);

            if (token != null)
            {
                return Ok(new
                {
                    Data = new{
                        UsuarioId = token.UsuarioId,
                        Token = token.Token,
                        Nombre = token.Nombre,
                        Rol = token.Rol
                    },
                    Codigo = 0,
                });
            } 
            
            return BadRequest(new { Mensaje = "Error de login" });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new
            {
                Error = 401,
                Mensaje = ex.Message
            });
        }
        catch(ArgumentException ex)
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
