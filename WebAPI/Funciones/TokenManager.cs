using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Extensions;
using WebAPI.Models;

namespace WebAPI.Funciones;

[RegisterService(ServiceLifetime.Singleton)]
public class TokenManager
{

    private readonly string _claveSecreta;

    public TokenManager(IConfiguration configuration)
    {
        _claveSecreta = configuration["Jwt:SecretKey"];
    }

    public async Task<string> GenerarToken(Guid usuarioId, string usuario, string rol)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_claveSecreta));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
                    new Claim(ClaimTypes.NameIdentifier, usuarioId.ToString()),
                    new Claim(ClaimTypes.Name, usuario),
                    new Claim(ClaimTypes.Role, rol)
            };

        var token = new JwtSecurityToken(
            issuer: "miapi",
            audience: "micliente",
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<Token.TokenDetalles> LeerToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_claveSecreta);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = "miapi",
                ValidateAudience = true,
                ValidAudience = "micliente",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var claims = principal.Claims.ToDictionary(c => c.Type, c => c.Value);

            return new Token.TokenDetalles
            {
                Token = token,
                UsuarioId = claims.TryGetValue(ClaimTypes.NameIdentifier, out var id) ? id : null,
                Nombre = claims.TryGetValue(ClaimTypes.Name, out var nombre) ? nombre : null,
                Rol = claims.TryGetValue(ClaimTypes.Role, out var rol) ? rol : null
            };
        }
        catch
        {
            // Puedes lanzar una excepción personalizada o devolver null según tu lógica
            return null;
        }
    }

}
