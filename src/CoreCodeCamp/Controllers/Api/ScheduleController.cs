using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodeCamp.Controllers.Api
{
  public class ScheduleController : Controller
  {
    private ILogger<ScheduleController> _logger;
    private ICodeCampRepository _repo;

    public ScheduleController(ICodeCampRepository repo, ILogger<ScheduleController> logger)
    {
      _repo = repo;
      _logger = logger;
    }



  }
}
