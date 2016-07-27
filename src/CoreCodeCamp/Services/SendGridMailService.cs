using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoreCodeCamp.Services
{
  public class SendGridMailService : IMailService
  {
    private IConfigurationRoot _config;
    private IHostingEnvironment _env;
    private ILogger<SendGridMailService> _logger;
    private IEmailTemplateEngine _templateEngine;

    public SendGridMailService(IHostingEnvironment env, 
      IConfigurationRoot config, 
      ILogger<SendGridMailService> logger,
      IEmailTemplateEngine templateEngine)
    {
      _env = env;
      _config = config;
      _logger = logger;
      _templateEngine = templateEngine;
    }

    public async Task SendMailAsync(string name, string email, string subject, string msg)
    {
      try
      {
        var key = _config["MailService:ApiKey"];

        var uri = $"https://api.sendgrid.com/api/mail.send.json";
        var post = new KeyValuePair<string, string>[]
              {
                new KeyValuePair<string, string>("api_user", _config["MailService:ApiUser"]),
                new KeyValuePair<string, string>("api_key", _config["MailService:ApiKey"]),
                new KeyValuePair<string, string>("to", email),
                new KeyValuePair<string, string>("toname", name),
                new KeyValuePair<string, string>("subject", subject),
                new KeyValuePair<string, string>("html", msg),
                new KeyValuePair<string, string>("from", _config["MailService:FromEmail"])
              };

        var client = new HttpClient();
        var response = await client.PostAsync(uri, new FormUrlEncodedContent(post));
        if (!response.IsSuccessStatusCode)
        {
          var result = await response.Content.ReadAsStringAsync();
          _logger.LogError($"Failed to send message via SendGrid: {Environment.NewLine}Body: {post}{Environment.NewLine}Result: {result}");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError("Exception Thrown sending message via SendGrid", ex);
      }
    }

    public async Task SendTemplateMailAsync(string name, string email, string subject, string templateName, params object[] args)
    {
      var parameters = args.Union(new object[] { name, email }).ToArray();
      var body = _templateEngine.GenerateTemplate(templateName, parameters);
      await SendMailAsync(name, email, subject, body);
    }
  }
}
