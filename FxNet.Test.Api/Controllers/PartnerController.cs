using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FxNet.Test.Contracts;
using FxNet.Test.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FxNet.Test.Api.Controllers;

[ApiController]
public class PartnerController : ControllerBase
{
    private readonly IConfiguration _config;

    public PartnerController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("/api.user.partner.rememberMe")]
    public ActionResult<TokenInfo> RememberMe([FromQuery] string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new SecureValidationException("Code is required");

        var jwtSection = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, code),
            new Claim("code", code)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new TokenInfo { Token = tokenString };
    }
}
