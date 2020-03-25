using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CoreCodeCamp.Extensions
{
  public static class HtmlHelperExtensions
  {
    public static HtmlString GetMoniker<T>(this IHtmlHelper<T> html)
    {
      return new HtmlString(html.ViewContext.RouteData.Values["moniker"] as string);
    }

    public static bool HasView<dynamic>(this IHtmlHelper<dynamic> html, string viewName, ICompositeViewEngine engine)
    {
      var result = engine.FindView(html.ViewContext, viewName, false);
      return result.Success;
    }

    public static HtmlString GetActiveMenuItem<T>(this IHtmlHelper<T> html, string controller, string action)
    {
      if (html.ViewContext.RouteData.Values.ContainsKey("controller") &&
        html.ViewContext.RouteData.Values.ContainsKey("action") &&
        html.ViewContext.RouteData.Values["controller"].Equals(controller) &&
        html.ViewContext.RouteData.Values["action"].Equals(action) &&
        html.ViewContext.RouteData.Values["area"] == null)
      {
        return new HtmlString("active");
      }

      return new HtmlString("");
    }

    public static HtmlString MakeUrlAbsolute<T>(this IHtmlHelper<T> html, string url)
    {
      if (url.ToLower().StartsWith("http://") ||
        url.ToLower().StartsWith("https://"))
      {
        return new HtmlString(url);
      }

      return new HtmlString(string.Concat("http://", url));
    }
  }
}
