using WebAPI.Extensions;
using WebAPI.Funciones;

namespace WebAPI.Services;

[RegisterService(ServiceLifetime.Transient)]
public class TokenService
{

    private readonly TokenManager _tokenManager;

    public TokenService(TokenManager tokenManager)
    {
        _tokenManager = tokenManager;
    }


    public async Task<bool> VerificarToken(string token)
    {
        bool operacion = true;
        var tokenVerificado = await _tokenManager.LeerToken(token);
        if (tokenVerificado == null)
            operacion = false;

        return operacion;
    }
}
