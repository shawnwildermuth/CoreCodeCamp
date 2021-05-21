using System.Threading.Tasks;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
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

      if (args?.Length == 1 && args[0].ToLower() == "/seed")
      {
        Seed(host).Wait();

      }
      else
      {
        host.Run();
      }
    }

    private static async Task Seed(IWebHost host)
    {
      var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
      using (var scope = scopeFactory.CreateScope())
      {
        var initializer = scope.ServiceProvider.GetService<CodeCampSeeder>();
        await initializer.SeedAsync();
      }
    }
  }
}
