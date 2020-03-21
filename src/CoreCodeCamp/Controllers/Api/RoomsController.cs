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
  [Route("{moniker}/api/rooms")]
  [ApiController]
  public class RoomsController : Controller
  {
    private ICodeCampRepository _repo;
    private ILogger<RoomsController> _logger;

    public RoomsController(ICodeCampRepository repo, ILogger<RoomsController> logger)
    {
      _repo = repo;
      _logger = logger;
    }

    [HttpGet()]
    public async Task<IActionResult> Get(string moniker)
    {
      return Ok(await _repo.GetRoomsAsync(moniker));
    }

    [HttpPost()]
    public async Task<IActionResult> Create(string moniker, [FromBody]Room model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var eventInfo = await _repo.GetEventInfoAsync(moniker);
          if (eventInfo != null)
          {
            model.Event = eventInfo;
            _repo.AddOrUpdate(model);
            await _repo.SaveChangesAsync();

            return Created($"{moniker}/api/rooms/{model.Id}", model);
          }
        }
        catch (Exception ex)
        {
          _logger.LogError("Failed to save new Room: {0}", ex);
        }
      }

      return BadRequest("Failed to save Room");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(string moniker, int id, [FromBody] Room model)
    {
      try
      {
        var room = await _repo.GetRoomAsync(moniker, id);
        if (room.Id != id || string.IsNullOrWhiteSpace(model.Name)) return BadRequest();
        room.Name = model.Name;
        await _repo.SaveChangesAsync();
        return Ok(room);
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to update Room: {0}", ex);
      }

      return BadRequest("Failed to update Room");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(string moniker, int id)
    {
      try
      {
        var room = await _repo.GetRoomAsync(moniker, id);
        _repo.Delete(room);
        await _repo.SaveChangesAsync();

        return Ok();
      }
      catch (Exception ex)
      {
        _logger.LogError("Couldn't Delete Favorite Room: {0}", ex);
      }

      return BadRequest("Failed to delete Room");
    }
  }
}
