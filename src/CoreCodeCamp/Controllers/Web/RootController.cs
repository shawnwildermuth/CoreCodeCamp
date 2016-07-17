using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreCodeCamp.Controllers.Web
{
  public class RootController : MonikerControllerBase
  {
    public RootController(ICodeCampRepository repo) : base(repo)
    {
    }

    public IActionResult Index(string moniker)
    {
      var sponsors = _repo.GetSponsors(moniker);
      return View(sponsors);
    }

    [HttpGet("{moniker}/Sponsoring")]
    public IActionResult Sponsoring(string moniker)
    {
      return View();
    }

    [HttpGet("{moniker}/Speakers")]
    public IActionResult Speakers(string moniker)
    {
      var speakers = _repo.GetSpeakers(moniker).Where(s => s.Talks.Any(t => t.Approved)).OrderBy(s => s.Name).ToList();

      return View(speakers);
    }

    [HttpGet("{moniker}/Speakers/{id}")]
    public IActionResult Speaker(string moniker, string id)
    {
      var speaker = _repo.GetSpeakerByName(moniker, id);
      var vm = Mapper.Map<SpeakerViewModel>(speaker);
      vm.Talks = Mapper.Map<ICollection<TalkViewModel>>(speaker.Talks);

      if (User.Identity.IsAuthenticated)
      {
        var user = _repo.GetUserWithFavorites(User.Identity.Name);
        foreach (var talk in vm.Talks)
        {
          talk.Favorite = user.FavoriteTalks.Any(f => f.Talk.Id == talk.Id);
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
        var user = _repo.GetUserWithFavorites(User.Identity.Name);
        sessions.ForEach(t => t.Favorite = user.FavoriteTalks.Any(f => f.Talk.Id == t.Id));
      }

      return View(sessions);
    }

    [HttpGet("{moniker}/Schedule")]
    public IActionResult Schedule(string moniker)
    {
      return View();
    }
  }
}
