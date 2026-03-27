using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Melanin.API.Token;

public class TokenTool
{
    private readonly IConfiguration _config;

    public TokenTool(IConfiguration config)
    {
        _config = config;
    }

    public class Data
    {
        public required int MemberId { get; set; }
        public required string Role { get; set; }
    }

    public string Generate(Data data)
    {
        Claim[] claims = [
            new Claim(ClaimTypes.NameIdentifier, data.MemberId.ToString()),
            new Claim(ClaimTypes.Role, data.Role)
        ];

        string secret = _config["Token:Key"]
            ?? throw new Exception("Clef du token non défini dans la config !");

        byte[] key = Encoding.UTF8.GetBytes(secret);
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
        SigningCredentials signingCredentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha512
        );

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _config["Token:Issuer"],
            audience: _config["Token:Audience"],
            expires: DateTime.UtcNow.AddMinutes(
                _config.GetValue<int>("Token:Expire")
            ),
            claims: claims,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}