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
      var speaker = _repo.GetSpeaker(moniker, User.Identity.Name);
      if (speaker == null)
      {
        var user = await _userMgr.FindByNameAsync(User.Identity.Name);
        speaker = new Speaker()
        {
          Name = user.Name,
          ImageUrl = "/img/speaker-placeholder.jpg"
        };
      }
      return Ok(Mapper.Map<SpeakerViewModel>(speaker));

    }

    [HttpPost("speaker/headshot")]
    public async Task<IActionResult> PostHeadshot(string moniker)
    {
      if (!Request.Form.Files.Any())
      {
        return BadRequest("No Image supplied");
      }

      // Ensure it's a valid extension
      var extension = Path.GetExtension(Request.Form.Files[0].FileName).ToLower();
      if (!(new[] { ".jpg", ".png" }.Any(s => extension == s)))
      {
        return BadRequest("File msut be .jpg or .png");
      }

      // Get Path to the speaker directory
      var path = Path.Combine(_env.WebRootPath, "img", moniker, "Speakers");


      // Make sure the directory exists
      Directory.CreateDirectory(path);
      var filePath = Path.Combine(path, Request.Form.Files[0].FileName);

      // Ensure file doesn't exist
      while (System.IO.File.Exists(filePath))
      {
        filePath = Path.Combine(path, Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(filePath)));
      }

      // TODO Resize Image
      using (var newStream = ResizeImage(Request.Form.Files[0].OpenReadStream()))
      using (var stream = System.IO.File.Create(filePath))
      {
        // Write It
        await newStream.CopyToAsync(stream);

        // Calculate the URL
        var imageUrl = $"/img/{moniker}/Speakers/{Path.GetFileName(filePath)}";

        return Ok(imageUrl);
      }
    }

    private Stream ResizeImage(Stream stream)
    {
      MemoryStream outStream = new MemoryStream();

      using (ImageFactory imageFactory = new ImageFactory())
      {
        // Load, resize, set the format, and quality and save an image.
        imageFactory.Load(stream)
                    .Resize(new ResizeLayer(new Size(300, 300)))
                    .Format(new JpegFormat())
                    .Quality(70)
                    .BackgroundColor(Color.White)
                    .Save(outStream);
      }

      return outStream;
    }

    [HttpPost("speaker")]
    public async Task<IActionResult> UpsertSpeaker(string moniker, [FromBody]SpeakerViewModel model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var speaker = _repo.GetSpeaker(moniker, User.Identity.Name);

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
            var speaker = _repo.GetSpeaker(moniker, User.Identity.Name);
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
