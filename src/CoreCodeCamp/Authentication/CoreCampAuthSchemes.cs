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
    // HACK
    public const string AnyAuthScheme = "Identity.Application," + JwtBearerDefaults.AuthenticationScheme;
  }
}
