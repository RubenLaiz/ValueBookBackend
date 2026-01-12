using WebAPI.DB;
using WebAPI.Extensions;
using WebAPI.Models;

namespace WebAPI.Services;

[RegisterService(ServiceLifetime.Transient)]
public class ValoracionService
{

    private readonly ValoracionDB _valoracionDB;
    private readonly UsuarioDB _usuarioDB;
    private readonly LibroDB _libroDB;
    public ValoracionService(ValoracionDB valoracionDB, UsuarioDB usuarioDB, LibroDB libroDB)
    {
        _valoracionDB = valoracionDB;
        _usuarioDB = usuarioDB;
        _libroDB = libroDB;
    }


    public async Task<Valoracion> ObtenerValoracion(Guid usuarioid, int libroId)
    {
        var valoracion = await _valoracionDB.ObtenerValoracionUsuarioLibro(usuarioid, libroId);

        return valoracion;
    }

    public async Task<List<Valoracion>> ObtenerValoracionesUsuario(Guid usuarioId)
    {
        var valoraciones = await _valoracionDB.ObtenerValoracionesPorUsuario(usuarioId);
        return valoraciones;
    }

    public async Task<List<Valoracion>> ObtenerValoracionesUsuarioNombre(string nombre)
    {
        var usuario = await _usuarioDB.ObtenerUsuarioNombre(nombre);
        var valoraciones = await _valoracionDB.ObtenerValoracionesPorUsuario(usuario.Id);
        return valoraciones;
    }

    public async Task<int> ObtenerValoracionesPorLibro(int libroId)
    {
        var valoraciones = await _valoracionDB.ObtenerValoracionesPorLibro(libroId);

        int puntuacion = 0;

        if (valoraciones != null)
        {
            foreach (var valoracion in valoraciones)
            {
                puntuacion += valoracion.Puntuacion;
            }
        }

        return puntuacion/valoraciones.Count;
    }

    /*
    * ------------------------------------------------------------
    * -------------------- MÉTODOS ADD ---------------------------
    * ------------------------------------------------------------
    */

    public async Task<Valoracion> AgregarValoracion(Valoracion nuevaValoracion)
    {
        if (nuevaValoracion == null)
        {
            throw new ArgumentNullException(nameof(nuevaValoracion));
        }
        var valoracion = await _valoracionDB.AgregarValoracion(nuevaValoracion);
        return valoracion;
    }

    /*
    * ------------------------------------------------------------
    * -------------------- MÉTODOS UPDATE ------------------------
    * ------------------------------------------------------------
    */

    public async Task<Valoracion> ActualizarValoracion(Valoracion valoracionActualizada)
    {
        var valoracion = await _valoracionDB.ActualizarValoracion(valoracionActualizada);
        return valoracion;
    }
}
