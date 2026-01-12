 using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Infrastructure.Atributos.Atributos;

namespace Infrastructure.Entities;

public class Usuario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]    
    [Unique]
    public string Nombre { get; set; }

    [Required]
    [MaxLength(500)]
    public string Contrasena { get; set; }

    [Required]
    public int Seguidores { get; set; } = 0;

    [Required]
    public TipoUsuario Rol {  get; set; }


    public enum TipoUsuario
    {
        SuperAdmin,
        Admin,
        Regular
    }
}
