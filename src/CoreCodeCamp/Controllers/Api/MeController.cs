using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers.Api
{
  [Route("{moniker}/api/me")]
  [Authorize]
  public class MeController : Controller
  {
    private ICodeCampRepository _repo;

    public MeController(ICodeCampRepository repo)
    {
      _repo = repo;
    }

    [HttpGet("favorites")]
    public IActionResult GetFavorites(string moniker)
    {
      var favs = _repo.GetUserWithFavoriteTalksForEvent(User.Identity.Name, moniker);
      return Ok(Mapper.Map<IEnumerable<FavoriteTalkViewModel>>(favs));
    }

    [HttpPut("favorites/{talkId:int}")]
    public async Task<IActionResult> ToggleStar(string moniker, int talkId)
    {
      try
      {
        var state = _repo.ToggleTalkForUser(moniker, User.Identity.Name, talkId);
        await _repo.SaveChangesAsync();
        return Ok(state);
      }
      catch
      {
      }

      return BadRequest("Failed to toggle star");
    }

  }
}
