using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Areas.Admin.Controllers
{
  [Authorize(Roles = Consts.AdminRole)]
  [Area("Admin")]
  public class RootController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Users()
    {
      return View();
    }
  }
}
