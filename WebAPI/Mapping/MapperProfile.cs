using AutoMapper;
using WebAPI.Models;

namespace WebAPI.Mapping;

public class MapperProfile : Profile
{
    public MapperProfile()
    {

        // Mapeo de Usuario
        CreateMap<Usuario.UsuarioBase, Infrastructure.Entities.Usuario>()
            .ReverseMap();
        CreateMap<Usuario.UsuarioBase, Usuario.UsuarioInput>()
            .ReverseMap();
        CreateMap<Usuario.UsuarioBase, Usuario.UsuarioOutput>()
            .ReverseMap();

        CreateMap<Valoracion, Infrastructure.Entities.Valoracion>()
            .ReverseMap();

        CreateMap<Libro, Infrastructure.Entities.Libro>()
            .ReverseMap();

        CreateMap<Comentario, Infrastructure.Entities.Comentario>()
            .ReverseMap();

        CreateMap<UsuarioSeguido, Infrastructure.Entities.UsuarioSeguido>()
            .ReverseMap();
    }
}
