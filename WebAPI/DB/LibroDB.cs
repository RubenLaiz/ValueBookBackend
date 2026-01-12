using AutoMapper;
using EBGBackend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebAPI.Extensions;
using WebAPI.Mapping;
using WebAPI.Models;

namespace WebAPI.DB;


[RegisterService(ServiceLifetime.Transient)]
public class LibroDB
{
    private readonly AppDbContext _context;
    private readonly IMapper mapper;

    public LibroDB(AppDbContext context)
    {
        _context = context;
        mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>()));
    }

    /*
    * ------------------------------------------------------------
    * ---------------------- MÉTODOS GET -------------------------
    * ------------------------------------------------------------
    */

    public async Task<List<Libro>> ObtenerLibros()
    {
        var libros = await _context.Libros.ToListAsync();
        return mapper.Map<List<Libro>>(libros);
    }

    public async Task<Libro> ObtenerLibroId(int libroId)
    {
        var libroExistente = await _context.Libros.FindAsync(libroId);
        if (libroExistente != null)
        {
            return mapper.Map<Libro>(libroExistente);
        }
        else
        {
            return null;
        }
    }

    public async Task<List<Libro>> ObtenerLibrosTitulo(string titulo)
    {
        var libroExistente = await _context.Libros
            .Where(l => l.Titulo.ToLower().Contains(titulo.ToLower()))
            .ToListAsync();
        if (libroExistente != null)
        {
            return mapper.Map<List<Libro>>(libroExistente);
        }
        else
        {
            return null;
        }
    }

    public async Task<List<Libro>> ObtenerLibros(string titulo, string autor, string anio)
    {
        var query = _context.Libros.AsQueryable();

        if (!string.IsNullOrWhiteSpace(titulo))
        {
            query = query.Where(l =>
                l.Titulo.ToLower().Contains(titulo.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(autor))
        {
            query = query.Where(l =>
                l.Autor.ToLower().Contains(autor.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(anio))
        {
            query = query.Where(l =>
                l.Anio.ToString().Contains(anio));
        }

        var libros = await query.ToListAsync();

        return mapper.Map<List<Libro>>(libros);
    }
    

    /*
     * ------------------------------------------------------------
     * ---------------------- MÉTODOS ADD -------------------------
     * ------------------------------------------------------------
     */

    public async Task<Libro> AgregarLibro(Libro nuevoLibro) {    
        var libroEntidad = mapper.Map<Infrastructure.Entities.Libro>(nuevoLibro);
        _context.Libros.Add(libroEntidad);
        await  _context.SaveChangesAsync();
        return mapper.Map<Libro>(libroEntidad);
    }

    /*
     * ------------------------------------------------------------
     * --------------------- MÉTODOS UPDATE -----------------------
     * ------------------------------------------------------------
     */

    public async Task<Libro> ActualizarLibro(int libroId, Libro libroActualizado)
    {
        var libroExistente = await _context.Libros.FindAsync(libroId);
        if (libroExistente != null)
        {
            libroExistente.Titulo = libroActualizado.Titulo;
            libroExistente.Autor = libroActualizado.Autor;
            await _context.SaveChangesAsync();
            return mapper.Map<Libro>(libroExistente);
        }
        else
        {
            return null;
        }
    }
}
