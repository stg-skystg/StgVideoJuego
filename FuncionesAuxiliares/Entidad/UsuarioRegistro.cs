
using Models.Dto;

namespace Models.Entidad
{
    public class UsuarioRegistro
    {
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
