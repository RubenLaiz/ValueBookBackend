namespace Infrastructure.Entities;

public class UsuarioSeguido
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }    
    public Guid SeguidoId { get; set; }
    public Usuario Usuario { get; set; }
    public Usuario Seguido { get; set; }
}
