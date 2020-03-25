using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodeCamp.Controllers.Web
{
  [Route("~/")]
  public class CurrentController : MonikerControllerBase
  {

    public CurrentController(ICodeCampRepository repo, ILogger<CurrentController> logger, IMapper mapper) 
      : base(repo, logger, mapper)
    {
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
      var currentEvent = await _repo.GetCurrentEventAsync();

      return RedirectToAction("Index", "Root", new { moniker = currentEvent.Moniker });
    }

  }
}
