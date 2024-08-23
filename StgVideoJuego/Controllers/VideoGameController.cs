using DataAcces;
using Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StgVideoJuego.Helper;
using System.Net;
using Models.Entidad;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Win32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StgVideoJuego.Controllers
{
    /// <summary>
    /// controllador de videogame
    /// </summary>
    public class VideoGameController : BaseAPIController
    {
        /// <summary>
        /// Configuracion del ambiente
        /// </summary>
        private readonly IConfiguration Objconfiguration;
        /// <summary>
        /// Memoria en cache
        /// </summary>
        private readonly IMemoryCache InfMemoryCache;
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
        public VideoGameController(ApplicationDbContext PrmObjDb, IConfiguration PrmIConfiguration, IMemoryCache PrmInfMemoryCache)
        {
            ObjDb = PrmObjDb;
            Objconfiguration = PrmIConfiguration;
            ObjFuncionesHelpers = new FuncionesHelpers(ObjDb);
            ObjAutoHelpers = new AuthHelpers(PrmIConfiguration);
            InfMemoryCache = PrmInfMemoryCache;
        }
        /// <summary>
        /// Consulta videojuegos total con guardado en redis
        /// </summary>
        /// <returns>Retornar listado de registros</returns>
        [Authorize]
        [HttpGet("ConsultaTotal")]
        public async Task<ActionResult<IEnumerable<VideoGame>>> GetVideoJuego()
        {
            List<VideoGame> ObjVideoGame;

            string StrCacheKey = Objconfiguration["ApplicationSettings:Cache_Key"]!;

            if (!InfMemoryCache.TryGetValue(StrCacheKey, out ObjVideoGame!))
            {
                ObjVideoGame = await ObjDb.VideoGame.ToListAsync();
                var ObjCacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));

                InfMemoryCache.Set(StrCacheKey, ObjVideoGame, ObjCacheOptions);
            }


            return Ok(ObjFuncionesHelpers.ObjRespuestaMetodo("Consulta de videoJuegos exitosa", ObjVideoGame, HttpStatusCode.OK, true));
        }
        /// <summary>
        /// Consulta videojuegos pagina filtro
        /// </summary>
        /// <returns>Retornar listado de registros</returns>
        [Authorize]
        [HttpGet("ConsultaPagina")]
        public async Task<ActionResult<IEnumerable<VideoGame>>> GetVideoJuego(int PrmIntPagina = 1, int PrmIntTamanoPagina = 5, string PrmStrNombre = null!, string PrmStrCompania = null!, int? PrmIntanno = null )
        {
            List<VideoGame> ObjVideoGame = await ObjDb.VideoGame.ToListAsync();

            if (!string.IsNullOrEmpty(PrmStrNombre))
            {
                ObjVideoGame = ObjVideoGame.Where(v => v.Titulo!.Contains(PrmStrNombre)).ToList();
            }

            if (!string.IsNullOrEmpty(PrmStrCompania))
            {
                ObjVideoGame = ObjVideoGame.Where(v => v.Compania!.Contains(PrmStrCompania)).ToList();
            }

            if (PrmIntanno.HasValue)
            {
                ObjVideoGame = ObjVideoGame.Where(v => v.Anno == PrmIntanno.Value).ToList();
            }

            var ObjVideoGamePaginados = ObjVideoGame
                .Skip((PrmIntPagina - 1) * PrmIntTamanoPagina)
                .Take(PrmIntTamanoPagina)
                .ToList();
            if(ObjVideoGamePaginados.Count > 0)
                return Ok(ObjFuncionesHelpers.ObjRespuestaMetodo("Consulta de videoJuegos exitosa", ObjVideoGamePaginados, HttpStatusCode.OK, true));
            return BadRequest(ObjFuncionesHelpers.ObjRespuestaMetodo("Paginacion excedida o sin registros", null!, HttpStatusCode.BadRequest, false));

        }
        /// <summary>
        /// Consulta videojuegos total
        /// </summary>
        /// <returns>Retornar listado de registros</returns>
        [Authorize]
        [HttpGet("ConsultaID")]
        public async Task<ActionResult<IEnumerable<VideoGame>>> GetVideoJuego(int PrmIntID)
        {
            var ObjVideoGame = await ObjDb.VideoGame.Where(x => x.ID == PrmIntID).FirstOrDefaultAsync();
            if(ObjVideoGame != null)
            {
                return Ok(ObjFuncionesHelpers.ObjRespuestaMetodo("Consulta de videoJuegos exitosa", ObjVideoGame, HttpStatusCode.OK, true));
            }
            return NotFound(ObjFuncionesHelpers.ObjRespuestaMetodo("No se encontro videogame", null! , HttpStatusCode.NotFound, false));
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
                    string StrAuthorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault()!;
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
                    string StrAuthorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault()!;
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
