using Microsoft.AspNetCore.Mvc;
using WebAPI.Constantes;
using WebAPI.Funciones;

namespace WebAPI.Controllers.Multimedia.Imagenes;

[Route("api")]
[ApiController]
public class PortadaController : ControllerBase
{
    private TextoMyExtension _textoMyExtension = new();
    private Archivos _archivos = new();


    [HttpGet("Portadas")]
    public async Task<IActionResult> GetPortada([FromQuery] string titulo, [FromQuery] int anio)
    {
        // Sanitiza el titulo recibido
        string tituloProcesado = $"{_textoMyExtension.SanitizarString(titulo)}_{anio}";
        string imagenPath = null;

        imagenPath = _archivos.BuscarArchivoPorNombre(RutasArchivos.PORTADAS, tituloProcesado);

        if (!System.IO.File.Exists(imagenPath))
        {
            return NotFound(); // 404 si la imagen no se encuentra
        }

        var imageFileStream = new FileStream(imagenPath, FileMode.Open, FileAccess.Read);
        return File(imageFileStream, "image/jpeg"); // Devuelve la imagen con el tipo MIME adecuado
    }
}
