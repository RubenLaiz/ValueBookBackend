using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class Libro
{
    [Key]    
    public int Id { get; set; }

    [Required]
    [MaxLength(500)]    
    public string Titulo { get; set; }

    [Required]
    [MaxLength(100)]    
    public string Autor { get; set; }

    [Required]    
    public int Anio { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Descripcion { get; set; }
}
