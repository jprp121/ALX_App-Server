using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Dtos;
using API.Entities;
using API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public string CreateToken(UserDto user)
    {
        var tokenKey = configuration["tokenKey"] ?? throw new Exception("Cannot find tokenKey");

        if(tokenKey.Length < 64) throw new Exception("Token key is not long enough");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        var claims = new List<Claim>{
            new (ClaimTypes.NameIdentifier, user.UserName) 
        };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokendescriptor = new SecurityTokenDescriptor{
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };

        var tokenhandler = new JwtSecurityTokenHandler();

        var token = tokenhandler.CreateToken(tokendescriptor);

        return tokenhandler.WriteToken(token);
    }
}
