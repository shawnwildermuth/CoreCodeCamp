using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Services;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CoreCodeCamp.Data
{
  public class CodeCampSeeder
  {
    private IConfiguration _config;
    private CodeCampContext _ctx;
    private RoleManager<IdentityRole> _roleManager;
    private UserManager<CodeCampUser> _userManager;
    private IWebHostEnvironment _env;

    public CodeCampSeeder(CodeCampContext ctx,
      UserManager<CodeCampUser> userManager,
      RoleManager<IdentityRole> roleManager,
      IConfiguration config,
      IWebHostEnvironment env)
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
        if (_env.IsDevelopment())
        {
          if (_config.GetValue<bool>("Data:IterateDatabase"))
          {
            await _ctx.Database.EnsureDeletedAsync();
            await _ctx.Database.EnsureCreatedAsync();
          }

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

            if (!(await _roleManager.CreateAsync(new IdentityRole(Consts.ADMINROLE))).Succeeded)
            {
              throw new InvalidOperationException("Failed to create Admin Role");
            }

            // Add to Admin Role
            if (!(await _userManager.AddToRoleAsync(admin, Consts.ADMINROLE)).Succeeded)
            {
              throw new InvalidOperationException("Failed to update Super User Role");
            }
          }

          EventInfo[] codeCamps;
          if (!_ctx.CodeCampEvents.Any())
          {
            codeCamps = new EventInfo[]
            {
            new EventInfo()
            {
              Moniker = "2017",
              Name = "Atlanta Code Camp 2017",
              EventDate = new DateTime(2017, 9, 17),
              EventLength = 1,
              Description = "The Atlanta Code Camp is awesome! The Atlanta Code Camp is awesome! The Atlanta Code Camp is awesome!",
              IsDefault = true,
              TwitterLink = "https://twitter.com/atlcodecamp",
              ContactEmail = "codecamp@live.com",
              CallForSpeakersOpened = new DateTime(2017, 5, 25),
              CallForSpeakersClosed = new DateTime(2016, 9, 1),
              Location = new EventLocation()
              {
                Facility = "Kennesaw State University",
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
              Moniker = "2016",
              Name = "Atlanta Code Camp 2016",
              EventDate = new DateTime(2016, 10, 15),
              EventLength = 1,
              Description = "The Atlanta Code Camp is awesome",
              IsDefault = true,
              TwitterLink = "https://twitter.com/atlcodecamp",
              ContactEmail = "codecamp@live.com",
              CallForSpeakersOpened = new DateTime(2016, 7, 1),
              CallForSpeakersClosed = new DateTime(2016, 10, 1),
              Location = new EventLocation()
              {
                Facility = "Kennesaw State University",
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
              CallForSpeakersOpened = new DateTime(2015, 8, 1),
              CallForSpeakersClosed = new DateTime(2015, 10, 1),
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
              EventDate = new DateTime(2014, 10, 11),
              EventLength = 1,
              Description = "The Atlanta Code Camp is awesome",
              IsDefault = false,
              CallForSpeakersOpened = new DateTime(2014, 8, 1),
              CallForSpeakersClosed = new DateTime(2014, 9, 15),
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
              EventDate = new DateTime(2013, 8, 24),
              EventLength = 1,
              Description = "The Atlanta Code Camp is awesome",
              IsDefault = false,
              CallForSpeakersOpened = new DateTime(2013, 6, 1),
              CallForSpeakersClosed = new DateTime(2013, 8, 1),
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

            _ctx.AddRange(codeCamps);

            await _ctx.SaveChangesAsync();
          }
          else
          {
            codeCamps = _ctx.CodeCampEvents.ToArray();
          }

          if (!_ctx.Sponsors.Any())
          {
            var sponsor = new Sponsor()
            {
              Name = "Wilder Minds",
              Link = "http://wilderminds.com",
              Event = codeCamps[0],
              Paid = true,
              ImageUrl = "/img/2016/sponsors/wilder-minds.jpg",
              SponsorLevel = "Silver"
            };
            _ctx.Add(sponsor);

            sponsor = new Sponsor()
            {
              Name = "Wilder Minds",
              Link = "http://wilderminds.com",
              Event = codeCamps[1],
              Paid = true,
              ImageUrl = "/img/2016/sponsors/wilder-minds.jpg",
              SponsorLevel = "Silver"
            };
            _ctx.Add(sponsor);

            _ctx.AddRange(Add2015Sponsors(codeCamps.First(s => s.Moniker == "2015")));
            _ctx.AddRange(Add2014Sponsors(codeCamps.First(s => s.Moniker == "2014")));
            _ctx.AddRange(Add2013Sponsors(codeCamps.First(s => s.Moniker == "2013")));

            await _ctx.SaveChangesAsync();
          }

          if (!_ctx.Speakers.Any())
          {
            await Migrate();
          }
        }
      }
      catch (Exception ex)
      {
        // TODO
        ex.ToString();
      }
    }

    private IEnumerable<Sponsor> Add2015Sponsors(EventInfo eventInfo)
    {
      var sponsors = new List<Sponsor>()
      {
        new Sponsor()
        {
          Name = "Magenic",
          Link = "http://www.magenic.com/",
          ImageUrl = "/img/2015/sponsors/magenic.jpg",
          SponsorLevel = "Platinum"
        },
        new Sponsor()
        {
          Name = "Slalom Consulting",
          Link = "http://slalom.com/",
          ImageUrl = "/img/2015/sponsors/slalom.png",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "Matrix",
          Link = "http://www.matrixres.com/",
          ImageUrl = "/img/2015/sponsors/matrix.jpg",
          SponsorLevel = "Other"
        },
        new Sponsor()
        {
          Name = "Tek Systems",
          Link = "http://teksystems.com/",
          ImageUrl = "/img/2015/sponsors/teksystems.png",
          SponsorLevel = "Platinum"
        },
        new Sponsor()
        {
          Name = "Wilder Minds",
          Link = "http://www.wilderminds.com/",
          ImageUrl = "/img/2015/sponsors/wilderminds.jpg",
          SponsorLevel = "Silver"
        },
        new Sponsor()
        {
          Name = "GreaterSum",
          Link = "http://www.greatersum.com/",
          ImageUrl = "/img/2015/sponsors/greatersum.jpg",
          SponsorLevel = "TShirt"
        },
        new Sponsor()
        {
          Name = "Air Watch",
          Link = "http://air-watch.com/",
          ImageUrl = "/img/2015/sponsors/airwatch.jpg",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "CTS",
          Link = "http://www.askcts.com/",
          ImageUrl = "/img/2015/sponsors/cts.jpg",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "iVision",
          Link = "http://ivision.com/our-services/technology-services/application-development/",
          ImageUrl = "/img/2015/sponsors/ivision.jpg",
          SponsorLevel = "Platinum"
        },
        new Sponsor()
        {
          Name = "Wintellect",
          Link = "http://www.wintellect.com",
          ImageUrl = "/img/2015/sponsors/wintellect.jpg",
          SponsorLevel = "Silver"
        },
        new Sponsor()
        {
          Name = "LogicNP Software",
          Link = "http://ssware.com",
          ImageUrl = "/img/2015/sponsors/LogicNP.png",
          SponsorLevel = "Other"
        },
        new Sponsor()
        {
          Name = "Red-gate",
          Link = "http://www.red-gate.com/",
          ImageUrl = "/img/2015/sponsors/redgate.png",
          SponsorLevel = "Other"
        },
        new Sponsor()
        {
          Name = "Tyler Tech",
          Link = "http://www.tylertech.com/",
          ImageUrl = "/img/2015/sponsors/tylertech.jpg",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "CBS",
          Link = "http://cbscorporation.jobs/",
          ImageUrl = "/img/2015/sponsors/cbs.jpg",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "Hired",
          Link = "http://hired.com/",
          ImageUrl = "/img/2015/sponsors/hired.jpg",
          SponsorLevel = "Platinum"
        },
        new Sponsor()
        {
          Name = "Innovative Architects",
          Link = "https://www.innovativearchitects.com/",
          ImageUrl = "/img/2015/sponsors/innovative-architects.jpg",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "PMG.net",
          Link = "https://www.pmg.net/",
          ImageUrl = "/img/2015/sponsors/pmg.jpg",
          SponsorLevel = "Gold"
        },
      };

      sponsors.ForEach(s => { s.Paid = true; s.Event = eventInfo; });

      return sponsors;
    }

    private IEnumerable<Sponsor> Add2014Sponsors(EventInfo eventInfo)
    {
      var sponsors = new List<Sponsor>()
      {
        new Sponsor()
        {
          Name = "Magenic",
          Link ="http://www.magenic.com/",
          ImageUrl = "/img/2014/sponsors/magenic.jpg",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "Peachtree Data",
          Link ="https://developer.peachtreedata.com",
          ImageUrl = "/img/2014/sponsors/peachtreedata.jpg",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "Wilder Minds",
          Link ="http://www.wilderminds.com/",
          ImageUrl = "/img/2014/sponsors/wilderminds.jpg",
          SponsorLevel = "Silver"
        },
        new Sponsor()
        {
          Name = "iVision",
          Link ="http://www.iVision.com/",
          ImageUrl = "/img/2014/sponsors/ivision.jpg",
          SponsorLevel = "Silver"
        },
        new Sponsor()
        {
          Name = "Slalom Consulting",
          Link ="http://slalom.com/",
          ImageUrl = "/img/2014/sponsors/slalom.png",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "RDA",
          Link ="http://www.rdacorp.com/",
          ImageUrl = "/img/2014/sponsors/rda.jpg",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "Mandrill",
          Link ="http://mandrill.com/",
          ImageUrl = "/img/2014/sponsors/mandrill.png",
          SponsorLevel = "Silver"
        },
        new Sponsor()
        {
  Name = "Matrix",
          Link ="http://www.matrixres.com/",
          ImageUrl = "/img/2014/sponsors/matrix.jpg",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "CTS/Particular Software",
          Link ="http://www.askcts.com/",
          ImageUrl = "/img/2014/sponsors/ctsparticular.jpg",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "Wintellect",
          Link ="http://www.wintellect.com",
          ImageUrl = "/img/2014/sponsors/wintellect.jpg",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "Air Watch",
          Link ="http://air-watch.com/",
          ImageUrl = "/img/2014/sponsors/airwatch.jpg",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "Tek Systems",
          Link ="http://teksystems.com/",
          ImageUrl = "/img/2014/sponsors/teksystems.png",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "Tyler Tech",
          Link ="http://www.tylertech.com/",
          ImageUrl = "/img/2014/sponsors/tylertech.jpg",
          SponsorLevel = "Gold"
        },
      };

      sponsors.ForEach(s => { s.Paid = true; s.Event = eventInfo; });

      return sponsors;
    }

    private IEnumerable<Sponsor> Add2013Sponsors(EventInfo eventInfo)
    {
      var sponsors = new List<Sponsor>()
      {
        new Sponsor()
        {
          Name = "Component One",
          Link ="http://www.componentone.com/",
          ImageUrl = "/img/2013/sponsors/componentone.png",
          SponsorLevel = "Platinum"
        },
        new Sponsor()
        {
          Name = "Magenic",
          Link ="http://www.magenic.com/",
          ImageUrl = "/img/2013/sponsors/magenic.png",
          SponsorLevel = "Platinum"
        },
        new Sponsor()
        {
          Name = "Matrix",
          Link ="http://www.matrixres.com/",
          ImageUrl = "/img/2013/sponsors/matrix.jpg",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "CTS",
          Link ="http://www.askcts.com/",
          ImageUrl = "/img/2013/sponsors/cts.jpg",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "Telerik",
          Link ="http://www.telerik.com/",
          ImageUrl = "/img/2013/sponsors/telerik.jpg",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "Pariveda Solutions",
          Link ="http://www.parivedasolutions.com/",
          ImageUrl = "/img/2013/sponsors/parivedasolutions.png",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "Wilder Minds",
          Link ="http://www.wilderminds.com/",
          ImageUrl = "/img/2013/sponsors/wilderminds.jpg",
          SponsorLevel = "Silver"
        },
        new Sponsor()
        {
          Name = "Discount ASP",
          Link ="http://discountasp.com/",
          ImageUrl = "/img/2013/sponsors/discountasp.png",
          SponsorLevel = "Silver"
        },
        new Sponsor()
        {
          Name = "Agile Thought",
          Link ="http://agilethought.com/",
          ImageUrl = "/img/2013/sponsors/agilethought.png",
          SponsorLevel = "Platinum"
        },
        new Sponsor()
        {
          Name = "Air Watch",
          Link ="http://air-watch.com/",
          ImageUrl = "/img/2013/sponsors/airwatch.jpg",
          SponsorLevel = "Platinum"
        },
        new Sponsor()
        {
          Name = "Pluralsight",
          Link ="http://pluralsight.com/",
          ImageUrl = "/img/2013/sponsors/pluralsight.png",
          SponsorLevel = "Other"
        },
        new Sponsor()
        {
          Name = "Code Magazine",
          Link ="http://code-magazine.com/",
          ImageUrl = "/img/2013/sponsors/codemag.jpg",
          SponsorLevel = "Other"
        },
        new Sponsor()
        {
          Name = "Campus MVP",
          Link ="http://campusmvp.net/",
          ImageUrl = "/img/2013/sponsors/campusMVP.png",
          SponsorLevel = "Other"
        },
        new Sponsor()
        {
          Name = "LogicNP Software",
          Link ="http://ssware.com/",
          ImageUrl = "/img/2013/sponsors/logicnp.png",
          SponsorLevel = "Other"
        },
        new Sponsor()
        {
          Name = "O'Reilly Publishing",
          Link ="http://oreilly.com/",
          ImageUrl = "/img/2013/sponsors/oreilly.gif",
          SponsorLevel = "Other"
        },
        new Sponsor()
        {
          Name = "Blue Fletch Consulting",
          Link ="http://bluefletch.com/",
          ImageUrl = "/img/2013/sponsors/blue-fletch.png",
          SponsorLevel = "Silver"
        },
        new Sponsor()
        {
          Name = "Bit Wizards",
          Link ="http://www.bitwizards.com/",
          ImageUrl = "/img/2013/sponsors/bitwizards.jpg",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "Mandrill",
          Link ="http://mandrill.com/",
          ImageUrl = "/img/2013/sponsors/mandrill.png",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "MailChimp",
          Link ="http://mailchimp.com/",
          ImageUrl = "/img/2013/sponsors/mailchimp.png",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "Jet Stream Technologies",
          Link ="http://jetstreamapp.com/",
          ImageUrl = "/img/2013/sponsors/jetstream.png",
          SponsorLevel = "Silver"
        },
        new Sponsor()
        {
          Name = "RedGate",
          Link ="http://red-gate.com/",
          ImageUrl = "/img/2013/sponsors/redgate.png",
          SponsorLevel = "Silver"
        },
        new Sponsor()
        {
          Name = "Slalom Consulting",
          Link ="http://slalom.com/",
          ImageUrl = "/img/2013/sponsors/slalom.png",
          SponsorLevel = "Silver"
        },
        new Sponsor()
        {
          Name = "Tek Systems",
          Link ="http://teksystems.com/",
          ImageUrl = "/img/2013/sponsors/teksystems.png",
          SponsorLevel = "Platinum"
        },
        new Sponsor()
        {
          Name = "Infragistics",
          Link ="http://infragistics.com/",
          ImageUrl = "/img/2013/sponsors/infragistics.png",
          SponsorLevel = "Platinum"
        },
        new Sponsor()
        {
          Name = "Daugherty Business Solutions",
          Link ="http://www.daugherty.com/",
          ImageUrl = "/img/2013/sponsors/daugherty.png",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "Fabric.com",
          Link ="http://www.fabric.com/",
          ImageUrl = "/img/2013/sponsors/fabric.png",
          SponsorLevel = "Silver"
        },
        new Sponsor()
        {
          Name = "Apex Payroll",
          Link ="http://www.apexpayroll.com",
          ImageUrl = "/img/2013/sponsors/apexpr.png",
          SponsorLevel = "Gold"
        },
        new Sponsor()
        {
          Name = "Syncfusion",
          Link ="http://www.syncfusion.com",
          ImageUrl = "/img/2013/sponsors/syncfusion.png",
          SponsorLevel = "Silver"
        },
      };

      sponsors.ForEach(s => { s.Paid = true; s.Event = eventInfo; });

      return sponsors;
    }

    private async Task Migrate()
    {
      var speakerFile = Path.Combine(_env.ContentRootPath, @"..\..\testdata\speakers.csv");
      if (File.Exists(speakerFile))
      {
        using (var oldSpeakers = new CsvReader(File.OpenText(speakerFile), CultureInfo.InvariantCulture))
        {
          if (oldSpeakers.Read())
          {
            oldSpeakers.ReadHeader();
            while (oldSpeakers.Read())
            {
              _ctx.Add(MigrateSpeaker(oldSpeakers));
            }
          }
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
        ImageUrl = oldSpeakers.GetField("ImageUrl").Replace("/images/", "/img/"),
        Name = oldSpeakers.GetField("Name"),
        Title = oldSpeakers.GetField("Title"),
        Twitter = oldSpeakers.GetField("Twitter"),
        UserName = "",
        Website = "",
        Talks = MigrateTalks(Int32.Parse(oldSpeakers.GetField("Id")), camp)
      };
    }

    private ICollection<Talk> MigrateTalks(int speakerId, EventInfo camp)
    {
      var result = new List<Talk>();

      var talkFile = Path.Combine(_env.ContentRootPath, @"..\..\testdata\talks.csv");
      if (File.Exists(talkFile))
      {
        using (var oldTalks = new CsvReader(File.OpenText(talkFile), CultureInfo.InvariantCulture))
        {
          if (oldTalks.Read())
          {
            oldTalks.ReadHeader();
            while (oldTalks.Read())
            {
              if (Int32.Parse(oldTalks.GetField("Speaker_Id")) == speakerId)
              {
                result.Add(MigrateTalk(oldTalks, camp));
              }
            }
          }
        }
      }

      return result;
    }

    private Talk MigrateTalk(CsvReader oldTalks, EventInfo camp)
    {
      var talk = new Talk()
      {
        Abstract = oldTalks.GetField("Abstract"),
        Approved = true,
        Audience = oldTalks.GetField("Audience"),
        CodeUrl = oldTalks.GetField("CodeUrl"),
        Level = ConvertToLevel(oldTalks.GetField("Level")),
        Prerequisites = oldTalks.GetField("Prerequisites"),
        PresentationUrl = oldTalks.GetField("PresentationUrl"),
        SpeakerDeckUrl = oldTalks.GetField("SpeakerDeckUrl"),
        SpeakerRateUrl = oldTalks.GetField("SpeakerRateUrl"),
        Title = oldTalks.GetField("Title"),
        Category = oldTalks.GetField("Category"),
        Room = HandleRoom(oldTalks.GetField("Room"), camp),
        TimeSlot = HandleTalkTime(oldTalks.GetField("StartTime"), camp)
      };

      _ctx.Add(talk);

      return talk;
    }

    List<Room> _rooms = new List<Room>();

    private Room HandleRoom(string roomName, EventInfo camp)
    {
      if (string.IsNullOrWhiteSpace(roomName)) return null;

      var room = _rooms.Where(r => r.Event.Moniker == camp.Moniker && r.Name == roomName).FirstOrDefault();
      if (room == null)
      {
        // Make new room
        room = new Room()
        {
          Name = roomName,
          Event = camp
        };
        _ctx.Rooms.Add(room);
        _rooms.Add(room);
      }

      return room;
    }

    List<TimeSlot> _slots = new List<TimeSlot>();

    private TimeSlot HandleTalkTime(string talkTime, EventInfo camp)
    {
      DateTime timeSlot;

      var culture = new CultureInfo("en-US");

      if ((!DateTime.TryParseExact(talkTime, "hh:mmtt", culture, DateTimeStyles.AssumeLocal, out timeSlot) &&
        !DateTime.TryParseExact(talkTime, "h:mmtt", culture, DateTimeStyles.AssumeLocal, out timeSlot)) ||
        timeSlot.Hour < 4)
      {
        return null;
      }

      var slot = _slots.Where(r => r.Event.Moniker == camp.Moniker && r.Time == timeSlot).FirstOrDefault();
      if (slot == null)
      {
        // Make new room
        slot = new TimeSlot()
        {
          Time = timeSlot,
          Event = camp
        };
        _ctx.TimeSlots.Add(slot);
        _slots.Add(slot);
      }

      return slot;
    }

    private string ConvertToLevel(string lvl)
    {
      if (lvl == "300") return "Advanced";
      if (lvl == "200") return "Intermediate";
      return "Beginner";
    }
  }
}
