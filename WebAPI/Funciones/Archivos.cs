using WebAPI.Extensions;

namespace WebAPI.Funciones;

[RegisterService(ServiceLifetime.Transient)]
public class Archivos
{
    private readonly ILogger<Archivos> _logger;

    public Archivos(ILogger<Archivos> logger)
    {
        _logger = logger;
    }

    public Archivos()
    {
    }

    public async Task<int> GuardarArchivo(string ruta, IFormFile archivo, string nombreArchivo = null)
    {
        try
        {
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }

            var filePath = Path.Combine(ruta, nombreArchivo ?? archivo.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            return 1; // Éxito
        }
        catch(Exception ex)
        {
            _logger.LogError($"Clase: Archivos -> GuardarArchivo - Error al guardar el archivo: {ex.Message}");
            return 0; // Error
        }
    }

    public async Task<int> EliminarArchivo(string ruta, string nombreArchivoSinExtension)
    {
        try
        {
            // Buscar el archivo con el nombre sin extensión
            var archivoAEliminar = BuscarArchivoPorNombre(ruta, nombreArchivoSinExtension);

            if (archivoAEliminar != null)
            {
                // Eliminar el archivo
                File.Delete(archivoAEliminar);
                return 1; // Éxito
            }
            else
            {
                _logger.LogWarning($"Clase: Archivos -> EliminarArchivo - El archivo a eliminar no existe: {nombreArchivoSinExtension}");
                return 0; // El archivo no existe
            }
        }
        catch (Exception ex)
        {
            // Log o manejo del error            
            _logger.LogError($"Clase: Archivos -> EliminarArchivo - Error al eliminar el archivo: {ex.Message}");
            return 0; // Error
        }
    }

    public async Task<int> RenombrarArchivo(string ruta, string nombreArchivoSanitizadoSinExtensionAntiguo, string nombreArchivoNuevo)
    {
        try
        {
            // Buscar el archivo con el nombre sin extensión
            var archivoAntiguo = BuscarArchivoPorNombre(ruta, nombreArchivoSanitizadoSinExtensionAntiguo);

            if (archivoAntiguo != null)
            {
                // Obtener la extensión del archivo encontrado
                var extension = Path.GetExtension(archivoAntiguo);

                // Construir la ruta del archivo nuevo con la extensión original
                var filePathNuevo = Path.Combine(ruta, nombreArchivoNuevo + extension);

                // Renombrar el archivo (esto equivale a moverlo con un nuevo nombre)
                File.Move(archivoAntiguo, filePathNuevo);
                _logger.LogInformation($"Clase: Archivos -> RenombrarArchivo - El archivo se renombro: {nombreArchivoSanitizadoSinExtensionAntiguo} a {nombreArchivoNuevo}");
                return 1; // Éxito
            }
            else
            {
                _logger.LogWarning($"Clase: Archivos -> RenombrarArchivo - El archivo a renombrar no existe: {nombreArchivoSanitizadoSinExtensionAntiguo}");
                return 0; // El archivo antiguo no existe
            }
        }
        catch (Exception ex)
        {
            // Log o manejo del error            
            _logger.LogError($"Clase: Archivos -> RenombrarArchivo - Error al renombrar el archivo: {ex.Message}");
            return 0; // Error
        }
    }

    public async Task<FileStream> LeerArchivo(string rutaArchivo)
    {
        try
        {
            if (!File.Exists(rutaArchivo))
            {
                return null; // Retorna null si el archivo no existe
            }

            // Abrir un FileStream para el archivo especificado
            var fileStream = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read);

            return fileStream;
        }
        catch(Exception ex)
        {
            _logger.LogError($"Clase: Archivos -> LeerArchivo - Error al leer el archivo: {ex.Message}");
            return null; // O maneja la excepción de manera adecuada según tu lógica
        }
    }

    public string BuscarArchivoPorNombre(string ruta, string nombreArchivoSinExtension)
    {
        try
        {
            // Obtener todos los archivos en el directorio especificado
            var archivos = Directory.GetFiles(ruta);

            // Buscar el archivo cuyo nombre coincida con el nombre sin la extensión
            foreach (var archivo in archivos)
            {
                var nombreArchivo = Path.GetFileNameWithoutExtension(archivo); // Obtener el nombre sin extensión
                if (nombreArchivo.Equals(nombreArchivoSinExtension, StringComparison.OrdinalIgnoreCase))
                {
                    return archivo; // Retorna la ruta completa del archivo
                }
            }

            return null; // Retorna null si no encuentra el archivo
        }
        catch (Exception ex)
        {
            // Manejo de errores            
            _logger.LogError($"Clase: Archivos -> BuscarArchivoPorNombre - Error al buscar el archivo: {ex.Message}");
            return null;
        }
    }
}
