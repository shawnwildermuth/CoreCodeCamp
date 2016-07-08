using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Services;
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
      await _ctx.Database.EnsureCreatedAsync();

      var admin = await _userManager.FindByEmailAsync(_config["Admin:SuperUser:Email"]);

      // If no Admin, then we haven't seeded the database
      if (admin == null)
      {
        admin = new CodeCampUser()
        {
          UserName = _config["Admin:SuperUser:Email"],
          Email = _config["Admin:SuperUser:Email"],
          Name = _config["Admin:SuperUser:Name"],
          EmailConfirmed = true
        };

        // Create Super User
        if (!(await _userManager.CreateAsync(admin, _config["Admin:SuperUser:TempPassword"])).Succeeded)
        {
          throw new InvalidOperationException("Failed to create Super User");
        }

        if (!(await _roleManager.CreateAsync(new IdentityRole(Consts.AdminRole))).Succeeded)
        {
          throw new InvalidOperationException("Failed to create Admin Role");
        }

        // Add to Admin Role
        if (!(await _userManager.AddToRoleAsync(admin, Consts.AdminRole)).Succeeded)
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
            TwitterLink = "https://twitter.com/atlcodecamp",
            ContactEmail = "codecamp@live.com",
            Location = new EventLocation()
            {
              Facility = "Kennesaw State University (Formerly Southern Polytechnic)",
              Address1 = "1100 S Marietta Pkwy",
              Address2 = "",
              City = "Marietta",
              StateProvince = "GA",
              PostalCode = "30060",
              Country = "USA",
              Link = ""
            }
          },
          new EventInfo()
          {
            Moniker = "2015",
            Name = "Atlanta Code Camp 2015",
            EventDate = new DateTime(2015, 10, 24),
            EventLength = 1,
            Description = "The Atlanta Code Camp is awesome",
            IsDefault = false,
            Location = new EventLocation()
            {
              Facility = "Kennesaw State University (Formerly Southern Polytechnic)",
              Address1 = "1100 S Marietta Pkwy",
              Address2 = "",
              City = "Marietta",
              StateProvince = "GA",
              PostalCode = "30060",
              Country = "USA",
              Link = ""
            }
          }
        };

        var sponsor = new Sponsor()
        {
          Name = "Wilder Minds",
          Link = "http://wilderminds.com",
          Event = codeCamps[0],
          Paid = true,
          ImageUrl = "/img/2016/sponsors/wilder-minds.jpg",
          SponsorLevel = "Silver"
        };

        _ctx.AddRange(codeCamps);
        _ctx.Add(sponsor);
        await _ctx.SaveChangesAsync();
      }
    }
  }
}
