using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoreCodeCamp.Data
{
  public class CodeCampSeeder
  {
    private IConfigurationRoot _config;
    private CodeCampContext _ctx;
    private RoleManager<IdentityRole> _roleManager;
    private UserManager<CodeCampUser> _userManager;

    public CodeCampSeeder(CodeCampContext ctx, UserManager<CodeCampUser> userManager, RoleManager<IdentityRole> roleManager, IConfigurationRoot config)
    {
      _ctx = ctx;
      _userManager = userManager;
      _config = config;
      _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
      await _ctx.Database.MigrateAsync();

      var admin = await _userManager.FindByEmailAsync(_config["Admin:SuperUser:Email"]);

      // If no Admin, then we haven't seeded the database
      if (admin == null)
      {
        admin = new CodeCampUser()
        {
          UserName = _config["Admin:SuperUser:Email"],
          Email = _config["Admin:SuperUser:Email"],
          EmailConfirmed = true
        };

        // Create Super User
        if (!(await _userManager.CreateAsync(admin, _config["Admin:SuperUser:TempPassword"])).Succeeded)
        {
          throw new InvalidOperationException("Failed to create Super User");
        }

        if (!(await _roleManager.CreateAsync(new IdentityRole("Admin"))).Succeeded)
        {
          throw new InvalidOperationException("Failed to create Admin Role");
        }

        // Add to Admin Role
        if (!(await _userManager.AddToRoleAsync(admin, "Admin")).Succeeded)
        {
          throw new InvalidOperationException("Failed to update Super User Role");
        }
      }

      if (!_ctx.CodeCampEvents.Any())
      {
        var codeCamps = new EventInfo[] {
          new EventInfo()
          {
            Moniker = "2016",
            Name = "Atlanta Code Camp 2016",
            EventDate = new DateTime(2016, 10, 15),
            EventLength = 1,
            Description = "The Atlanta Code Camp is awesome",
            IsDefault = true,
            Location = new EventLocation()
            {
              Facility = "TDB",
              Address1 = "123 Main Street",
              Address2 = "First Floor",
              City = "Atlanta",
              StateProvince = "GA",
              PostalCode = "30307",
              Country = "USA",
              Link = ""
            }
          },
          new EventInfo()
          {
            Moniker = "2015",
            Name = "Atlanta Code Camp 2015",
            EventDate = new DateTime(2015, 10, 12),
            EventLength = 1,
            Description = "The Atlanta Code Camp is awesome",
            IsDefault = false,
            Location = new EventLocation()
            {
              Facility = "TDB",
              Address1 = "123 Main Street",
              Address2 = "First Floor",
              City = "Atlanta",
              StateProvince = "GA",
              PostalCode = "30307",
              Country = "USA",
              Link = ""
            }
          }
        };

        _ctx.AddRange(codeCamps);

        await _ctx.SaveChangesAsync();
      }
    }
  }
}
