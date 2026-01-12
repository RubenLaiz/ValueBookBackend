using AutoMapper;
using EBGBackend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebAPI.Extensions;
using WebAPI.Mapping;
using WebAPI.Models;

namespace WebAPI.DB;


[RegisterService(ServiceLifetime.Transient)]
public class ValoracionDB
{
    private readonly AppDbContext _context;
    private readonly IMapper mapper;

    public ValoracionDB(AppDbContext context)
    {
        _context = context;
        mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>()));
    }

    /*
     * ------------------------------------------------------------
     * ---------------------- MÉTODOS GET -------------------------
     * ------------------------------------------------------------
     */

    public async Task<Valoracion> ObtenerValoracionUsuarioLibro(Guid usuarioId, int libroId)
    {
        var valoracionExistente = await _context.Valoraciones
            .FirstOrDefaultAsync(v => v.UsuarioId == usuarioId && v.LibroId == libroId);

        var valoracion = mapper.Map<Valoracion>(valoracionExistente);
        if (valoracionExistente != null)
        {
            return valoracion;
        }
        else
        {
            return null;
        }
    }

    public async Task<List<Valoracion>> ObtenerValoracionesPorUsuario(Guid usuarioId)
    {
        var valoraciones = await _context.Valoraciones
            .Where(v => v.UsuarioId == usuarioId)
            .ToListAsync();
        return mapper.Map<List<Valoracion>>(valoraciones);
    }

    public async Task<List<Valoracion>> ObtenerValoracionesPorLibro(int libroId)
    {
        var valoraciones = await _context.Valoraciones
            .Where(v => v.LibroId == libroId)
            .ToListAsync();
        return mapper.Map<List<Valoracion>>(valoraciones);
    }


    /*
     * ------------------------------------------------------------
     * ---------------------- MÉTODOS ADD -------------------------
     * ------------------------------------------------------------
     */

    public async Task<Valoracion> AgregarValoracion(Valoracion nuevaValoracion)
    {
        var entidadValoracion = mapper.Map<Infrastructure.Entities.Valoracion>(nuevaValoracion);
        _context.Valoraciones.Add(entidadValoracion);
        await  _context.SaveChangesAsync();
        return mapper.Map<Valoracion>(entidadValoracion);
    }

    /*
    * ------------------------------------------------------------
    * -------------------- MÉTODOS UPDATE ------------------------
    * ------------------------------------------------------------
    */

    public async Task<Valoracion> ActualizarValoracion(Valoracion valoracionActualizada)
    {
        var entidadValoracion = await _context.Valoraciones
            .FirstOrDefaultAsync(v => v.Id == Guid.Parse(valoracionActualizada.Id));
        if (entidadValoracion != null)
        {
            entidadValoracion.Puntuacion = valoracionActualizada.Puntuacion;
            _context.Valoraciones.Update(entidadValoracion);
            await _context.SaveChangesAsync();
            return mapper.Map<Valoracion>(entidadValoracion);
        }
        else
        {
            return null;
        }
    }
}
