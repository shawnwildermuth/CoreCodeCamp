using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data;
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

      // Put the current event in scope data
      context.HttpContext.Items["EventInfo"] = _repo.GetEventInfo(context.RouteData.Values["moniker"] as string);
    }


  }
}
