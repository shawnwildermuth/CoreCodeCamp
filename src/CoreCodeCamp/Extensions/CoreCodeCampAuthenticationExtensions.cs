using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Authentication;

namespace CoreCodeCamp.Services
{
  public static class CoreCodeCampAuthenticationExtensions
  {
    public static AuthenticationBuilder AddCoreCodeCampBearerToken(this AuthenticationBuilder bldr, string key = "TokenOptions")
    {
      var config = bldr.Services.BuildServiceProvider().GetService<IConfiguration>();

      var options = new CoreCodeCampTokenOptions();
      config.Bind(key, options);

      return bldr.AddJwtBearer(cfg =>
      {
        cfg.TokenValidationParameters = new TokenValidationParameters()
        {
          ValidIssuer = options.Issuer,
          ValidAudience = options.Audience,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey)),
        };

      });
    }

    public static AuthenticationBuilder AddCoreCodeCampBearerToken(this AuthenticationBuilder bldr, Action<JwtBearerOptions> config)
    {
      return bldr.AddJwtBearer(config);
    }
  }
}
