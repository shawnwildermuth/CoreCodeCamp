using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models;
using CoreCodeCamp.Models.Emails;
using CoreCodeCamp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodeCamp.Controllers.Api
{
  [Authorize]
  [ApiController]
  public class TalksController : Controller
  {
    private ICodeCampRepository _repo;
    private UserManager<CodeCampUser> _userMgr;
    private ILogger<TalksController> _logger;
    private IMailService _mailService;
    private readonly IMapper _mapper;

    public TalksController(ICodeCampRepository repo, 
      UserManager<CodeCampUser> userMgr, 
      ILogger<TalksController> logger, 
      IMailService mailService,
      IMapper mapper)
    {
      _logger = logger;
      _repo = repo;
      _userMgr = userMgr;
      _mailService = mailService;
      _mapper = mapper;
    }

    [HttpGet("{moniker}/api/talks")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(string moniker)
    {
      try
      {
        var talks = _mapper.Map<IEnumerable<TalkViewModel>>(await _repo.GetTalksAsync(moniker));

        // Update Vote Counts
        var counts = await _repo.GetTalkCountsAsync(moniker);
        foreach (var t in talks)
        {
          var result = counts.Where(c => c.Item1.Id == t.Id).FirstOrDefault();
          if (result != null)
          {
            t.Votes = result.Item2;
          }
        }

        return Ok(talks);
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to get talks: {0}", ex);
      }

      return BadRequest("Couldn't load talks.");
    }

    [HttpGet("{moniker}/api/talks/me")]
    public async Task<IActionResult> GetMyTalks(string moniker)
    {
      try
      {
        var speaker = await _repo.GetSpeakerForCurrentUserAsync(moniker, User.Identity.Name);
        if (speaker != null && speaker.Talks != null)
        {
          return Ok(_mapper.Map<IEnumerable<TalkViewModel>>(speaker.Talks));
        }
        return Ok(_mapper.Map<IEnumerable<TalkViewModel>>(new List<Talk>()));
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to get my talks: {0}", ex);
      }

      return BadRequest("Couldn't load talks.");
    }


    [HttpGet("{moniker}/api/talks/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(string moniker, int id)
    {
      try
      {
        return Ok(_mapper.Map<TalkViewModel>(await _repo.GetTalkAsync(id)));
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to get individual talk: {0}", ex);
      }

      return BadRequest("Couldn't load talk.");
    }

    [HttpPut("{moniker}/api/talks/{id}/toggleapproved")]
    public async Task<IActionResult> ToggleApproved(string moniker, int id)
    {
      try
      {
        var talk = await _repo.GetTalkAsync(id);
        if (talk.Speaker.Event.Moniker != moniker) return BadRequest("Bad Event for this Talk");

        talk.Approved = !talk.Approved;
        await _repo.SaveChangesAsync();

        if (talk.Approved)
        {
          var user = await _userMgr.FindByNameAsync(talk.Speaker.UserName);
          if (user != null)
          {
            var speakerUrl = this.Url.Link("SpeakerTalkPage", new { moniker = moniker, id = talk.Speaker.Slug });
            await _mailService.SendTemplateMailAsync(
              "TalkAcceptance",
              new TalkModel()
              {
                Name = user.Name,
                Email = user.Email,
                Subject = $"Invited to Speak at the {talk.Speaker.Event.Name}",
                Talk = talk,
                SpeakerUrl = speakerUrl,
                Moniker = moniker
              });
          }
        }

        return Ok(talk.Approved);
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to toggle the 'approved' talk: {0}", ex);
      }

      return BadRequest("Could not change Approved.");
    }

    [HttpPut("{moniker}/api/talks/{id}/unassign")]
    public async Task<IActionResult> Unassign(string moniker, int id)
    {
      try
      {
        var talk = await _repo.GetTalkAsync(id);
        if (talk.Speaker.Event.Moniker != moniker) return BadRequest("Bad Event for this Talk");

        talk.TimeSlot = null;
        talk.Room = null;
        talk.Track = null;
        await _repo.SaveChangesAsync();

        return Ok(true);
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to toggle the 'approved' talk: {0}", ex);
      }

      return BadRequest("Could not change Approved.");
    }


    [HttpGet("{moniker}/api/speakers/{id:int}/talks")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSpeakerTalks(string moniker, int id)
    {
      try
      {
        var speaker = await _repo.GetSpeakerAsync(id);
        foreach (var t in speaker.Talks) t.Speaker = null; // Trim Speaker when returning just the talks

        return Ok(_mapper.Map<IEnumerable<TalkViewModel>>(speaker.Talks));
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to get speaker's talks: {0}", ex);
      }

      return BadRequest("Couldn't load talk.");
    }

    [HttpPost("{moniker}/api/speakers/{id:int}/talks")]
    public async Task<IActionResult> UpsertTalk(string moniker, int id, [FromBody]TalkViewModel vm)
    {
      if (ModelState.IsValid)
      {
        return await UpsertTalk(await _repo.GetSpeakerAsync(id), moniker, vm);
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
          talk = _mapper.Map<Talk>(vm);
          speaker.Talks.Add(talk);
        }
        else
        {
          talk = _mapper.Map(vm, talk);
        }

        await _repo.SaveChangesAsync();

        return Ok(_mapper.Map<TalkViewModel>(talk));
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to update talk: {0}", ex);
      }

      return BadRequest("Couldn't save or update talk.");

    }

    [HttpPost("{moniker}/api/speakers/me/talks")]
    public async Task<IActionResult> UpsertMyTalk(string moniker, [FromBody]TalkViewModel vm)
    {
      if (ModelState.IsValid)
      {
        return await UpsertTalk(await _repo.GetSpeakerForCurrentUserAsync(moniker, User.Identity.Name), moniker, vm);
      }
      return BadRequest("Couldn't Save");
    }


    [HttpPut("{moniker}/api/talks/{id:int}/room")]
    public async Task<IActionResult> UpdateRoom(string moniker, int id, [FromBody]TalkViewModel model)
    {
      try
      {
        var talk = await _repo.GetTalkAsync(id);
        var room = (await _repo.GetRoomsAsync(moniker)).Where(r => r.Name == model.Room).FirstOrDefault();
        if (room == null || talk == null) return NotFound("Cannot find talk.");
        talk.Room = room;

        await _repo.SaveChangesAsync();
        return Ok(true);

      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to update room on talk: {0}", ex);
      }

      return BadRequest("Couldn't update talk.");
    }

    [HttpPut("{moniker}/api/talks/{id:int}/time")]
    public async Task<IActionResult> UpdateTime(string moniker, int id, [FromBody]TalkViewModel model)
    {
      try
      {
        var talk = await _repo.GetTalkAsync(id);
        var time = (await _repo.GetTimeSlotsAsync(moniker)).Where(r => r.Time == model.Time).FirstOrDefault();
        if (time == null || talk == null) return NotFound("Cannot find talk.");
        talk.TimeSlot = time;

        await _repo.SaveChangesAsync();
        return Ok(true);

      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to update talk time: {0}", ex);
      }

      return BadRequest("Couldn't update talk.");
    }

    [HttpPut("{moniker}/api/talks/{id:int}/track")]
    public async Task<IActionResult> UpdateTrack(string moniker, int id, [FromBody]TalkViewModel model)
    {
      try
      {
        var talk = await _repo.GetTalkAsync(id);
        var track = (await _repo.GetTracksAsync(moniker)).Where(r => r.Name == model.Track).FirstOrDefault();
        if (track == null || talk == null) return NotFound("Cannot find talk.");
        talk.Track = track;

        await _repo.SaveChangesAsync();
        return Ok(true);

      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to update track: {0}", ex);
      }

      return BadRequest("Couldn't update talk.");
    }


    [HttpDelete("{moniker}/api/talks/{id:int}")]
    public async Task<IActionResult> Delete(string moniker, int id)
    {
      try
      {
        var talk = await _repo.GetTalkAsync(id);
        if (talk != null)
        {
          _repo.Delete(talk);
          await _repo.SaveChangesAsync();
          return Ok(true);
        }
        else
        {
          return NotFound();
        }
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to delete talk: {0}", ex);
      }

      return BadRequest("Couldn't delete talk.");
    }

  }
}
