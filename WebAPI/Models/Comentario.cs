namespace WebAPI.Models;

public class Comentario
{
    public Guid? Id { get; set; }
    public string Contenido { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public int LibroId { get; set; }
    public string NombreUsuario { get; set; }
}
