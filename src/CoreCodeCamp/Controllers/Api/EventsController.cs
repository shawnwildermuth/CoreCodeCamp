using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodeCamp.Controllers.Api
{
  [Authorize]
  [Route("api/events")]
  public class EventsController : Controller
  {
    private ICodeCampRepository _repo;
    private ILogger<EventsController> _logger;

    public EventsController(ICodeCampRepository repo, ILogger<EventsController> logger)
    {
      _repo = repo;
      _logger = logger;
    }

    [HttpGet("")]
    [AllowAnonymous]
    public IActionResult GetEvents()
    {
      try
      {
        var events = _repo.GetAllEventInfo()
          .OrderByDescending(e => e.Moniker)
          .ToArray();

        return Ok(Mapper.Map<IEnumerable<EventInfoViewModel>>(events));
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to read events. {0}", ex);

        return BadRequest("Failed to get events");
      }
    }

    [HttpGet("{moniker}")]
    [AllowAnonymous]
    public IActionResult GetEvent(string moniker)
    {
      try
      {
        var info = _repo.GetAllEventInfo()
          .Where(e => e.Moniker == moniker)
          .FirstOrDefault();

        return Ok(Mapper.Map<EventInfoViewModel>(info));
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to read event. {0}", ex);

        return BadRequest("Failed to get event");
      }
    }

    [HttpPost("{moniker}")]
    public async Task<IActionResult> Insert(string moniker, [FromBody]EventInfoViewModel vm)
    {
      try
      {
        if (moniker != vm.Moniker) return BadRequest("Wrong Event with Moniker");
        var info = _repo.GetEventInfo(vm.Moniker);
        if (info != null)
        {
          return BadRequest("Cannot add duplicate moniker");
        }
        else
        {
          info = Mapper.Map<EventInfo>(vm);
          info.Name = $"Event Name for {vm.Moniker}";
          info.Location = new EventLocation();
        }

        _repo.AddOrUpdate(info);
        await _repo.SaveChangesAsync();

        return Created($"/api/events/{info.Moniker}", Mapper.Map<EventInfoViewModel>(info));
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to upsert EventInfo. {0}", ex);
      }

      return BadRequest("Failed to save new event.");
    }

    [HttpPut("{moniker}")]
    public async Task<IActionResult> Update(string moniker, [FromBody]EventInfoViewModel vm)
    {
      try
      {
        if (ModelState.IsValid)
        {
          if (moniker != vm.Moniker) return BadRequest("Wrong Event with Moniker");
          var info = _repo.GetEventInfo(moniker);
          if (info == null)
          {
            return BadRequest("Cannot add update new event");
          }
          else
          {
            Mapper.Map(vm, info);
          }

          _repo.AddOrUpdate(info);
          await _repo.SaveChangesAsync();

          return Ok(Mapper.Map<EventInfoViewModel>(info));
        }
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to upsert EventInfo. {0}", ex);
      }

      return BadRequest("Failed to update event.");
    }


    [HttpPut("{moniker}/location")]
    public async Task<IActionResult> UpdateLocation(string moniker, [FromBody]EventLocationViewModel vm)
    {
      try
      {
        var info = _repo.GetEventInfo(moniker);
        if (info == null)
        {
          return BadRequest("Cannot update a location on a missing event");
        }
        else
        {
          Mapper.Map(vm, info.Location);
        }

        _repo.AddOrUpdate(info.Location);
        await _repo.SaveChangesAsync();

        return Ok(Mapper.Map<EventLocationViewModel>(info.Location));
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to update location. {0}", ex);
      }

      return BadRequest("Failed to save new location.");
    }
  }
}
