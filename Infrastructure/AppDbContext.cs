using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using static Infrastructure.Atributos.Atributos;

namespace EBGBackend.Infrastructure;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {}

    // Constructor sin parámetros que se usará cuando se cree el DbContext manualmente
    public AppDbContext() { }

    // Configuración de la base de datos (cadena de conexión predeterminada)
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Aquí defines tu cadena de conexión predeterminada
            optionsBuilder.UseNpgsql("Server=Localhost;Port=5432;Database=valuebook;;Username=admin;Password=admin123");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Buscar las propiedades con el atributo [Unique]
        var propertiesWithUnique = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.PropertyInfo.GetCustomAttributes(typeof(Unique), false).Any());

        var propertiesWithCurrentDateTime = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.PropertyInfo.GetCustomAttributes(typeof(CurrentDateTime), false).Any());

        // Crear índices únicos en esas propiedades
        foreach (var property in propertiesWithUnique)
        {
            modelBuilder.Entity(property.DeclaringEntityType.ClrType)
                .HasIndex(property.Name)
                .IsUnique();
        }

        // Crear índices normales en propiedades con [CurrentDateTime] (pero NO únicos)
        foreach (var property in propertiesWithCurrentDateTime)
        {
            // Asignar valor por defecto en la base de datos (para PostgreSQL)
            modelBuilder.Entity(property.DeclaringEntityType.ClrType)
                .Property(property.Name)                
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();
        }

    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Libro> Libros { get; set; }
    public DbSet<Valoracion> Valoraciones { get; set; }
    public DbSet<Comentario> Comentarios { get; set; }
    public DbSet<UsuarioSeguido> UsuariosSeguidos { get; set; }
}
