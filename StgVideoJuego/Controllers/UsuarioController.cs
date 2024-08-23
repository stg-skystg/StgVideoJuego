using Microsoft.AspNetCore.Mvc;
using StgVideoJuego.Helper;
using DataAcces;
using System.Net;
using Models.Entidad;
using Models.Dto;

namespace StgVideoJuego.Controllers
{
    /// <summary>
    /// Controlador de usuario
    /// </summary>
    public class UsuarioController : BaseAPIController
    {
        /// <summary>
        /// Funciones para el apoyo de los procesos
        /// </summary>
        private readonly FuncionesHelpers ObjFuncionesHelpers;
        /// <summary>
        /// Conexion base de datos
        /// </summary>
        private readonly ApplicationDbContext ObjDb;
        /// <summary>
        /// funciones de ayuda con la autentificacion
        /// </summary>
        private readonly AuthHelpers ObjAutoHelpers;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="PrmObjDb">conexion base de datos</param>
        /// <param name="PrmIConfiguration">configuracion del ambiente</param>
        public UsuarioController(ApplicationDbContext PrmObjDb, IConfiguration PrmIConfiguration)
        {
            ObjAutoHelpers = new AuthHelpers(PrmIConfiguration);
            ObjDb = PrmObjDb;
            ObjFuncionesHelpers = new FuncionesHelpers(ObjDb);
        }
        /// <summary>
        /// Proceso post para crear usuario
        /// </summary>
        /// <param name="PrmObjUsuario">Objeto para la creacion de usuario</param>
        /// <returns>Mensaje ok caso correcto badrequest caso de fallo</returns>
        [HttpPost("Registrar")]
        public async Task<ActionResult<UsuarioRegistro>> Registro([FromBody] UsuarioRegistro PrmObjUsuario)
        {
            if (await ObjFuncionesHelpers.BlUsuarioExiste(PrmObjUsuario.Email! , PrmObjUsuario.NickName!)) 
                return BadRequest(ObjFuncionesHelpers.ObjRespuestaMetodo("Mail o nickname ya existentes", null!, HttpStatusCode.BadRequest, false));

            ObjDb.Usuario.Add(ObjFuncionesHelpers.ObjCambioUsuario(PrmObjUsuario));
            await ObjDb.SaveChangesAsync();
            return Ok( ObjFuncionesHelpers.ObjRespuestaMetodo("Usuario registrado", PrmObjUsuario, HttpStatusCode.OK, true));
        }
        /// <summary>
        /// Proceso de login para creacion de token
        /// </summary>
        /// <param name="PrmObjUsuario">Objeto de usuario con los campos para login</param>
        /// <returns>Mensaje ok caso correcto badrequest caso de fallo</returns>
        [HttpPost("Login")]
        public async Task<ActionResult<UsuarioRegistro>> Login([FromBody] UsuarioLogin PrmObjUsuario)
        {
            if (await ObjFuncionesHelpers.BlUsuarioExisteLogin(PrmObjUsuario.Email!, PrmObjUsuario.Password!))
            {
                
                return Ok(ObjFuncionesHelpers.ObjRespuestaMetodo("Token generado", new { Duracion = 3600, Token = ObjAutoHelpers.GenerateJWTToken(ObjDb.Usuario.Where(x => x.Email == PrmObjUsuario.Email || x.Password == PrmObjUsuario.Password).FirstOrDefault()!)}, HttpStatusCode.OK, true));
            }
            return BadRequest(ObjFuncionesHelpers.ObjRespuestaMetodo("Usuario no valido", null!, HttpStatusCode.BadRequest, false));
            
        }
    }
}
