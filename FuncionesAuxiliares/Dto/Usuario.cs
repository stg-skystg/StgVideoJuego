namespace Models.Dto
{
    /// <summary>
    /// Dto datos usuario
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// valor id del registro
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Apodo de jugador
        /// </summary>
        public string? NickName { get; set; }
        /// <summary>
        /// Contraseña del usaurio
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// Email del usuario
        /// </summary>
        public string? Email { get; set; }


    }
}
