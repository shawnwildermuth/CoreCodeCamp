using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Controllers.Web;
using CoreCodeCamp.Data;
using CoreCodeCamp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Areas.Admin.Controllers
{
  [Authorize(Roles = Consts.ADMINROLE)]
  [Area("Admin")]
  public class RootController : MonikerControllerBase
  {
    public RootController(ICodeCampRepository repo) : base(repo)
    {

    }

    public IActionResult Index()
    {
      return View();
    }

    [HttpGet("users")]
    public IActionResult Users()
    {
      return View();
    }

    [HttpGet("talks")]
    public IActionResult Talks()
    {
      return View();
    }

    [HttpGet("sponsors")]
    public IActionResult Sponsors()
    {
      return View();
    }
  }
}
