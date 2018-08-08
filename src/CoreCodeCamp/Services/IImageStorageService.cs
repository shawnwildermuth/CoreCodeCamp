using System.Threading.Tasks;

namespace CoreCodeCamp.Services
{
  public interface IImageStorageService
  {
    Task<string> StoreImage(string filename, byte[] image);
  }
}