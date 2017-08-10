using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Models.Emails;
using Microsoft.Extensions.Logging;

namespace CoreCodeCamp.Services
{
  public class DebugMailService : IMailService
  {
    private ILogger<DebugMailService> _logger;
    private ViewRenderer _renderer;

    public DebugMailService(ILogger<DebugMailService> logger, ViewRenderer renderer)
    {
      _logger = logger;
      _renderer = renderer;
    }

    public Task<bool> SendMailAsync(string name, string email, string subject, string msg)
    {
      _logger.LogInformation($"Mail Sending to {name}/{email} for {subject}: {Environment.NewLine}{msg}");
      return Task.FromResult(true);
    }

    public async Task<bool> SendTemplateMailAsync<T>(string templateName, T model) where T : EmailModel
    {
      var body = await _renderer.RenderAsync($"Emails/{templateName}", model);
      return await SendMailAsync(model.Name, model.Email, model.Subject, body);
    }
  }
}
