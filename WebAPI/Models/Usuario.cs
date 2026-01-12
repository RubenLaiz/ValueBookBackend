namespace WebAPI.Models;

public class Usuario
{

    public class UsuarioBase
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }        
        public string Contrasena { get; set; }
        public TipoUsuario Rol {  get; set; }
    }

    public class UsuarioNullable
    {
        public Guid? Id { get; set; }
        public string? Nombre { get; set; }        
        public string? Contrasena { get; set; }
        public TipoUsuario? Rol { get; set; }
    }


    public class UsuarioInput
    {
        public string? Nombre { get; set; }       
        public string Contrasena { get; set; }
    }


    public class UsuarioOutput
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }        
        public TipoUsuario Rol { get; set; }
    }  

    public enum TipoUsuario
    {
        SuperAdmin,
        Admin,
        Regular
    }
}
