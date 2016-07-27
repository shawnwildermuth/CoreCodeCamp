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

    [HttpPost("")]
    public async Task<IActionResult> Upsert([FromBody]EventInfoViewModel vm)
    {
      try
      {
        var info = _repo.GetEventInfo(vm.Moniker);
        if (info == null)
        {
          info = Mapper.Map<EventInfo>(vm);
        }
        else
        {
          Mapper.Map(vm, info);
        }

        _repo.AddOrUpdate(info);
        await _repo.SaveChangesAsync();

        return Ok(Mapper.Map<EventInfoViewModel>(info));
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to upsert EventInfo. {0}", ex);
      }

      return BadRequest("Failed to save new event.");
    }
  }
}
