using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace CoreCodeCamp.Controllers.Api
{
  [Route("{moniker}/api/images")]
  public class ImageController : Controller
  {
    private IHostingEnvironment _env;
    private ILogger<ImageController> _logger;

    public ImageController(IHostingEnvironment env, ILogger<ImageController> logger)
    {
      _env = env;
      _logger = logger;
    }

    [HttpPost("sponsors")]
    [Authorize(Roles = Consts.ADMINROLE)]
    public async Task<IActionResult> PostSponsorImage(string moniker)
    {
      return await SaveImage($"{moniker}/sponsors/", new Size(300, 88));
    }

    [HttpPost("speakers")]
    [Authorize]
    public async Task<IActionResult> PostSpeakerImage(string moniker)
    {
      return await SaveImage($"{moniker}/speakers/", new Size(300, 300));
    }

    async Task<IActionResult> SaveImage(string imagePath, Size size)
    {
      if (!Request.Form.Files.Any())
      {
        _logger.LogWarning("Tried to upload image with no body/image attached");
        return BadRequest("No Image supplied");
      }

      // Ensure it's a valid extension
      var extension = Path.GetExtension(Request.Form.Files[0].FileName).ToLower();
      if (!(new[] { ".jpg", ".png", ".jpeg" }.Any(s => extension == s)))
      {
        return BadRequest("File must be .jpg or .png");
      }

      // Get Path to the speaker directory
      var path = Path.Combine(_env.WebRootPath, "img", imagePath);


      // Make sure the directory exists
      Directory.CreateDirectory(path);
      var filePath = Path.Combine(path, Request.Form.Files[0].FileName);

      // Ensure file doesn't exist
      while (System.IO.File.Exists(filePath))
      {
        filePath = Path.Combine(path, Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(filePath)));
      }

      using (var newStream = ResizeImage(Request.Form.Files[0].OpenReadStream(), size))
      using (var stream = System.IO.File.Create(filePath))
      {
        // Write It
        await newStream.CopyToAsync(stream);

        // Calculate the URL
        var imageUrl = $"/img/{imagePath}/{Path.GetFileName(filePath)}";

        return Created(imageUrl, new { succeeded = true });
      }

    }

    private Stream ResizeImage(Stream stream, Size size)
    {
      MemoryStream outStream = new MemoryStream();

      using (var image = Image.Load(stream))
      {
        var options = new ResizeOptions()
        {
          Mode = ResizeMode.Max,
          Size = new SixLabors.Primitives.Size(size.Width, size.Height)
        };

        // Load, resize, set the format, and quality and save an image.
        image.Mutate(x => x
          .Resize(options));

        image.Save(outStream, new JpegEncoder() { Quality = 70 });

        return outStream;
      }

    }
  }
}
