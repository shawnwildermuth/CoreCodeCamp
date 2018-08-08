using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Controllers.Web;
using CoreCodeCamp.Data;
using CoreCodeCamp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodeCamp.Areas.Admin.Controllers
{
  [Authorize(Roles = Consts.ADMINROLE)]
  [Area("Admin")]
  public class RootController : MonikerControllerBase
  {
    public RootController(ICodeCampRepository repo, ILogger<RootController> logger, IMapper mapper)
     : base(repo, logger, mapper)
    {

    }

    [HttpGet("[area]")]
    public IActionResult Index()
    {
      return View(_repo.GetAllEventInfo());
    }

    [HttpGet("[area]/users")]
    public IActionResult Users()
    {
      return View();
    }

    [HttpGet("{moniker}/[area]/schedule")]
    public IActionResult Schedule(string moniker)
    {
      return View();
    }

    [HttpGet("{moniker}/[area]/sponsors")]
    public IActionResult Sponsors(string moniker)
    {
      return View();
    }

    [HttpGet("{moniker}/[area]/eventinfo")]
    public IActionResult EventInfo(string moniker)
    {
      return View();
    }

    [HttpGet("{moniker}/[area]/speakerlist")]
    public FileContentResult SpeakerList(string moniker)
    {
      var speakers = _repo.GetSpeakers(moniker).Where(s => s.Talks.Count(t => t.Approved) > 0).ToList();

      var csv = new StringBuilder();
      csv.AppendLine("\"Name\",\"Email\",\"CompanyName\",\"PhoneNumber\",\"TwitterHandle\",\"TShirtSize\"");
      foreach (var s in speakers)
      {
        csv.Append($@"""{s.Name}"",");
        csv.Append($@"""{s.UserName}"",");
        csv.Append($@"""{s.CompanyName}"",");
        csv.Append($@"""{s.PhoneNumber}"",");
        csv.Append($@"""{s.Twitter}"",");
        csv.Append($@"""{s.TShirtSize}""");
        csv.AppendLine();
      }

      return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "SpeakerList.csv");
    }

    [HttpGet("{moniker}/[area]/submissionlist")]
    public FileContentResult SubmissionList(string moniker)
    {
      var talks = _repo.GetTalks(moniker).ToList();

      var csv = new StringBuilder();
      csv.AppendLine(@"""Title"",""SpeakerName"",""SpeakerCompanyName"",""SpeakerPhoneNumber"",""SpeakerTwitterHandle"",""SpeakerTitle"",""SpeakerWebsite"",""SpeakerBlog"",""TShirtSize"",""Audience"",""Category"",""Level"",""Prerequisites"",""Approved"",""Abstract""");
      foreach (var t in talks)
      {
        csv.Append($@"""{t.Title}"",");
        csv.Append($@"""{t.Speaker.Name}"",");
        csv.Append($@"""{t.Speaker.CompanyName}"",");
        csv.Append($@"""{t.Speaker.PhoneNumber}"",");
        csv.Append($@"""{t.Speaker.Twitter}"",");
        csv.Append($@"""{t.Speaker.Title}"",");
        csv.Append($@"""{t.Speaker.Website}"",");
        csv.Append($@"""{t.Speaker.Blog}"",");
        csv.Append($@"""{t.Speaker.TShirtSize}"",");
        csv.Append($@"""{t.Audience}"",");
        csv.Append($@"""{t.Category}"",");
        csv.Append($@"""{t.Level}"",");
        csv.Append($@"""{t.Prerequisites}"",");
        csv.Append($@"""{t.Approved}"",");
        csv.Append($@"""{t.Abstract.Replace(Environment.NewLine, " ")}""");
        csv.AppendLine();
      }

      return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "SubmissionList.csv");
    }
  }
}
