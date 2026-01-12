using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;
public class Valoracion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public Guid UsuarioId { get; set; }
    
    [Required]
    public int LibroId { get; set; }
    [Required]
    public int Puntuacion { get; set; } // Valoración del 1 al 5

    public Usuario Usuario { get; set; }
    public Libro Libro { get; set; }
}
