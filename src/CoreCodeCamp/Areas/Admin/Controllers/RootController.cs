using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Areas.Admin.Controllers
{
  [Authorize]
  [Area("Admin")]
  public class RootController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
