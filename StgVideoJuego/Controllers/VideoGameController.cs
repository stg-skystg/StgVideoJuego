using DataAcces;
using Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StgVideoJuego.Helper;
using System.Net;
using Models.Entidad;
using Microsoft.EntityFrameworkCore;

namespace StgVideoJuego.Controllers
{
    public class VideoGameController : BaseAPIController
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
        public VideoGameController(ApplicationDbContext PrmObjDb, IConfiguration PrmIConfiguration)
        {
            ObjDb = PrmObjDb;
            ObjFuncionesHelpers = new FuncionesHelpers(ObjDb);
            ObjAutoHelpers = new AuthHelpers(PrmIConfiguration);
        }
        /// <summary>
        /// Consulta videojuegos total
        /// </summary>
        /// <returns>Retornar listado de registros</returns>
        [Authorize]
        [HttpGet("ConsultaTotal")]
        public async Task<ActionResult<IEnumerable<VideoGame>>> GetVideoJuego()
        {
            var ObjUsuario = ObjDb.VideoGame.ToList();
            return Ok(ObjFuncionesHelpers.ObjRespuestaMetodo("Consulta de videoJuegos exitosa", ObjUsuario, HttpStatusCode.OK, true));
        }
        /// <summary>
        /// Consulta videojuegos total
        /// </summary>
        /// <returns>Retornar listado de registros</returns>
        [Authorize]
        [HttpPost("CrearVideoJuego")]
        public async Task<ActionResult<IEnumerable<VideoGame>>> PostVideoJuego([FromBody] VideoGameRegistro PrmObjVideoGame)
        {
            if (PrmObjVideoGame != null)
            {
                if (!await ObjFuncionesHelpers.BlVideoGameExiste(PrmObjVideoGame.Titulo!, PrmObjVideoGame.Compania!))
                {
                    String StrAuthorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault()!;
                    var ObjRegistroVideoGame = ObjFuncionesHelpers.ObjCambioVideo(PrmObjVideoGame, ObjAutoHelpers.LeerToken(StrAuthorizationHeader.Remove(0, 7)));
                    ObjDb.VideoGame.Add(ObjRegistroVideoGame);
                    await ObjDb.SaveChangesAsync();
                    return Ok(ObjFuncionesHelpers.ObjRespuestaMetodo("Creacion de videoJuegos exitosa", ObjRegistroVideoGame, HttpStatusCode.OK, true));
                }
                return BadRequest(ObjFuncionesHelpers.ObjRespuestaMetodo("Ya existe videogame a crear", PrmObjVideoGame, HttpStatusCode.BadRequest, false));
            }
            return BadRequest(ObjFuncionesHelpers.ObjRespuestaMetodo("No hay data para agregar videogame", null!, HttpStatusCode.BadRequest, false));
        }

        /// <summary>
        /// Consulta videojuegos total
        /// </summary>
        /// <returns>Retornar listado de registros</returns>
        [Authorize]
        [HttpPut("ModificarVideoJuego")]
        public async Task<ActionResult<IEnumerable<VideoGame>>> PutVideoJuego([FromBody] VideoGameUpdate PrmObjVideoGame)
        {
            if (PrmObjVideoGame != null)
            {
                if(await ObjDb.VideoGame.AnyAsync(x => x.ID == PrmObjVideoGame.ID))
                {
                    String StrAuthorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault()!;
                    int IntIdUsuario = ObjAutoHelpers.LeerToken(StrAuthorizationHeader.Remove(0, 7));
                    VideoGame ObjRegistroVideoGame = ObjDb.VideoGame.SingleOrDefault(x => x.ID == PrmObjVideoGame.ID)!;
                    ObjRegistroVideoGame.Titulo = PrmObjVideoGame.Titulo!;
                    ObjRegistroVideoGame.Compania = PrmObjVideoGame.Compania!;
                    ObjRegistroVideoGame.Anno = PrmObjVideoGame.Anno!;
                    ObjRegistroVideoGame.Precio = PrmObjVideoGame.Precio!;
                    ObjRegistroVideoGame.Fecha_Actualizacion = DateTime.Now!;
                    ObjRegistroVideoGame.IDUsuario = ObjAutoHelpers.LeerToken(StrAuthorizationHeader.Remove(0, 7))!;
                    await ObjDb.SaveChangesAsync();
                    return Ok(ObjFuncionesHelpers.ObjRespuestaMetodo("Actualizacion de videoJuegos exitosa", ObjRegistroVideoGame, HttpStatusCode.OK, true));
                }
                return BadRequest(ObjFuncionesHelpers.ObjRespuestaMetodo("No se encontro videogame a modificar", PrmObjVideoGame, HttpStatusCode.BadRequest, false));
            }
            return BadRequest(ObjFuncionesHelpers.ObjRespuestaMetodo("No hay data para modificar videogame", null!, HttpStatusCode.BadRequest, false));
        }

        /// <summary>
        /// borrar videojuegos total
        /// </summary>
        /// <returns>Retornar listado de registros</returns>
        [Authorize]
        [HttpDelete("BorrarVideoJuego")]
        public async Task<ActionResult<IEnumerable<VideoGame>>> DeleteVideoJuego(int PrmIntID)
        {
            var ObjVideoGame = await ObjDb.VideoGame.Where(x => x.ID == PrmIntID).FirstOrDefaultAsync();
            if (ObjVideoGame != null)
            {
                ObjDb.Remove(ObjVideoGame);
                ObjDb.SaveChanges();
                return Ok(ObjFuncionesHelpers.ObjRespuestaMetodo("Eliminado el videogame exitosa", null! , HttpStatusCode.OK, true));
            }
            return BadRequest(ObjFuncionesHelpers.ObjRespuestaMetodo("No se encontro videogame a modificar", null!, HttpStatusCode.BadRequest, false));
        }
    }
}
