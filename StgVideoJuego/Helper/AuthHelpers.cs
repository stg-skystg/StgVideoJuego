using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Models.Dto;
using System.Security.Claims;
using System.Text;
using Models.Entidad;

namespace StgVideoJuego.Helper
{
    public class AuthHelpers
    {
        /// <summary>
        /// Configuracion del ambiente
        /// </summary>
        private readonly IConfiguration Objconfiguration;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="PrmIConfiguration"></param>
        public AuthHelpers(IConfiguration PrmIConfiguration)
        {
            Objconfiguration = PrmIConfiguration;
        }
        /// <summary>
        /// Generar token
        /// </summary>
        /// <param name="PrmObjUser">Valor de usuario a encantar</param>
        /// <returns>string de token</returns>
        public string GenerateJWTToken(Usuario PrmObjUser)
        {
            var prueba = Objconfiguration["ConnectionStrings:JWT_Secret"]!;
            var claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, PrmObjUser.ID.ToString()),
            new Claim(ClaimTypes.Name, PrmObjUser.Email!),};
            var jwtToken = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(Objconfiguration["ApplicationSettings:JWT_Secret"]!)
                        ),
                    SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
        /// <summary>
        /// Leer token extraer usuario
        /// </summary>
        /// <param name="PrmStrToken">valor del token</param>
        public int LeerToken(string PrmStrToken)
        {
            // Obtener la clave secreta utilizada para generar el token
            var StrSecretKey = Objconfiguration["ApplicationSettings:JWT_Secret"];
            var Strkey = Encoding.UTF8.GetBytes(StrSecretKey!);

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Strkey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(PrmStrToken, validationParameters, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                return Int32.Parse(userIdClaim!);
            }
            catch
            {
                return 0;
            }
        }
    }
}
