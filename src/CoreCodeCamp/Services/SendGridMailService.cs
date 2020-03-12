using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CoreCodeCamp.Models.Emails;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;

namespace CoreCodeCamp.Services
{
  public class SendGridMailService : IMailService
  {
    private IConfiguration _config;
    private IWebHostEnvironment _env;
    private ILogger<SendGridMailService> _logger;
    private ViewRenderer _renderer;

    public SendGridMailService(IWebHostEnvironment env, 
      IConfiguration config, 
      ILogger<SendGridMailService> logger,
      ViewRenderer renderer)
    {
      _env = env;
      _config = config;
      _logger = logger;
      _renderer = renderer;
    }

    public async Task<bool> SendMailAsync(string name, string email, string subject, string msg)
    {
      try
      {
        var key = _config["MailService:ApiKey"];
        var client = new SendGridClient(key);
        var mail = MailHelper.CreateSingleEmail(new EmailAddress(_config["MailService:FromEmail"]),
          new EmailAddress(email),
          subject,
          "",
          msg);

        var response = await client.SendEmailAsync(mail);
        if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted)
        {
          var result = await response.Body.ReadAsStringAsync();
          _logger.LogError($"Failed to send message via SendGrid: {Environment.NewLine}Key: {key}{Environment.NewLine}Result: {result}");
          return false;
        }
      }
      catch (Exception ex)
      {
        _logger.LogError("Exception Thrown sending message via SendGrid", ex);
        return false;
      }

      return true;
    }

    public async Task<bool> SendTemplateMailAsync<T>(string templateName, T model) where T : EmailModel
    {
      try
      {
        var body = await _renderer.RenderAsync($"Emails/{templateName}", model);
        return await SendMailAsync(model.Name, model.Email, model.Subject, body);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Failed while rendering or sending template email: {ex}");
        return false;
      }
    }
  }
}
