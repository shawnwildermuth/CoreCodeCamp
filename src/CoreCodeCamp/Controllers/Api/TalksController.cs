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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers.Api
{
  [Authorize]
  public class TalksController : Controller
  {
    private ICodeCampRepository _repo;
    private UserManager<CodeCampUser> _userMgr;

    public TalksController(ICodeCampRepository repo, UserManager<CodeCampUser> userMgr)
    {
      _repo = repo;
      _userMgr = userMgr;

    }

    [HttpGet("{moniker}/api/talks")]
    [AllowAnonymous]
    public IActionResult Get(string moniker)
    {
      try
      {
        return Ok(Mapper.Map<IEnumerable<TalkViewModel>>(_repo.GetTalks(moniker)));
      }
      catch
      {

      }

      return BadRequest("Couldn't load talks.");
    }

    [HttpGet("{moniker}/api/talks/me")]
    public IActionResult GetMyTalks(string moniker)
    {
      try
      {
        var speaker = _repo.GetSpeakerForCurrentUser(moniker, User.Identity.Name);
        return Ok(Mapper.Map<IEnumerable<TalkViewModel>>(speaker.Talks));
      }
      catch
      {

      }

      return BadRequest("Couldn't load talks.");
    }


    [HttpGet("{moniker}/api/talks/{id}")]
    [AllowAnonymous]
    public IActionResult Get(string moniker, int id)
    {
      try
      {
        return Ok(Mapper.Map<TalkViewModel>(_repo.GetTalk(id)));
      }
      catch
      {

      }

      return BadRequest("Couldn't load talk.");
    }

    [HttpPut("{moniker}/api/talks/{id}/toggleapproved")]
    public async Task<IActionResult> ToggleApproved(string moniker, int id)
    {
      try
      {
        var talk = _repo.GetTalk(id);
        if (talk.Speaker.Event.Moniker != moniker) return BadRequest("Bad Event for this Talk");

        talk.Approved = !talk.Approved;
        await _repo.SaveChangesAsync();

        return Ok();
      }
      catch
      {

      }

      return BadRequest("Could not change Approved.");
    }

    [HttpGet("{moniker}/api/speakers/{id:int}/talks")]
    [AllowAnonymous]
    public IActionResult GetSpeakerTalks(string moniker, int id)
    {
      try
      {
        var speaker = _repo.GetSpeaker(id);
        foreach (var t in speaker.Talks) t.Speaker = null; // Trim Speaker when returning just the talks

        return Ok(Mapper.Map<IEnumerable<TalkViewModel>>(speaker.Talks));
      }
      catch
      {

      }

      return BadRequest("Couldn't load talk.");
    }

    [HttpPost("{moniker}/api/speakers/{id:int}/talks")]
    public async Task<IActionResult> UpsertTalk(string moniker, int id, [FromBody]TalkViewModel vm)
    {
      if (ModelState.IsValid)
      {
        return await UpsertTalk(_repo.GetSpeaker(id), moniker, vm);
      }
      return BadRequest("Couldn't Save");
    }

    async Task<IActionResult> UpsertTalk(Speaker speaker, string moniker, TalkViewModel vm)
    {
      try
      {
        Talk talk = speaker.Talks.Where(t => t.Id == vm.Id).FirstOrDefault();
        if (talk == null)
        {
          talk = Mapper.Map<Talk>(vm);
          speaker.Talks.Add(talk);
        }
        else
        {
          talk = Mapper.Map(vm, talk);
        }

        await _repo.SaveChangesAsync();

        return Ok(Mapper.Map<TalkViewModel>(talk));
      }
      catch
      {

      }

      return BadRequest("Couldn't save or update talk.");

    }

    [HttpPost("{moniker}/api/speakers/me/talks")]
    public async Task<IActionResult> UpsertMyTalk(string moniker, [FromBody]TalkViewModel vm)
    {
      if (ModelState.IsValid)
      {
        return await UpsertTalk(_repo.GetSpeakerForCurrentUser(moniker, User.Identity.Name), moniker, vm);
      }
      return BadRequest("Couldn't Save");
    }

    [HttpDelete("{moniker}/api/talks/{id:int}")]
    public async Task<IActionResult> Delete(string moniker, int id)
    {
      try
      {
        var talk = _repo.GetTalk(id);
        if (talk != null)
        {
          _repo.Delete(talk);
          await _repo.SaveChangesAsync();
          return Ok();
        }
        else
        {
          return NotFound();
        }
      }
      catch
      {

      }

      return BadRequest("Couldn't save or update talk.");
    }

  }
}
