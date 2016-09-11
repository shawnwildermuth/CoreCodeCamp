using System.Threading.Tasks;
using CoreCodeCamp.Models.Emails;

namespace CoreCodeCamp.Services
{
  public interface IMailService
  {
    Task SendMailAsync(string name, string email, string subject, string msg);
    Task SendTemplateMailAsync<T>(string templateName, T model) where T : EmailModel;
  }
}