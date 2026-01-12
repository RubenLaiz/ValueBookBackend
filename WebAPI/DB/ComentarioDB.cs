using AutoMapper;
using EBGBackend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebAPI.Extensions;
using WebAPI.Mapping;
using WebAPI.Models;

namespace WebAPI.DB;

[RegisterService(ServiceLifetime.Transient)]
public class ComentarioDB
{
    private readonly AppDbContext _context;
    private readonly IMapper mapper;

    public ComentarioDB(AppDbContext context)
    {
        _context = context;
        mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>()));
    }

    /*
    * ------------------------------------------------------------
    * ---------------------- MÉTODOS GET -------------------------
    * ------------------------------------------------------------
    */

    public async Task<Comentario> ObtenerComentario(int libroId, string nombreUsuario) {
        var comentario = await _context.Comentarios
            .FirstOrDefaultAsync(c => c.LibroId == libroId && c.NombreUsuario.Trim() == nombreUsuario.Trim());
        return mapper.Map<Comentario>(comentario);
    }

    public async Task<List<Comentario>> ObtenerComentariosLibro(int libroId) {
        var comentarios = _context.Comentarios.Where(c => c.LibroId == libroId).ToList();
        return mapper.Map<List<Comentario>>(comentarios);
    }

    /*
    * ------------------------------------------------------------
    * ---------------------- MÉTODOS ADD -------------------------
    * ------------------------------------------------------------
    */

    public async Task<bool> AgregarComentario(Comentario nuevoComentario) {    
        var comentarioEntidad = mapper.Map<Infrastructure.Entities.Comentario>(nuevoComentario);
        _context.Comentarios.Add(comentarioEntidad);
        await  _context.SaveChangesAsync();
        return true;
    }

    /*
    * ------------------------------------------------------------
    * --------------------- MÉTODOS DELETE -----------------------
    * ------------------------------------------------------------
    */

    public async Task<bool> EliminarComentario(Guid? comentarioId)
    {
        var comentarioEntidad = await _context.Comentarios.FindAsync(comentarioId);
        if (comentarioEntidad == null) {
            return false;
        }
        _context.Comentarios.Remove(comentarioEntidad);
        await _context.SaveChangesAsync();
        return true;
    }

}
