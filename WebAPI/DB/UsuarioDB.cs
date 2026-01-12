using AutoMapper;
using EBGBackend.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAPI.Extensions;
using WebAPI.Mapping;
using WebAPI.Models;

namespace WebAPI.DB;

[RegisterService(ServiceLifetime.Transient)]
public class UsuarioDB
{

    private readonly AppDbContext _context;
    private readonly IMapper mapper;

    public UsuarioDB(AppDbContext context)
    {
        _context = context;
        mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>()));
    }

    /*
     * ------------------------------------------------------------
     * ---------------------- MÉTODOS GET -------------------------
     * ------------------------------------------------------------
     */

    public async Task<Usuario.UsuarioBase> ObtenerUsuarioId(Guid usuarioId)
    {

        var usuarioExistente = await _context.Usuarios.FindAsync(usuarioId);

        if(usuarioExistente != null)
        {
            return mapper.Map<Usuario.UsuarioBase>(usuarioExistente);
        } 
        else
        {
            return null;
        }
    }

    public async Task<Usuario.UsuarioBase> ObtenerUsuarioNombre(string nombreUsuario)
    {

        var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == nombreUsuario.ToLower());

        if (usuarioExistente != null)
        {
            return mapper.Map<Usuario.UsuarioBase>(usuarioExistente);
        }
        else
        {
            return null;
        }
    }


    /// <summary>
    /// Método para verificar las credenciales de un usuario.
    /// </summary>
    /// <param name="usuario">Objeto que contiene el nombre y la contraseña del usuario.</param>
    /// <returns>Estado de la operación (1: éxito, -1: contraseña incorrecta, -2: usuario no encontrado).</returns>
    public async Task<int> VerificarCredencialesUsuario(Usuario.UsuarioBase usuario)
    {
        int estadoOperacion = 0;
        var usuarioExistente = mapper.Map<Usuario.UsuarioBase>(await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == usuario.Nombre.ToLower()));

        if (usuarioExistente != null)
        {
            if (VerificarContrasena(usuario.Nombre.ToLower(), usuario.Contrasena, usuarioExistente.Contrasena))
            {
                estadoOperacion = 1;
            }
            else
            {
                estadoOperacion = -1;
            }
        }
        else
        {
            estadoOperacion = -2;
        }

        return estadoOperacion;
    }

    /*
     * ------------------------------------------------------------
     * ---------------------- MÉTODOS ADD -------------------------
     * ------------------------------------------------------------
     */

    public async Task<bool> AgregarUsuario(Usuario.UsuarioBase usuario)
    {
        bool estadoOperacion = false;
        try
        {
            usuario.Contrasena = EncriptarContrasena(usuario.Nombre.ToLower(), usuario.Contrasena);
            usuario.Nombre = usuario.Nombre.ToLower();

            var nuevoUsuario = mapper.Map<Infrastructure.Entities.Usuario>(usuario);

            await _context.Usuarios.AddAsync(nuevoUsuario);
            await _context.SaveChangesAsync();

            estadoOperacion = true;
        }
        catch (Exception ex)
        {           
        }     

        return estadoOperacion;
    }

    /*
     * ------------------------------------------------------------
     * --------------- MÉTODOS PRIVADOS GENERALES -----------------
     * ------------------------------------------------------------
     */

    private string EncriptarContrasena(string nombre, string contrasena)
    {
        PasswordHasher<string> hasher = new PasswordHasher<string>();
        return hasher.HashPassword(nombre, contrasena);        
    }

    private bool VerificarContrasena(string nombre, string contrasenaIngresada, string hashAlmacenado)
    {
        PasswordHasher<string> hasher = new PasswordHasher<string>();
        var result = hasher.VerifyHashedPassword(nombre, hashAlmacenado, contrasenaIngresada);        
        return result == PasswordVerificationResult.Success;
    }
}
