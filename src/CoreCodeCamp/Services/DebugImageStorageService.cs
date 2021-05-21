using System.IO;
using System.Threading.Tasks;
using WilderMinds.AzureImageService;

namespace CoreCodeCamp.Services
{
  public class DebugImageStorageService : IImageStorageService
  {

    public Task<ImageResponse> StoreImage(string storageImagePath, Stream imageStream)
    {
      return Task.FromResult(new ImageResponse() { Success = true, ImageUrl = "https://wilderminds.com/images/logo_800x250_bktrans.png", ImageChanged = false });
    }

    public Task<ImageResponse> StoreImage(string storeImagePath, byte[] imageData)
    {
      return StoreImage(storeImagePath, new MemoryStream(imageData));
    }
  }
}
