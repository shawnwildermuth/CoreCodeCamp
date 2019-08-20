//#define USE_FILE_SYSTEM 
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Services
{
  public class DebugImageStorageService : IImageStorageService
  {

#if USE_FILE_SYSTEM
    private readonly IHostingEnvironment _env;
    public DebugImageStorageService(IHostingEnvironment env)
    {
      _env = env;
    }
#endif

    public async Task<string> StoreImage(string filename, byte[] image)
    {
#if USE_FILE_SYSTEM
      var path = Path.Combine(_env.WebRootPath, filename);
      using (var file = new FileStream(path, FileMode.Create))
      {
        await file.WriteAsync(image);
        await file.FlushAsync();
        file.Close();
        return Path.Combine("/", filename);
      }
#else
      return await Task.FromResult("https://wilderminds.com/images/logo_800x250_bktrans.png");
#endif
    }
  }
}
