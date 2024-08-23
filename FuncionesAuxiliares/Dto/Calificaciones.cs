namespace Models.Dto
{
    /// <summary>
    /// Tabla calificacion
    /// </summary>
    public class Calificaciones
    {
        public int ID { get; set; }
        public int IDUsuario { get; set; }
        public int IDVideoJuego { get; set; }
        public decimal Puntaje { get; set; }

    }
}
