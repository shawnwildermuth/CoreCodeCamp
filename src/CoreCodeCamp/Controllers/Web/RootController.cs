using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models;
using CoreCodeCamp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CoreCodeCamp.Controllers.Web
{
  public class RootController : MonikerControllerBase
  {
    public RootController(ICodeCampRepository repo, ILogger<RootController> logger) : base(repo, logger)
    {
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

    public IActionResult Index(string moniker)
    {

      var sponsors = _repo.GetSponsors(moniker)
                  .OrderBy(s => _levels.IndexOf(s.SponsorLevel))
                 .ThenBy(s => Guid.NewGuid())
                 .ToList();

      return View(sponsors);
    }

    [HttpGet("{moniker}/Sponsoring")]
    public IActionResult Sponsoring(string moniker)
    {
      return View();
    }

    [HttpGet("{moniker}/Sponsors")]
    public IActionResult Sponsors(string moniker)
    {
      var sponsors = _repo.GetSponsors(moniker)
                  .OrderBy(s => _levels.IndexOf(s.SponsorLevel))
                 .ThenBy(s => Guid.NewGuid())
                 .ToList();

      return View(sponsors);
    }

    [HttpGet("{moniker}/Speakers")]
    public IActionResult Speakers(string moniker)
    {
      var speakers = _repo.GetSpeakers(moniker).Where(s => s.Talks.Any(t => t.Approved)).OrderBy(s => s.Name).ToList();

      return View(speakers);
    }

    [HttpGet("{moniker}/Speakers/{id}", Name = "SpeakerTalkPage")]
    public IActionResult Speaker(string moniker, string id)
    {

      var speaker = _repo.GetSpeakerByName(moniker, id);

      if (!User.IsInRole(Consts.ADMINROLE))
      {
        if (!speaker.Talks.Any(t => t.Approved)) return RedirectToAction("Speakers");
      }

      var vm = Mapper.Map<SpeakerViewModel>(speaker);
      vm.Talks = Mapper.Map<ICollection<TalkViewModel>>(speaker.Talks.Where(t => t.Approved).ToList());

      if (User.Identity.IsAuthenticated)
      {
        var favs = _repo.GetUserWithFavoriteTalksForEvent(User.Identity.Name, moniker);
        foreach (var talk in vm.Talks)
        {
          talk.Favorite = favs.Any(f => f.Id == talk.Id);
        }
      }

      return View(vm);
    }

    [HttpGet("{moniker}/Sessions")]
    public IActionResult Sessions(string moniker)
    {
      var talks = _repo.GetTalks(moniker).Where(t => t.Approved).ToList();
      var sessions = Mapper.Map<List<TalkViewModel>>(talks);

      if (User.Identity.IsAuthenticated)
      {
        var favs = _repo.GetUserWithFavoriteTalksForEvent(User.Identity.Name, moniker);
        sessions.ForEach(t => t.Favorite = favs.Any(f => f.Id == t.Id));
      }

      return View(sessions);
    }

    [HttpGet("{moniker}/Schedule")]
    public IActionResult Schedule(string moniker)
    {
      var favorites = _repo.GetUserWithFavoriteTalksForEvent(User.Identity.Name, moniker);

      var slots = _repo.GetTalksInSlots(moniker);

      DateTime pickedSlot = DateTime.MinValue;

      if (slots.Count() > 0)
      {
        var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
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
      return View(Tuple.Create(slots, favorites, pickedSlot));
    }

    [HttpGet("{moniker}/Register")]
    public IActionResult Register(string moniker)
    {
      if (this._theEvent == null || string.IsNullOrWhiteSpace(this._theEvent.RegistrationLink)) return RedirectToAction("Index");

      return View();
    }
  }
}
