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

      if (context.RouteData.Values.ContainsKey("moniker"))
      {
        var moniker = context.RouteData.Values["moniker"] as string;

        if (!context.HttpContext.Items.ContainsKey(Consts.EVENT_INFO_ITEM))
        {
          _theEvent = _repo.GetEventInfo(moniker);
        }
        else
        {
          _theEvent = (EventInfo)context.HttpContext.Items[Consts.EVENT_INFO_ITEM];
          if (_theEvent.Moniker != moniker)
          {
            _theEvent = _repo.GetEventInfo(moniker);
          }
        }

      }
      else
      {
        _theEvent = _repo.GetCurrentEvent();
      }

      if (_theEvent == null) context.HttpContext.Response.Redirect("/");
      else context.HttpContext.Items[Consts.EVENT_INFO_ITEM] = _theEvent;


    }


  }
}
