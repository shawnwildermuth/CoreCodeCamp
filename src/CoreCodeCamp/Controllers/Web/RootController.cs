using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
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
  }
}
