using System.Threading.Tasks;
using CoreCodeCamp.Models.Emails;

namespace CoreCodeCamp.Services
{
  public interface IMailService
  {
    Task<bool> SendMailAsync(string name, string email, string subject, string msg);
    Task<bool> SendTemplateMailAsync<T>(string templateName, T model) where T : EmailModel;
  }
}