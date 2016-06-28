using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodeCamp.Data;
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

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult About()
    {
      ViewData["Message"] = "Your application description page.";

      return View();
    }

    public IActionResult Contact()
    {
      ViewData["Message"] = "Your contact page.";

      return View();
    }

    public IActionResult Error()
    {
      return View();
    }
  }
}
