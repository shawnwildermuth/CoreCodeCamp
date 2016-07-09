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

    [HttpGet("{moniker}/CallForSpeakers")]
    public IActionResult CallForSpeakers(string moniker)
    {
      return View();
    }

  }
}
