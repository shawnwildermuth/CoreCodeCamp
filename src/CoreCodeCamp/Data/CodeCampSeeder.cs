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
    }
  }
}
