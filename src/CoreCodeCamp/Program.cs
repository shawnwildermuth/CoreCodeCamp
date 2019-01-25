using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoreCodeCamp
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var host = WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .Build();

      host.Run();
    }

  }
}
