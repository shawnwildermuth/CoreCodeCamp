using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers.Api
{
  [Authorize]
  [Route("api/talks")]
  public class TalksController : Controller
  {
    private ICodeCampRepository _repo;
    private UserManager<CodeCampUser> _userMgr;

    public TalksController(ICodeCampRepository repo, UserManager<CodeCampUser> userMgr)
    {
      _repo = repo;
      _userMgr = userMgr;

    }

    [HttpPut("{talkId:int}/toggleStar")]
    public async Task<IActionResult> ToggleStar(int talkId)
    {
      try
      {
        if (!User.Identity.IsAuthenticated)
        {
          return BadRequest("Cannot set favorites without logging in");
        }

        _repo.ToggleTalkForUser(User.Identity.Name, talkId);
        await _repo.SaveChangesAsync();
        return Ok();
      }
      catch
      {

      }

      return BadRequest("Failed to toggle star");
    }
  }
}
