using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models;
using CoreCodeCamp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Authentication
{
  public class CoreCodeCampTokenFactory
  {
    private readonly IConfiguration _config;
    private readonly UserManager<CodeCampUser> _userManager;
    private CoreCodeCampTokenOptions _tokenOptions = new CoreCodeCampTokenOptions();

    public CoreCodeCampTokenFactory(IConfiguration config, UserManager<CodeCampUser> userManager)
    {
      _config = config;
      _userManager = userManager;
    }

    public async Task<TokenModel> GenerateForUser(CodeCampUser user, string optionsKey = "TokenOptions")
    {
      _config.Bind(optionsKey, _tokenOptions);

      // Create the token
      var claims = new List<Claim>()
      {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
      };

      var userClaims = await _userManager.GetClaimsAsync(user);

      claims.AddRange(userClaims);

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SigningKey));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

      var token = new JwtSecurityToken(
        _tokenOptions.Issuer,
        _tokenOptions.Audience,
        claims,
        expires: DateTime.Now.AddMinutes(_tokenOptions.ExpirationLength),
        signingCredentials: creds);

      return new TokenModel()
      {
        Token = new JwtSecurityTokenHandler().WriteToken(token),
        Expiration = token.ValidTo
      };
    }
  }
}
