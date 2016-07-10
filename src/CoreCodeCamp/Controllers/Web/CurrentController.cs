using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers.Web
{
  [Route("~/")]
  public class CurrentController : MonikerControllerBase
  {

    public CurrentController(ICodeCampRepository repo) : base(repo)
    {
    }

    [HttpGet("")]
    public IActionResult Index()
    {
      var currentEvent = _repo.GetCurrentEvent();

      return RedirectToAction("Index", "Root", new { moniker = currentEvent.Moniker });
    }

  }
}
