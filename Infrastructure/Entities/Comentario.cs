using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class Comentario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [Required]
    public string Contenido { get; set; }
    [Required]
    public DateTime FechaCreacion { get; set; }
    [Required]
    public int LibroId { get; set; }
    [Required]
    public string NombreUsuario { get; set; }
}
