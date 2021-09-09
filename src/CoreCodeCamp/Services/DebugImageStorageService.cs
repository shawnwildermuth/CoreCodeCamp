using System.IO;
using System.Threading.Tasks;
using WilderMinds.AzureImageStorageService;

namespace CoreCodeCamp.Services
{
  public class DebugImageStorageService : IAzureImageStorageService
  {

    public Task<ImageResponse> StoreImage(string container, string storageImagePath, Stream imageStream)
    {
      return Task.FromResult(new ImageResponse() { Success = true, ImageUrl = "https://wilderminds.com/images/logo_800x250_bktrans.png", ImageChanged = false });
    }

    public Task<ImageResponse> StoreImage(string container, string storeImagePath, byte[] imageData)
    {
      return StoreImage(container, storeImagePath, new MemoryStream(imageData));
    }
  }
}
