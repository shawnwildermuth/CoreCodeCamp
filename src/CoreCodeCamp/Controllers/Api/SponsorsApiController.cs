using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data;
using CoreCodeCamp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers.Api
{
  [Route("api")]
  [Authorize(Roles = Consts.ADMINROLE)]
  public class SponsorsApiController : Controller
  {
    private ICodeCampRepository _repo;

    public SponsorsApiController(ICodeCampRepository repo)
    {
      _repo = repo;
    }

    [HttpGet("events")]
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

    [HttpGet("sponsors/{moniker}")]
    public IActionResult GetSponsors(string moniker)
    {
      try
      {
        return Ok(_repo.GetSponsors(moniker));
      }
      catch
      {
        return BadRequest("Failed to get sponsors");
      }
    }

  }
}
