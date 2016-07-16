using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers.Api
{
  [Authorize]
  [Route("{moniker}/api/cfs")]
  public class CallForSpeakersApiController : Controller
  {
    private UserManager<CodeCampUser> _userMgr;
    private ICodeCampRepository _repo;
    private IHostingEnvironment _env;

    public CallForSpeakersApiController(ICodeCampRepository repo, UserManager<CodeCampUser> userMgr, IHostingEnvironment env)
    {
      _userMgr = userMgr;
      _repo = repo;
      _env = env;
    }

    [HttpGet("speaker")]
    public async Task<IActionResult> GetSpeaker(string moniker)
    {
      var speaker = _repo.GetSpeakerForCurrentUser(moniker, User.Identity.Name);
      if (speaker == null)
      {
        var user = await _userMgr.FindByNameAsync(User.Identity.Name);
        speaker = new Speaker()
        {
          Name = user.Name
        };
      }
      return Ok(Mapper.Map<SpeakerViewModel>(speaker));

    }


    [HttpPost("speaker")]
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

          return Ok("Success");
        }
        catch (Exception ex)
        {
          ModelState.AddModelError("", $"Failed to Save: {ex.Message}");
        }
      }

      return BadRequest("Failed to save Speaker");
    }

    [HttpPost("speaker/talk")]
    public async Task<IActionResult> UpsertTalk(string moniker, [FromBody]TalkViewModel model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var talk = _repo.GetTalk(model.Id);
          var isNew = (talk == null);

          if (isNew)
          {
            talk = Mapper.Map<Talk>(model);
            var speaker = _repo.GetSpeakerForCurrentUser(moniker, User.Identity.Name);
            speaker.Talks.Add(talk);
          }
          else
          {
            Mapper.Map<TalkViewModel, Talk>(model, talk);
          }

          _repo.AddOrUpdate(talk);
          await _repo.SaveChangesAsync();

          var result = Mapper.Map<TalkViewModel>(talk);

          if (isNew) return Created($"/{moniker}/api/cfs/speaker/talk/{result.Id}", result);
          return Ok(result);
        }
        catch (Exception ex)
        {
          ModelState.AddModelError("", $"Failed to Save: {ex.Message}");
        }
      }

      return BadRequest("Failed to save Talk");
    }

    [HttpDelete("speaker/talk/{id:int}")]
    public async Task<IActionResult> DeleteTalk(string moniker, int id)
    {
      try
      {
        var talk = _repo.GetTalk(id);

        _repo.Delete(talk);
        await _repo.SaveChangesAsync();

        return Ok();
      }
      catch (Exception ex)
      {
        ModelState.AddModelError("", $"Failed to Save: {ex.Message}");
      }

      return BadRequest("Failed to delete task");
    }
  }
}
