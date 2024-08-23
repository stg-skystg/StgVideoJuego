
using Models.Dto;

namespace Models.Entidad
{
    public class UsuarioLogin
    {
        /// <summary>
        /// Email del usuario
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Contraseña del usaurio
        /// </summary>
        public string? Password { get; set; }
    }
}
