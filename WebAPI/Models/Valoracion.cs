namespace WebAPI.Models;

public class Valoracion
{
    public string Id { get; set; }
    public string UsuarioId { get; set; }
    public int LibroId { get; set; }
    public int Puntuacion { get; set; }

}

