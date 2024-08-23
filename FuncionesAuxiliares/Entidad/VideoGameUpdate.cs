using Models.Dto;

namespace Models.Entidad
{
    /// <summary>
    /// Clase de registro de videogame para pasar a tabla de videogame
    /// </summary>
    public class VideoGameUpdate
    {
        public int ID { get; set; }
        public string? Titulo { get; set; }
        public string? Compania { get; set; }
        public Int16 Anno { get; set; }
        public decimal Precio { get; set; }
        
    }
}
