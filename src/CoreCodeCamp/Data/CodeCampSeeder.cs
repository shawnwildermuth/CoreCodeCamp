using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Services;
using CsvHelper;
using Microsoft.AspNetCore.Hosting;
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
    private IHostingEnvironment _env;

    public CodeCampSeeder(CodeCampContext ctx, 
      UserManager<CodeCampUser> userManager, 
      RoleManager<IdentityRole> roleManager, 
      IConfigurationRoot config,
      IHostingEnvironment env)
    {
      _env = env;
      _ctx = ctx;
      _userManager = userManager;
      _config = config;
      _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
      try
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
          },
          new EventInfo()
          {
            Moniker = "2014",
            Name = "Atlanta Code Camp 2014",
            EventDate = new DateTime(2014, 10, 24),
            EventLength = 1,
            Description = "The Atlanta Code Camp is awesome",
            IsDefault = false,
            Location = new EventLocation()
            {
              Facility = "Southern Polytechnic",
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
            Moniker = "2013",
            Name = "Atlanta Code Camp 2013",
            EventDate = new DateTime(2013, 10, 24),
            EventLength = 1,
            Description = "The Atlanta Code Camp is awesome",
            IsDefault = false,
            Location = new EventLocation()
            {
              Facility = "Southern Polytechnic",
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

          await Migrate();
        }
      }
      catch (Exception ex)
      {
        // TODO
        ex.ToString();
      }
    }

    private async Task Migrate()
    {
      var speakerFile = string.Concat(_env.ContentRootPath, @"\Database\speakers.csv");
      if (File.Exists(speakerFile))
      {
        var oldSpeakers = new CsvReader(File.OpenText(speakerFile));

        while (oldSpeakers.Read())
        {
          _ctx.Add(MigrateSpeaker(oldSpeakers));
        }
      }

      await _ctx.SaveChangesAsync();
    }

    private Speaker MigrateSpeaker(CsvReader oldSpeakers)
    {
      var moniker = oldSpeakers.GetField("Year");
      var camp = _ctx.CodeCampEvents.Where(c => c.Moniker == moniker).First();
      return new Speaker()
      {
        Event = camp,
        Bio = oldSpeakers.GetField("Bio"),
        Blog = oldSpeakers.GetField("Blog"),
        CompanyName = oldSpeakers.GetField("CompanyName"),
        CompanyUrl = oldSpeakers.GetField("CompanyUrl"),
        ImageUrl = oldSpeakers.GetField("ImageUrl"),
        Name = oldSpeakers.GetField("Name"),
        Title = oldSpeakers.GetField("Title"),
        Twitter = oldSpeakers.GetField("Twitter"),
        UserName = "",
        Website = "",
        Talks = MigrateTalks(Int32.Parse(oldSpeakers.GetField("Id")))
      };
    }

    private ICollection<Talk> MigrateTalks(int speakerId)
    {
      var result = new List<Talk>();

      var talkFile = string.Concat(_env.ContentRootPath, @"\Database\talks.csv");
      if (File.Exists(talkFile))
      {
        var oldTalks = new CsvReader(File.OpenText(talkFile));
        while (oldTalks.Read())
        {
          if (Int32.Parse(oldTalks.GetField("Speaker_Id")) == speakerId)
          {
            result.Add(MigrateTalk(oldTalks));
          }
        }
      }

      return result;
    }

    private Talk MigrateTalk(CsvReader oldTalks)
    {
      var talk = new Talk()
      {
        Abstract = oldTalks.GetField("Abstract"),
        Approved = true,
        Audience = oldTalks.GetField("Audience"),
        CodeUrl = oldTalks.GetField("CodeUrl"),
        Level = Int32.Parse(oldTalks.GetField("Level")),
        Prerequisites = oldTalks.GetField("Prerequisites"),
        PresentationUrl = oldTalks.GetField("PresentationUrl"),
        SpeakerDeckUrl = oldTalks.GetField("SpeakerDeckUrl"),
        SpeakerRateUrl = oldTalks.GetField("SpeakerRateUrl"),
        Title = oldTalks.GetField("Title")
      };

      // Add Categories

      _ctx.Add(talk);

      return talk;
    }
  }
}
