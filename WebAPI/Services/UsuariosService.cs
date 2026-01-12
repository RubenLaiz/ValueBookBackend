using AutoMapper;
using System.ComponentModel.DataAnnotations;
using WebAPI.DB;
using WebAPI.Extensions;
using WebAPI.Funciones;
using WebAPI.Mapping;
using WebAPI.Models;

namespace WebAPI.Services;

[RegisterService(ServiceLifetime.Transient)]
public class UsuariosService
{
    private readonly UsuarioDB _usuarioDB;
    private readonly IMapper mapper;
    private readonly TokenManager _tokenManager;
    private readonly ILogger<UsuariosService> _logger;  
    public UsuariosService(UsuarioDB usuarioDb, TokenManager tokenManager, ILogger<UsuariosService> logger)
    {
        _usuarioDB = usuarioDb;
        _tokenManager = tokenManager;
        mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>()));
        _logger = logger;
    }

    /*
     * ------------------------------------------------------------
     * ---------------------- MÉTODOS GET -------------------------
     * ------------------------------------------------------------
     */

    public async Task<Usuario.UsuarioOutput> ObtenerUsuarioPorId(string usuarioId)
    {
        var usuarioExistente = await _usuarioDB.ObtenerUsuarioId(Guid.Parse(usuarioId));
        if (usuarioExistente != null)
        {
            return mapper.Map<Usuario.UsuarioOutput>(usuarioExistente);
        }
        else
        {
            return null;
        }
    }

    public async Task<Usuario.UsuarioOutput> ObtenerUsuarioPorNombre(string nombre)
    {
        var usuarioExistente = await _usuarioDB.ObtenerUsuarioNombre(nombre);
        if (usuarioExistente != null)
        {
            return mapper.Map<Usuario.UsuarioOutput>(usuarioExistente);
        }
        else
        {
            return null;
        }
    }

    public async Task<Token.TokenDetalles> Login(Usuario.UsuarioInput usuario)
    {
        if (string.IsNullOrEmpty(usuario.Nombre))
        {
            throw new ValidationException("El nombre no puede estar vacio");
        }

        Token.TokenDetalles jwt = null;
        Usuario.UsuarioBase usuarioMapeado = mapper.Map<Usuario.UsuarioBase>(usuario);
        int usuarioLogeado = await _usuarioDB.VerificarCredencialesUsuario(usuarioMapeado);

        if(usuarioLogeado == 1)
        {
            var usuarioExistente = await _usuarioDB.ObtenerUsuarioNombre(usuarioMapeado.Nombre);            

            jwt = new Token.TokenDetalles
            {
                Token = await _tokenManager.GenerarToken(usuarioExistente.Id, usuarioExistente.Nombre, usuarioExistente.Rol.ToString()),
                Rol = usuarioExistente.Rol.ToString(),
                Nombre = usuarioExistente.Nombre,
                UsuarioId = usuarioExistente.Id.ToString()
            };            
        }
        else if(usuarioLogeado == -1)
        {
            throw new ArgumentException("El usuario o la contraseña es incorrecta");
        }
        else if( usuarioLogeado == -2)
        {
            _logger.LogWarning("Intento de inicio de sesión fallido para el usuario: {Usuario}", usuarioMapeado.Nombre);
            throw new ArgumentException("El usuario no existe");            
        }

        return jwt;
    }


    /*
     * ------------------------------------------------------------
     * ---------------------- MÉTODOS ADD -------------------------
     * ------------------------------------------------------------
     */


    public async Task<bool> RegistrarUsuario(Usuario.UsuarioInput usuario)
    {

        if (string.IsNullOrEmpty(usuario.Nombre))
        {
            throw new ValidationException("El nombre no puede estar vacio");
        }

        bool estadoOperacion = false;

        var usuarioMapeado = mapper.Map<Usuario.UsuarioBase>(usuario);
        usuarioMapeado.Rol = Usuario.TipoUsuario.Regular;

        
        // Comprueba que no existe un usuario con el mismo nombre
        if (await _usuarioDB.ObtenerUsuarioNombre(usuarioMapeado.Nombre) == null)
        {
            estadoOperacion = await _usuarioDB.AgregarUsuario(usuarioMapeado);       
        }
        else
        {
            throw new ArgumentException("El usuario ya existe");
        }

        return estadoOperacion;

    }
}
