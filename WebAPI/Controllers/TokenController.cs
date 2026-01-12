using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Services;

namespace WebAPI.Controllers;

[Route("Api/Token")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly TokenService _tokenService;
    public TokenController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet("Verificar")]    
    public async Task<IActionResult> VerificarToken([FromQuery]string token)
    {
        try
        {
            var tokenVerificado = await _tokenService.VerificarToken(token);

            if (tokenVerificado)
            {
                return Ok(new
                {
                    Codigo = 200, 
                    Mensaje = "Token correcto"
                });
            }

            return BadRequest(new
            {
                Error = 401,
                Mensaje = "Token no valido"
            });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new
            {
                Error = 400,
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
