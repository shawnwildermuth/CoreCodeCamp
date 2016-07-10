using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Services;
using Microsoft.AspNetCore.Mvc.Razor;

namespace CoreCodeCamp.Extensions
{
  public static class RazorPageExtensions
  {
    public static EventInfo GetEventInfo(this RazorPage page)
    {
      var info = page.Context.Items[Consts.EVENT_INFO_ITEM] as EventInfo;
      if (info == null) info = new EventInfo();
      return info;
    }
  }
}
