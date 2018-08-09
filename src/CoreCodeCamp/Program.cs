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

namespace CoreCodeCamp
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var host = WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>()
                   .Build();

      //Seed(host).Wait();

      host.Run();
    }

    private static async Task Seed(IWebHost host)
    {
      IConfiguration config = host.Services.GetService<IConfiguration>();
      IServiceScopeFactory scopeFactory = host.Services.GetService<IServiceScopeFactory>();
      using (var scope = scopeFactory.CreateScope())
      {
        var initializer = scope.ServiceProvider.GetService<CodeCampSeeder>();
        await initializer.SeedAsync();
      }
    }
  }
}
