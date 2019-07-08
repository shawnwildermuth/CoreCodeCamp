using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Services
{
  public class DebugImageStorageService : IImageStorageService
  {
    public Task<string> StoreImage(string filename, byte[] image)
    {
      return Task.FromResult("https://wilderminds.com/images/logo_800x250_bktrans.png");
    }
  }
}
