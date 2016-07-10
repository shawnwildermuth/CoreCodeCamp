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

namespace CoreCodeCamp.Controllers.Web
{
  public abstract class MonikerControllerBase : Controller
  {
    protected ICodeCampRepository _repo;

    public MonikerControllerBase(ICodeCampRepository repo)
    {
      _repo = repo;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
      base.OnActionExecuting(context);

      if (!context.HttpContext.Items.ContainsKey(Consts.EVENT_INFO_ITEM))
      {
        EventInfo theEvent;

        if (context.RouteData.Values.ContainsKey("moniker"))
        {
          theEvent = _repo.GetEventInfo(context.RouteData.Values["moniker"] as string);
        }
        else
        {
          theEvent = _repo.GetCurrentEvent();
        }

        // Put the current event in scope data
        context.HttpContext.Items[Consts.EVENT_INFO_ITEM] = theEvent;
      }
    }


  }
}
