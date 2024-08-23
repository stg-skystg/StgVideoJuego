using DataAcces;
using Microsoft.EntityFrameworkCore;
using Models.Dto;
using Models.Entidad;
using System.Net;

namespace StgVideoJuego.Helper
{
    public class FuncionesHelpers
    {
        /// <summary>
        /// Parametro conexion base de datos
        /// </summary>
        private readonly ApplicationDbContext ObjDb;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="PrmObjDb">conexion base de datos</param>
        public FuncionesHelpers(ApplicationDbContext PrmObjDb)
        {
            ObjDb = PrmObjDb;
        }
        /// <summary>
        /// Validacion de existencia
        /// </summary>
        /// <param name="PrmStrTitulo"></param>
        /// <param name="PrmStrCompania"></param>
        /// <returns>bool segun la existencia</returns>
        public async Task<bool> BlVideoGameExiste(string PrmStrTitulo, string PrmStrCompania)
        {
            return await ObjDb.VideoGame.AnyAsync(x => x.Titulo == PrmStrTitulo || x.Compania == PrmStrCompania);
        }
        /// <summary>
        /// Funcion validar de existencia usuario
        /// </summary>
        /// <param name="PrmStrEmail">valor email a comparar</param>
        /// <param name="PrmStrNickName">valor nick name a comparar</param>
        /// <returns>si existe registro true si no false</returns>
        public async Task<bool> BlUsuarioExiste(string PrmStrEmail, string PrmStrNickName)
        {
            return await ObjDb.Usuario.AnyAsync(x => x.Email == PrmStrEmail || x.NickName == PrmStrNickName);
        }
        /// <summary>
        /// Valida la existencia de un usuario con los valores de login
        /// </summary>
        /// <param name="PrmStrEmail">correo</param>
        /// <param name="PrmStrPassword"> contraseña</param>
        /// <returns>si existe registro true si no false</returns>
        public async Task<bool> BlUsuarioExisteLogin(string PrmStrEmail, string PrmStrPassword)
        {
            return await ObjDb.Usuario.AnyAsync(x => x.Email == PrmStrEmail && x.Password == PrmStrPassword);
        }
        public Respuesta ObjRespuestaMetodo(string PrmStrMensaje, Object PrmObjRespuesta, HttpStatusCode PrmObjStatusCode, bool PrmBlEsExitoso = true)
        {
            Respuesta ObjRespuestaDto = new Respuesta
            {
                Mensaje = PrmStrMensaje,
                Resultado = PrmObjRespuesta,
                BlIsExitoso = PrmBlEsExitoso,
                StatusCode = PrmObjStatusCode
            };
            return ObjRespuestaDto;
        }
        /// <summary>
        /// Cambio de un objeto para registro a la tabla de videogame
        /// </summary>
        /// <param name="PrmObjCambio">Objeto a transformar</param>
        /// <param name="PrmIntIDUsuario">ID de usuario</param>
        /// <returns>Objeto transformado</returns>
        public VideoGame ObjCambioVideo(VideoGameRegistro PrmObjCambio, int PrmIntIDUsuario)
        {
            var ObjRegistroVideoGame = new VideoGame()
            {
                Titulo = PrmObjCambio.Titulo!,
                Compania = PrmObjCambio.Compania!,
                Anno = PrmObjCambio.Anno,
                Precio = PrmObjCambio.Precio,
                Fecha_Actualizacion = DateTime.Now,
                IDUsuario = PrmIntIDUsuario,
            };
            return ObjRegistroVideoGame;
        }
        /// <summary>
        /// Cambio de un objeto para registro a la tabla de Usuario
        /// </summary>
        /// <param name="PrmObjCambio">Objeto a transformar</param>
        /// <returns>Objeto transformado</returns>
        public Usuario ObjCambioUsuario(UsuarioRegistro PrmObjCambio)
        {
            var ObjRegistroUsuario = new Usuario()
            {
                NickName = PrmObjCambio.NickName,
                Password = PrmObjCambio.Password,
                Email = PrmObjCambio.Email,
            };
            return ObjRegistroUsuario;
        }
    }
}
