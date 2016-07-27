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
using Microsoft.Extensions.Logging;

namespace CoreCodeCamp.Areas.Admin.Controllers
{
  [Authorize(Roles = Consts.ADMINROLE)]
  [Area("Admin")]
  public class RootController : MonikerControllerBase
  {
    public RootController(ICodeCampRepository repo, ILogger<RootController> logger)
     : base(repo, logger)
    {

    }

    [HttpGet("[area]")]
    public IActionResult Index()
    {
      return View(_repo.GetAllEventInfo());
    }

    [HttpGet("[area]/users")]
    public IActionResult Users()
    {
      return View();
    }

    [HttpGet("{moniker}/[area]/schedule")]
    public IActionResult Schedule(string moniker)
    {
      return View();
    }

    [HttpGet("{moniker}/[area]/sponsors")]
    public IActionResult Sponsors(string moniker)
    {
      return View();
    }

    [HttpGet("{moniker}/[area]/eventinfo")]
    public IActionResult EventInfo(string moniker)
    {
      return View();
    }
  }
}
