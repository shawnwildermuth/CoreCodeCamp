using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using Microsoft.AspNetCore.Mvc;

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

    public IActionResult Sponsoring(string moniker)
    {
      return View();
    }


  }
}
