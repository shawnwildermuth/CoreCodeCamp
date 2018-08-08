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
  [Route("{moniker}/api/timeslots")]
  [ApiController]
  public class TimeSlotsController : Controller
  {
    private ICodeCampRepository _repo;
    private ILogger<TimeSlotsController> _logger;

    public TimeSlotsController(ICodeCampRepository repo, ILogger<TimeSlotsController> logger)
    {
      _logger = logger;
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
        catch (Exception ex)
        {
          _logger.LogError("Failed to create a timeslot: {0}", ex);
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
      catch (Exception ex)
      {
        _logger.LogError("Failed to delete a time slot: {0}", ex);
      }

      return BadRequest("Failed to delete TimeSlot");
    }

  }
}
