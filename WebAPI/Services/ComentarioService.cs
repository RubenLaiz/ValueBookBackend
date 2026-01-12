using WebAPI.DB;
using WebAPI.Extensions;

namespace WebAPI.Services;

[RegisterService(ServiceLifetime.Transient)]
public class ComentarioService
{
    private readonly ComentarioDB _comentarioDB;

    public ComentarioService(ComentarioDB comentarioDB)
    {
        _comentarioDB = comentarioDB;
    }

    public async Task<List<Models.Comentario>> ObtenerComentariosLibro(int libroId)
    {
        var comentarios = await _comentarioDB.ObtenerComentariosLibro(libroId);
        return comentarios;
    }

    public async Task<bool> AgregarComentario(Models.Comentario nuevoComentario)
    {
        nuevoComentario.FechaCreacion = DateTime.UtcNow;
        var resultado = await _comentarioDB.AgregarComentario(nuevoComentario);
        return resultado;
    }

    public async Task<bool> EliminarComentario(int libroId, string nombreUsuario)
    {
        var comentario = await _comentarioDB.ObtenerComentario(libroId, nombreUsuario);
        var resultado = await _comentarioDB.EliminarComentario(comentario.Id);
        return resultado;
    }
}
