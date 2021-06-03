using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoreCodeCamp.Controllers.Web
{
  public abstract class MonikerControllerBase : Controller
  {
    protected ICodeCampRepository _repo;
    protected ILogger _logger;
    protected IMapper _mapper;
    protected EventInfo _theEvent;

    public MonikerControllerBase(ICodeCampRepository repo, ILogger logger, IMapper mapper)
    {
      _repo = repo;
      _logger = logger;
      _mapper = mapper;
    }

    public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      using (var scope = this.HttpContext.RequestServices.CreateScope())
      {
        var repo = scope.ServiceProvider.GetService<ICodeCampRepository>();

        if (context.RouteData.Values.ContainsKey("moniker"))
        {
          var moniker = context.RouteData.Values["moniker"] as string;

          if (!context.HttpContext.Items.ContainsKey(Consts.EVENT_INFO_ITEM))
          {
            _theEvent = await repo.GetEventInfoAsync(moniker);
          }
          else
          {
            _theEvent = (EventInfo)context.HttpContext.Items[Consts.EVENT_INFO_ITEM];
            if (_theEvent.Moniker != moniker)
            {
              _theEvent = await repo.GetEventInfoAsync(moniker);
            }
          }

        }
        else
        {
          _theEvent = await repo.GetCurrentEventAsync();
        }
      }
      if (_theEvent == null) context.HttpContext.Response.Redirect("/");
      else context.HttpContext.Items[Consts.EVENT_INFO_ITEM] = _theEvent;

      await base.OnActionExecutionAsync(context, next);
      
      return;
    }

  }
}
