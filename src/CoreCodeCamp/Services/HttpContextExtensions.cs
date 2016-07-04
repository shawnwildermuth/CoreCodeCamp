using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace CoreCodeCamp.Services
{
  public static class HttpContextExtensions
  {
    public static EventInfo EventInfo(this HttpContext ctx)
    {
      return ctx.Items["Moniker"] as EventInfo;
    }
  }
}
