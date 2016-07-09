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
    public static HtmlString GetMoniker(this IHtmlHelper<dynamic> html)
    {
      return new HtmlString(html.ViewContext.RouteData.Values["moniker"] as string);
    }
  }
}
