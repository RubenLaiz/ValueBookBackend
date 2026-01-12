namespace WebAPI.Constantes;

public static class RutasArchivos
{
    // Ruta del programa global
    private static readonly string RutaDirectorioGlobal = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../"));

    // Ruta a las portadas
    public static readonly string PORTADAS = Path.Combine(RutaDirectorioGlobal, "WebApi/Portadas");    
    
}
