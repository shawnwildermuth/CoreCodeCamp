using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Authentication
{
  public static class CoreCampAuthSchemes
  {
    public static readonly string AnyAuthScheme = string.Join(",", 
      CookieAuthenticationDefaults.AuthenticationScheme, 
      JwtBearerDefaults.AuthenticationScheme);
  }
}
