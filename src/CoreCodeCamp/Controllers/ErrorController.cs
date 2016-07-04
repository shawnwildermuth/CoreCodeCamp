using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers
{
  [Route("~/[controller]/")]
  public class ErrorController : Controller
  {
    [HttpGet("Exception")]
    public IActionResult Exception()
    {
      return View();
    }

    [HttpGet("404")]
    public IActionResult FileNotFound()
    {
      return View();
    }

    [HttpGet("403")]
    public IActionResult Forbidden()
    {
      return View();
    }
  }
}
