using System.Threading.Tasks;

namespace CoreCodeCamp.Services
{
  public interface IMailService
  {
    Task SendMailAsync(string name, string email, string subject, string msg);
    Task SendTemplateMailAsync(string name, string email, string subject, string templateName, params object[] args);
  }
}