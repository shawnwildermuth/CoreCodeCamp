using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Data
{
  public class CodeCampContextFactory : IDesignTimeDbContextFactory<CodeCampContext>
  {
    public CodeCampContext CreateDbContext(string[] args)
    {
      // Create a configuration 
      var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddJsonFile("appsettings.debug.json", true)
        .AddEnvironmentVariables()
        .Build();

      return new CodeCampContext(config, new DbContextOptionsBuilder<CodeCampContext>().Options);
    }
  }
}
