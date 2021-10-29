using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models;
using CoreCodeCamp.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using TimeZoneConverter;

namespace CoreCodeCamp.Controllers.Web
{
  public class RootController : MonikerControllerBase
  {
    public RootController(ICodeCampRepository repo, ILogger<RootController> logger, IMapper mapper, IWebHostEnvironment env) 
      : base(repo, logger, mapper)
    {
      _env = env;
    }

    List<string> _levels = new List<string> { "Platinum",
              "Attendee Party",
              "Speaker Dinner",
              "Attendee Shirts",
              "TShirt",
              "Speaker Shirts",
              "Gold",
              "Silver",
              "Swag",
              "Other"};
    private readonly IWebHostEnvironment _env;

    public async Task<IActionResult> Index(string moniker)
    {

      var sponsors = (await _repo.GetSponsorsAsync(moniker))
                  .OrderBy(s => _levels.IndexOf(s.SponsorLevel))
                 .ThenBy(s => Guid.NewGuid())
                 .ToList();

      var urlToSpk = Path.Combine("img", moniker, "keynote-speaker.jpg");
      var fileInfo = _env.WebRootFileProvider.GetFileInfo(urlToSpk);
      ViewBag.HasSpeaker = fileInfo.Exists;

      return View(sponsors);
    }

    [HttpGet("{moniker}/Sponsoring")]
    public IActionResult Sponsoring(string moniker)
    {
      return View();
    }

    [HttpGet("{moniker}/Sponsors")]
    public async Task<IActionResult> Sponsors(string moniker)
    {
      var sponsors = (await _repo.GetSponsorsAsync(moniker))
                  .OrderBy(s => _levels.IndexOf(s.SponsorLevel))
                 .ThenBy(s => Guid.NewGuid())
                 .ToList();

      return View(sponsors);
    }

    [HttpGet("{moniker}/Speakers")]
    public async Task<IActionResult> Speakers(string moniker)
    {
      var speakers = (await _repo.GetSpeakersAsync(moniker)).Where(s => s.Talks.Any(t => t.Approved)).OrderBy(s => s.Name).ToList();

      return View(speakers);
    }

    [HttpGet("{moniker}/Speakers/{id}", Name = "SpeakerTalkPage")]
    public async Task<IActionResult> Speaker(string moniker, string id)
    {

      var speaker = await _repo.GetSpeakerByNameAsync(moniker, id);

      if (!User.IsInRole(Consts.ADMINROLE))
      {
        if (!speaker.Talks.Any(t => t.Approved)) return RedirectToAction("Speakers");
      }

      var vm = _mapper.Map<SpeakerViewModel>(speaker);
      vm.Talks = _mapper.Map<ICollection<TalkViewModel>>(speaker.Talks.Where(t => t.Approved).ToList());

      if (User.Identity.IsAuthenticated)
      {
        var favs = await _repo.GetUserWithFavoriteTalksForEventAsync(User.Identity.Name, moniker);
        foreach (var talk in vm.Talks)
        {
          talk.Favorite = favs.Any(f => f.Id == talk.Id);
        }
      }

      return View(vm);
    }

    [HttpGet("{moniker}/Sessions")]
    public async Task<IActionResult> Sessions(string moniker)
    {
      var talks = (await _repo.GetTalksAsync(moniker)).Where(t => t.Approved).ToList();
      var sessions = _mapper.Map<List<TalkViewModel>>(talks);

      if (User.Identity.IsAuthenticated)
      {
        var favs = await _repo.GetUserWithFavoriteTalksForEventAsync(User.Identity.Name, moniker);
        sessions.ForEach(t => t.Favorite = favs.Any(f => f.Id == t.Id));
      }

      return View(sessions);
    }

    [HttpGet("{moniker}/Schedule")]
    public async Task<IActionResult> Schedule(string moniker)
    {
      if (string.IsNullOrWhiteSpace(this._theEvent.SessionizeId))
      {
        var favorites = await _repo.GetUserWithFavoriteTalksForEventAsync(User.Identity.Name, moniker);

        var slots = await _repo.GetTalksInSlotsAsync(moniker);

        var categories = slots.SelectMany(s => s.ToList())
        .SelectMany(s => s.Talks)
        .Select(t => t.Category)
        .OrderBy(t => t)
        .Distinct()
        .ToList();

        DateTime pickedSlot = DateTime.MinValue;

        if (slots.Count() > 0)
        {
          var easternZone = TZConvert.GetTimeZoneInfo("Eastern Standard Time");
          var eventTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

          if (eventTime.Date == this._theEvent.EventDate)
          {
            pickedSlot = slots[0].First().Time;

            foreach (var slot in slots)
            {
              if (slot.First().Time > eventTime)
              {
                pickedSlot = slot.First().Time;
              }
            }
          }
        }
        return View(Tuple.Create(slots, favorites, pickedSlot, categories));
      }

      return View();
    }

    [HttpGet("{moniker}/Register")]
    public IActionResult Register(string moniker)
    {
      return View();
    }

      [HttpGet("codeofconduct")]
      public IActionResult CodeOfConduct()
      {
          return View();
      }
    }
}
