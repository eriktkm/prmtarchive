using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PrimataArchive.Api.Models;

namespace PrimataArchive.Api.Services;

public class TokenService
{
    private readonly string _secret = "SuaChaveSecretaSuperSeguraComPeloMenos32Caracteres!";

    public string GenerateToken(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email ?? ""),
                new Claim(ClaimTypes.Name, usuario.Nome ?? "")
            }),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}