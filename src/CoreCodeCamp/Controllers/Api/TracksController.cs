using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodeCamp.Controllers.Api
{
  [Authorize(Roles = Consts.ADMINROLE)]
  [Route("{moniker}/api/tracks")]
  [ApiController]
  public class TracksController : Controller
  {
    private ICodeCampRepository _repo;
    private ILogger<TracksController> _logger;

    public TracksController(ICodeCampRepository repo, ILogger<TracksController> logger)
    {
      _logger = logger;
      _repo = repo;
    }

    [HttpGet()]
    public IActionResult Get(string moniker)
    {
      return Ok(_repo.GetTracks(moniker));
    }

    [HttpPost()]
    public async Task<IActionResult> Create(string moniker, [FromBody]Track model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var eventInfo = _repo.GetEventInfo(moniker);
          if (eventInfo != null)
          {
            model.Event = eventInfo;

            _repo.AddOrUpdate(model);
            await _repo.SaveChangesAsync();

            return Created($"{moniker}/api/tracks/{model.Id}", model);
          }
        }
        catch (Exception ex)
        {
          _logger.LogError("Failed to create a track: {0}", ex);
        }

      }

      return BadRequest("Failed to save Track");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(string moniker, int id)
    {
      try
      {
        var track = _repo.GetTrack(moniker, id);
        _repo.Delete(track);
        await _repo.SaveChangesAsync();

        return Ok();
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to delete a track: {0}", ex);
      }

      return BadRequest("Failed to delete Track");
    }

  }
}
