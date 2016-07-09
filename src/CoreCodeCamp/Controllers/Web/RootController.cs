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
  public class RootController : Controller
  {
    private ICodeCampRepository _repo;

    public RootController(ICodeCampRepository repo)
    {
      _repo = repo;
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

    public override void OnActionExecuting(ActionExecutingContext context)
    {
      base.OnActionExecuting(context);

      // Put the current event in scope data
      context.HttpContext.Items["EventInfo"] = _repo.GetEventInfo(context.RouteData.Values["moniker"] as string);
    }

  }
}
