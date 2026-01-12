namespace WebAPI.Models;

public class UsuarioSeguido
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }    
    public Guid SeguidoId { get; set; }
}
