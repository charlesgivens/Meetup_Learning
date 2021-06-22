using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.IO;
using MeetupApi.Identity;
using MeetupAPI.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using MeetupAPI.Identity;

namespace MeetupApi.Provider
{
  public class JwtProvider : IJwtProvider
  {
    private readonly JwtOptions _jwtOptions;

    public JwtProvider(JwtOptions jwtOptions)
    {
      _jwtOptions = jwtOptions;
    }
    public string GenerateJwtToken(User user)
    {
      var claims = new List<Claim>()
      {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Role, user.Role.RoleName),
        new Claim(ClaimTypes.Name, user.Email),
        new Claim("FirstName", user.FirstName),
        new Claim("LastName", user.LastName)
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.JwtKey));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var expires = DateTime.Now.AddDays(_jwtOptions.JwtExpireDays);

      var token = new JwtSecurityToken(
        _jwtOptions.JwtIssuer,
        _jwtOptions.JwtIssuer,
        claims,
        expires: expires,
        signingCredentials: creds
      );

      var tokenHandler = new JwtSecurityTokenHandler();

      return tokenHandler.WriteToken(token);
    }
  }
}