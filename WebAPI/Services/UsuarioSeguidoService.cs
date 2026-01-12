using WebAPI.DB;
using WebAPI.Extensions;
using WebAPI.Models;

namespace WebAPI.Services;

[RegisterService(ServiceLifetime.Transient)]
public class UsuarioSeguidoService
{

    private readonly UsuarioSeguidoDB _usuarioSeguidoDB; 
    private readonly UsuarioDB _usuarioDB;
    public UsuarioSeguidoService(UsuarioSeguidoDB usuarioSeguidoDB, UsuarioDB usuarioDB)
    {
        _usuarioSeguidoDB = usuarioSeguidoDB;
        _usuarioDB = usuarioDB;
    }

    public async Task<List<UsuarioSeguido>> ObtenerUsuariosSeguidosPorUsuario(Guid usuarioId)
    {
        return await _usuarioSeguidoDB.ObtenerUsuariosSeguidosPorUsuario(usuarioId);
    }

    public async Task<bool> EstaSiguiendoUsuario(Guid usuarioId, string usuarioSeguido)
    {
        var usuarioSeguidoEncontrado = await _usuarioDB.ObtenerUsuarioNombre(usuarioSeguido);
        var usuariosSeguidos = await _usuarioSeguidoDB.ObtenerUsuariosSeguidosPorUsuario(usuarioId);
        return usuariosSeguidos.Any(us => us.SeguidoId == usuarioSeguidoEncontrado.Id);
    }

    public async Task<bool> AgregarUsuarioSeguido(Guid usuarioId, string usuarioSeguido)
    {
        var usuarioSeguidoEncontrado = await _usuarioDB.ObtenerUsuarioNombre(usuarioSeguido);
        var nuevoUsuarioSeguido = new UsuarioSeguido
        {
            UsuarioId = usuarioId,
            SeguidoId = usuarioSeguidoEncontrado.Id
        };
        return await _usuarioSeguidoDB.AgregarUsuarioSeguido(nuevoUsuarioSeguido);
    }

    public async Task<bool> EliminarUsuarioSeguido(Guid usuarioId, string usuarioSeguido)
    {
        var usuarioSeguidoEncontrado = await _usuarioDB.ObtenerUsuarioNombre(usuarioSeguido);
        return await _usuarioSeguidoDB.EliminarUsuarioSeguido(usuarioId, usuarioSeguidoEncontrado.Id);
    }
}
