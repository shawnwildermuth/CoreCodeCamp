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
    public async Task<IActionResult> Get(string moniker)
    {
      return Ok(await _repo.GetTimeSlotsAsync(moniker));
    }

    [HttpPost()]
    public async Task<IActionResult> Create(string moniker, [FromBody]TimeSlot model)
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

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(string moniker, int id, [FromBody]TimeSlot model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var eventInfo = await _repo.GetEventInfoAsync(moniker);
          if (eventInfo != null)
          {
            var slot = await _repo.GetTimeSlotAsync(moniker, id);
            if (slot == null || slot.Id != id) return BadRequest();

            slot.Time = model.Time.ToLocalTime();

            await _repo.SaveChangesAsync();

            return Ok(slot);
          }
        }
        catch (Exception ex)
        {
          _logger.LogError("Failed to update a timeslot: {0}", ex);
        }
      }

      return BadRequest("Failed to update TimeSlot");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(string moniker, int id)
    {
      try
      {
        var timeSlot = await _repo.GetTimeSlotAsync(moniker, id);
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
