using WebAPI.DB;
using WebAPI.Extensions;
using WebAPI.Models;

namespace WebAPI.Services;

[RegisterService(ServiceLifetime.Transient)]
public class LibroService
{
    private readonly LibroDB _libroDB;

    public LibroService(LibroDB libro)
    {
        _libroDB = libro;
    }

    /*
   * ------------------------------------------------------------
   * ---------------------- MÉTODOS GET -------------------------
   * ------------------------------------------------------------
   */

    public async Task<object> ObtenerLibroId(int libroId)
    {
        var libroExistente = await _libroDB.ObtenerLibroId(libroId);
        if (libroExistente != null)
        {
            return libroExistente;
        }
        else
        {
            return null;
        }
    }

    public async Task<List<Libro>> ObtenerLibros(string titulo, string autor, string anio)
    {
        List<Libro> librosEncontrados;
        if (string.IsNullOrEmpty(titulo) && string.IsNullOrEmpty(autor) && string.IsNullOrEmpty(anio))
        {
            librosEncontrados = await _libroDB.ObtenerLibros();
        } else {
            librosEncontrados = await _libroDB.ObtenerLibros(titulo, autor, anio);
        }      
        
        if (librosEncontrados != null)
        {
            return librosEncontrados;
        }
        else
        {
            return null;
        }
    }

    /*
     * ------------------------------------------------------------
     * ---------------------- MÉTODOS ADD -------------------------
     * ------------------------------------------------------------
     */

    public async Task<Libro> AgregarLibro(Libro nuevoLibro) {   
        var libroCreado = await _libroDB.AgregarLibro(nuevoLibro);
        return libroCreado;
    }
}
