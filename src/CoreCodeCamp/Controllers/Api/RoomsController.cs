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

namespace CoreCodeCamp.Controllers.Api
{
  [Authorize(Roles = Consts.ADMINROLE)]
  [Route("{moniker}/api/rooms")]
  public class RoomsController : Controller
  {
    private ICodeCampRepository _repo;

    public RoomsController(ICodeCampRepository repo)
    {
      _repo = repo;
    }

    [HttpGet()]
    public IActionResult Get(string moniker)
    {
      return Ok(_repo.GetRooms(moniker));
    }

    [HttpPost()]
    public async Task<IActionResult> Create(string moniker, [FromBody]Room model)
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

            return Created($"{moniker}/api/rooms/{model.Id}", model);
          }
        }
        catch
        {

        }
      }

      return BadRequest("Failed to save Room");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(string moniker, int id)
    {
      try
      {
        var room = _repo.GetRoom(moniker, id);
        _repo.Delete(room);
        await _repo.SaveChangesAsync();

        return Ok();
      }
      catch
      {

      }

      return BadRequest("Failed to delete Room");
    }
  }
}
