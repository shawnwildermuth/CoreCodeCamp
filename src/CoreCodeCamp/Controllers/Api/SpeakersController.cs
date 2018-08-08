using System;
using System.Collections.Generic;
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
  [Route("{moniker}/api/speakers")]
  [ApiController]
  public class SpeakersController : Controller
  {
    private ICodeCampRepository _repo;
    private ILogger<SpeakersController> _logger;
    private IMailService _mailService;
    private UserManager<CodeCampUser> _userMgr;
    private readonly IMapper _mapper;

    public SpeakersController(ICodeCampRepository repo, 
      ILogger<SpeakersController> logger, 
      IMailService mailService, 
      UserManager<CodeCampUser> userMgr,
      IMapper mapper)
    {
      _repo = repo;
      _logger = logger;
      _mailService = mailService;
      _userMgr = userMgr;
      _mapper = mapper;
    }

    [HttpGet("")]
    public ActionResult<IEnumerable<Speaker>> Get(string moniker)
    {
      try
      {
        return Ok(_mapper.Map<IEnumerable<SpeakerViewModel>>(_repo.GetSpeakers(moniker)));
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to get speakers: {0}", ex);
      }

      return BadRequest("Failed to get Speakers");
    }

    [HttpGet("me")]
    [Authorize]
    public ActionResult<SpeakerViewModel> GetCurrent(string moniker)
    {
      try
      {
        var speaker = _repo.GetSpeakerForCurrentUser(moniker, User.Identity.Name);
        if (speaker == null)
        {
          speaker = new Speaker()
          {
            Talks = new List<Talk>(),
            UserName = User.Identity.Name
          };
        }
        return _mapper.Map<SpeakerViewModel>(speaker);
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to get current speaker: {0}", ex);
      }

      return BadRequest("Failed to get Speakers");
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(string moniker, int id)
    {
      try
      {
        return Ok(_mapper.Map<SpeakerViewModel>(_repo.GetSpeaker(id)));
      }
      catch (Exception ex)
      {
        _logger.LogError("Failed to get speaker: {0}", ex);
      }

      return BadRequest("Failed to get Speakers");
    }

    [HttpPost("me")]
    [Authorize]
    public async Task<IActionResult> UpsertMySpeaker(string moniker, [FromBody]SpeakerViewModel model)
    {
      return await UpsertSpeaker(model, _repo.GetSpeakerForCurrentUser(moniker, User.Identity.Name), moniker, User.Identity.Name);
    }

    [HttpPost("")]
    [Authorize]
    public async Task<IActionResult> UpsertSpeaker(string moniker, [FromBody]SpeakerViewModel model)
    {
      return await UpsertSpeaker(model, _repo.GetSpeakerByName(moniker, model.Name), moniker, User.Identity.Name);
    }

    async Task<IActionResult> UpsertSpeaker(SpeakerViewModel model, Speaker speaker, string moniker, string userName)
    {
      if (ModelState.IsValid)
      {
        try
        {
          if (speaker == null)
          {
            speaker = _mapper.Map<Speaker>(model);
            speaker.UserName = userName;
            speaker.Event = _repo.GetEventInfo(moniker);

            // Send confirmation email on new speaker
            var user = await _userMgr.FindByNameAsync(userName);
            var speakerUrl = this.Url.Link("MySpeakerPage", new { moniker = moniker });
            await _mailService.SendTemplateMailAsync(
              "SpeakerSignUp",
              new SpeakerModel()
              {
                Name = user.Name,
                Email = user.Email,
                Subject = $"Speaking at the {speaker.Event.Name}",
                Speaker = speaker,
                SpeakerUrl = speakerUrl,
                Event = speaker.Event
              });
          }
          else
          {
            _mapper.Map<SpeakerViewModel, Speaker>(model, speaker);
          }

          _repo.AddOrUpdate(speaker);
          await _repo.SaveChangesAsync();

          return Ok(_mapper.Map<SpeakerViewModel>(speaker));
        }
        catch (Exception ex)
        {
          _logger.LogError("Failed to get update speaker: {0}", ex);
          ModelState.AddModelError("", $"Failed to Save: {ex.Message}");
        }
      }
      _logger.LogError("Failed to get update speaker because of bad Model State: {0}", ModelState);
      return BadRequest($"Failed to save Speaker: ModelState has Errors: #{ModelState.ErrorCount}");

    }
  }
}
