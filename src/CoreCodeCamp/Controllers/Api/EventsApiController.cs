using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers.Api
{
  [Authorize]
  [Route("api/events")]
  public class EventsApiController : Controller
  {
    private ICodeCampRepository _repo;

    public EventsApiController(ICodeCampRepository repo)
    {
      _repo = repo;
    }

    [HttpGet("")]
    public IActionResult GetEvents()
    {
      try
      {
        return Ok(_repo.GetAllEventInfo()
          .OrderByDescending(e => e.Moniker)
          .Select(e => new { Name = e.Name, Moniker = e.Moniker })
          .ToArray());
      }
      catch
      {
        return BadRequest("Failed to get events");
      }
    }

  }
}
