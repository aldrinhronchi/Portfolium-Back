using Microsoft.IdentityModel.Tokens;
using Portfolium_Back.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Portfolium_Back.Services
{
    public class TokenService
    {
        public static IConfiguration _configuration;

        public TokenService(IConfiguration config)
        {
            _configuration = config;
        }

        public static string GenerateToken(User user)
        {
            JwtSecurityTokenHandler? tokenHandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey? key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
            Claim[] claims = new[] {
                                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                                        new Claim(ClaimTypes.Name, user.Name),
                                        new Claim(ClaimTypes.Email, user.Email),
                                        new Claim(ClaimTypes.NameIdentifier, user.ID.ToString())
                                    };
            SigningCredentials? signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            SecurityTokenDescriptor? tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = signIn
            };
            SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}