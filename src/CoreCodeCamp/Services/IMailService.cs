using System.Threading.Tasks;

namespace CoreCodeCamp.Services
{
  public interface IMailService
  {
    Task SendMailAsync(string name, string email, string subject, string msg);
  }
}