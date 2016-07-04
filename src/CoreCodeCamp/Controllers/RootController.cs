using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers
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
      return View(EnsureEvent(moniker));
    }

    private EventInfo EnsureEvent(string moniker)
    {
      var eventInfo = _repo.GetEventInfo(moniker);

      if (eventInfo == null)
      {
        eventInfo = _repo.GetCurrentEvent();
      }

      return eventInfo;
    }

    public IActionResult About(string moniker)
    {
      ViewData["Message"] = "About this Site";

      return View(EnsureEvent(moniker));
    }

    public IActionResult Contact(string moniker)
    {
      ViewData["Message"] = "Your contact page.";

      return View(EnsureEvent(moniker));
    }

  }
}
