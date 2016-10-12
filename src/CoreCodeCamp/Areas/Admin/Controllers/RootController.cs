using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public RootController(ICodeCampRepository repo, ILogger<RootController> logger)
     : base(repo, logger)
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
      csv.AppendLine("\"Name\",\"Email\",\"CompanyName\",\"PhoneNumber\",\"TwitterHandle\"");
      foreach (var s in speakers)
      {
        csv.Append($@"""{s.Name}"",");
        csv.Append($@"""{s.UserName}"",");
        csv.Append($@"""{s.CompanyName}"",");
        csv.Append($@"""{s.PhoneNumber}"",");
        csv.Append($@"""{s.Twitter}"",");
        csv.AppendLine();
      }

      return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "SpeakerList.csv");
    }
  }
}
