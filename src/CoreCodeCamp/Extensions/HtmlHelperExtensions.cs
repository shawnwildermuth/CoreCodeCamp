using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CoreCodeCamp.Extensions
{
  public static class HtmlHelperExtensions
  {
    public static HtmlString GetMoniker<T>(this IHtmlHelper<T> html) 
    {
      return new HtmlString(html.ViewContext.RouteData.Values["moniker"] as string);
    }

    public static HtmlString GetActiveMenuItem<T>(this IHtmlHelper<T> html, string controller, string action)
    {
      if (html.ViewContext.RouteData.Values["controller"].Equals(controller) && 
        html.ViewContext.RouteData.Values["action"].Equals(action) &&
        html.ViewContext.RouteData.Values["area"] == null)
      {
        return new HtmlString("active");
      }

      return new HtmlString("");
    }
  }
}
