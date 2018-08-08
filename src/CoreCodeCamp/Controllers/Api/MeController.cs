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
using Microsoft.Extensions.Logging;

namespace CoreCodeCamp.Controllers.Api
{
  [Route("{moniker}/api/me")]
  [Authorize]
  [ApiController]
  public class MeController : Controller
  {
    private ILogger<MeController> _logger;
    private readonly IMapper _mapper;
    private ICodeCampRepository _repo;

    public MeController(ICodeCampRepository repo, ILogger<MeController> logger, IMapper mapper)
    {
      _repo = repo;
      _logger = logger;
      _mapper = mapper;
    }

    [HttpGet("favorites")]
    public IActionResult GetFavorites(string moniker)
    {
      var favs = _repo.GetUserWithFavoriteTalksForEvent(User.Identity.Name, moniker);
      return Ok(_mapper.Map<IEnumerable<FavoriteTalkViewModel>>(favs));
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
      catch (Exception ex)
      {
        _logger.LogError("Failed to save favorite talk: {0}", ex);
      }

      return BadRequest("Failed to toggle star");
    }

  }
}
