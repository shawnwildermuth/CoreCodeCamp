using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CoreCodeCamp.Controllers.Web
{
  public abstract class MonikerControllerBase : Controller
  {
    protected ICodeCampRepository _repo;
    protected ILogger _logger;
    protected EventInfo _theEvent;

    public MonikerControllerBase(ICodeCampRepository repo, ILogger logger)
    {
      _repo = repo;
      _logger = logger;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
      base.OnActionExecuting(context);

      if (!context.HttpContext.Items.ContainsKey(Consts.EVENT_INFO_ITEM))
      {
        if (context.RouteData.Values.ContainsKey("moniker"))
        {
          _theEvent = _repo.GetEventInfo(context.RouteData.Values["moniker"] as string);
        }
        else
        {
          _theEvent = _repo.GetCurrentEvent();
        }

        // Put the current event in scope data
        context.HttpContext.Items[Consts.EVENT_INFO_ITEM] = _theEvent;
      }
    }


  }
}
