using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Portfolium_Back.Extensions.Middleware;
using Portfolium_Back.Models;

namespace Portfolium_Back.Extensions.Helpers
{
    /// <summary>
    /// Helper para efetuar as ações relacionadas ao Token JWT
    /// </summary>
    public class TokenHelper
    {
        /// <summary>
        /// Gera o Token JWT daquele usuário
        /// </summary>
        /// <param name="user">O usuário a ser efetuado a autenticação</param>
        /// <returns>O Token JWT</returns>
        public static String GenerateToken(User user)
        {
            IConfiguration? appsettings = ServiceLocator.Current.BuscarServico<IConfiguration>();
            if (appsettings == null)
            {
                throw new Exception("Configurações de Token não encontradas");
            }
            JwtSecurityTokenHandler Handler = new JwtSecurityTokenHandler();
            var JwtKey = appsettings["Jwt:Key"];
            if (String.IsNullOrWhiteSpace(JwtKey))
            {
                throw new Exception("Configurações de Token não encontradas");
            }
            Byte[] key = Encoding.ASCII.GetBytes(JwtKey);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(ClaimTypes.Name, user.Name ?? String.Empty),
                        new Claim(ClaimTypes.Email, user.Email ?? String.Empty),
                        new Claim(ClaimTypes.NameIdentifier, user.GuidID.ToString()),
                        new Claim(ClaimTypes.Role, user.Role ?? String.Empty)
                    }),
                Issuer = appsettings["Jwt:Issuer"],
                Audience = appsettings["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = Handler.CreateToken(tokenDescriptor);
            return Handler.WriteToken(token);
        }

        /// <summary>
        /// Busca o ID do Usuário de dentro do Token
        /// </summary>
        /// <param name="token">Token a ter o seu ID extraído</param>
        /// <returns>o ID de Usuário</returns>
        public static Guid GetUserIdFromToken(String token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var ClaimID = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            if (!Guid.TryParse(ClaimID, out Guid GuidUsuario))
            {
                throw new UnauthorizedAccessException("Usuário não encontrado no token!");
            }
            return GuidUsuario;
        }
    }
} 