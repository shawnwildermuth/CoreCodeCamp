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
  [Route("{moniker}/api/timeslots")]
  public class TimeSlotsController : Controller
  {
    private ICodeCampRepository _repo;

    public TimeSlotsController(ICodeCampRepository repo)
    {
      _repo = repo;
    }

    [HttpGet()]
    public IActionResult Get(string moniker)
    {
      return Ok(_repo.GetTimeSlots(moniker));
    }

    [HttpPost()]
    public async Task<IActionResult> Create(string moniker, [FromBody]TimeSlot model)
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

            return Created($"{moniker}/api/timeslot/{model.Id}", model);
          }
        }
        catch
        {

        }
      }

      return BadRequest("Failed to save TimeSlot");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(string moniker, int id)
    {
      try
      {
        var timeSlot = _repo.GetTimeSlot(moniker, id);
        _repo.Delete(timeSlot);
        await _repo.SaveChangesAsync();

        return Ok();
      }
      catch
      {

      }

      return BadRequest("Failed to delete TimeSlot");
    }

  }
}
