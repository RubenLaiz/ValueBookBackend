using System.Text.RegularExpressions;

namespace WebAPI.Funciones;

public class TextoMyExtension
{
    public string SanitizarString(string text)
    {
        // Sanitizar el título: convertir a minúsculas y reemplazar caracteres no alfanuméricos por guiones
        var textoSanitizado = Regex.Replace(text.ToLower(), @"[^a-z0-9]+", "-");

        // Eliminar guiones adicionales al principio y al final
        textoSanitizado = textoSanitizado.Trim('-');

        return textoSanitizado;
    }
}
