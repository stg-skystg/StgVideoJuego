using System.Net;

namespace Models.Entidad
{
    /// <summary>
    /// Respuesta de peticiones 
    /// </summary>
    public class Respuesta
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool BlIsExitoso { get; set; }
        public object? Resultado { get; set; }
        public string? Mensaje { get; set; }
    }
}
