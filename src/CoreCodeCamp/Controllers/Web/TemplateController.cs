using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Controllers.Web
{
  [Route("templates")]
  public class TemplateController : Controller
  {

    [HttpGet("join")]
    public IActionResult Join()
    {
      return View();
    }
  }
}
