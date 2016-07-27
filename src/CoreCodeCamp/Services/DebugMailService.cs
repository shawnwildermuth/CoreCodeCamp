using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CoreCodeCamp.Services
{
  public class DebugMailService : IMailService
  {
    private ILogger<DebugMailService> _logger;
    private IEmailTemplateEngine _templateEngine;

    public DebugMailService(ILogger<DebugMailService> logger, IEmailTemplateEngine templateEngine)
    {
      _logger = logger;
      _templateEngine = templateEngine;
    }

    public Task SendMailAsync(string name, string email, string subject, string msg)
    {
      _logger.LogInformation($"Mail Sending to {name}/{email} for {subject}: {Environment.NewLine}{msg}");
      return Task.FromResult(0);
    }

    public async Task SendTemplateMailAsync(string name, string email, string subject, string templateName, params object[] args)
    {
      var parameters = args.Union(new object[] { name, email }).ToArray();
      var body = _templateEngine.GenerateTemplate(templateName, parameters);
      await SendMailAsync(name, email, subject, body);
    }
  }
}
