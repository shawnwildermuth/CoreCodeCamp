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

namespace CoreCodeCamp.Controllers.Api
{
  [Route("{moniker}/api/speakers")]
  public class SpeakersController : Controller
  {
    private ICodeCampRepository _repo;

    public SpeakersController(ICodeCampRepository repo)
    {
      _repo = repo;
    }

    [HttpGet("")]
    public IActionResult Get(string moniker)
    {
      try
      {
        return Ok(Mapper.Map<IEnumerable<SpeakerViewModel>>(_repo.GetSpeakers(moniker)));
      }
      catch
      {

      }

      return BadRequest("Failed to get Speakers");
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrent(string moniker)
    {
      try
      {
        var speaker = _repo.GetSpeakerForCurrentUser(moniker, User.Identity.Name);
        if (speaker == null) speaker = new Speaker();
        return Ok(Mapper.Map<SpeakerViewModel>(speaker));
      }
      catch
      {

      }

      return BadRequest("Failed to get Speakers");
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(string moniker, int id)
    {
      try
      {
        return Ok(Mapper.Map<SpeakerViewModel>(_repo.GetSpeaker(id)));
      }
      catch
      {

      }

      return BadRequest("Failed to get Speakers");
    }

    [HttpPost("")]
    [Authorize]
    public async Task<IActionResult> UpsertSpeaker(string moniker, [FromBody]SpeakerViewModel model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var speaker = _repo.GetSpeakerForCurrentUser(moniker, User.Identity.Name);

          if (speaker == null)
          {
            speaker = Mapper.Map<Speaker>(model);
            speaker.UserName = User.Identity.Name;
            speaker.Event = _repo.GetEventInfo(moniker);
          }
          else
          {
            Mapper.Map<SpeakerViewModel, Speaker>(model, speaker);
          }

          _repo.AddOrUpdate(speaker);
          await _repo.SaveChangesAsync();

          return Ok(Mapper.Map<SpeakerViewModel>(speaker));
        }
        catch (Exception ex)
        {
          ModelState.AddModelError("", $"Failed to Save: {ex.Message}");
        }
      }

      return BadRequest("Failed to save Speaker");
    }

  }
}
