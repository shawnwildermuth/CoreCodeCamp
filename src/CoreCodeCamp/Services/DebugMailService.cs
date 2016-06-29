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

    public DebugMailService(ILogger<DebugMailService> logger)
    {
      _logger = logger;
    }

    public Task SendMailAsync(string name, string email, string subject, string msg)
    {
      _logger.LogInformation($"Mail Sending to {name}/{email} for {subject}: {Environment.NewLine}{msg}");
      return Task.FromResult(0);
    }
  }
}
