﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

      var env = host.Services.GetService<IHostingEnvironment>();
      if (env.IsDevelopment())
      {
        IConfiguration config = host.Services.GetService<IConfiguration>();
        var shouldSeed = config.GetValue<bool>("Data:SeedDatabase");
        if ( shouldSeed)
        {
          Seed(host).Wait();
        }
      }

      host.Run();
    }

    private static async Task Seed(IWebHost host)
    {
      IServiceScopeFactory scopeFactory = host.Services.GetService<IServiceScopeFactory>();
      using (var scope = scopeFactory.CreateScope())
      {
        var initializer = scope.ServiceProvider.GetService<CodeCampSeeder>();
        await initializer.SeedAsync();
      }
    }
  }
}
