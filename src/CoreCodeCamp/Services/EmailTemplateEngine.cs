using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace CoreCodeCamp.Services
{
  public class EmailTemplateEngine : IEmailTemplateEngine
  {
    private IHostingEnvironment _env;

    public EmailTemplateEngine(IHostingEnvironment env)
    {
      _env = env;
    }

    public string GenerateTemplate(string templateName, params object[] args)
    {
      var path = Path.Combine(_env.ContentRootPath, $"EmailTemplates/{templateName}.htm");
      var layoutPath = Path.Combine(_env.ContentRootPath, $"EmailTemplates/_Layout.htm");
      var layout = File.ReadAllText(layoutPath);
      var template = File.ReadAllText(path);
      return string.Format(layout, templateName, string.Format(template, args));
    }
  }
}
