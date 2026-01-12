namespace WebAPI.Models;

public class Token
{

    public class TokenBase
    {
        public string Token { get; set; }        
    }

    public class TokenDetalles : TokenBase
    {
        public string UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }
    }
}
