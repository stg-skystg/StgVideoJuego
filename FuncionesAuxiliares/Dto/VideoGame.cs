namespace Models.Dto
{
    /// <summary>
    /// Tabla videogame
    /// </summary>
    public class VideoGame
    {
        public int ID   { get; set; }
        public string? Titulo { get; set; }
        public string? Compania { get; set; }
        public Int16 Anno { get; set; }
        public decimal Precio { get; set; }
        public decimal Puntaje { get; set; }
        public DateTime Fecha_Actualizacion { get; set; }
        public int IDUsuario { get; set; }

    }
}
