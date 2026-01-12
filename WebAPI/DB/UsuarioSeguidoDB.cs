using AutoMapper;
using EBGBackend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebAPI.Extensions;
using WebAPI.Mapping;
using WebAPI.Models;

namespace WebAPI.DB;

[RegisterService(ServiceLifetime.Transient)]
public class UsuarioSeguidoDB
{
    private readonly AppDbContext _context;
    private readonly IMapper mapper;

    public UsuarioSeguidoDB(AppDbContext context)
    {
        _context = context;
        mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>()));
    }

    /*
    * ------------------------------------------------------------
    * ---------------------- MÉTODOS GET -------------------------
    * ------------------------------------------------------------
    */

    public async Task<List<UsuarioSeguido>> ObtenerUsuariosSeguidosPorUsuario(Guid usuarioId)
    {
        var usuariosSeguidos = _context.Set<Infrastructure.Entities.UsuarioSeguido>()
            .Where(us => us.UsuarioId == usuarioId)
            .ToList();

        var usuariosObtenidos = mapper.Map<List<UsuarioSeguido>>(usuariosSeguidos);
        return usuariosObtenidos;
    }


    /*
    * ------------------------------------------------------------
    * ---------------------- MÉTODOS ADD -------------------------
    * ------------------------------------------------------------
    */


    public async Task<bool> AgregarUsuarioSeguido(UsuarioSeguido nuevoUsuarioSeguido)
    {
        var usuarioSeguidoEntidad = mapper.Map<Infrastructure.Entities.UsuarioSeguido>(nuevoUsuarioSeguido);
        _context.Set<Infrastructure.Entities.UsuarioSeguido>().Add(usuarioSeguidoEntidad);
        await _context.SaveChangesAsync();
        return true;
    }

    /*
    * ------------------------------------------------------------
    * -------------------- MÉTODOS DELETE ------------------------
    * ------------------------------------------------------------
    */


    public async Task<bool> EliminarUsuarioSeguido(Guid usuarioId, Guid seguidoId)
    {
        var usuarioSeguidoExistente = await _context.Set<Infrastructure.Entities.UsuarioSeguido>()
            .Where(x => x.UsuarioId == usuarioId && x.SeguidoId == seguidoId)
            .FirstOrDefaultAsync();

        if (usuarioSeguidoExistente != null)
        {
            _context.Remove(usuarioSeguidoExistente);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

}
