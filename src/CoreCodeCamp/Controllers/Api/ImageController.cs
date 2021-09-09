using System;
using System.Collections.Generic;
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
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using WilderMinds.AzureImageStorageService;

namespace CoreCodeCamp.Controllers.Api
{
  [Route("{moniker}/api/images")]
  [ApiController]
  public class ImageController : Controller
  {
    private IWebHostEnvironment _env;
    private ILogger<ImageController> _logger;
    private readonly IAzureImageStorageService _imageService;

    public ImageController(IWebHostEnvironment env, ILogger<ImageController> logger, IAzureImageStorageService imageService)
    {
      _env = env;
      _logger = logger;
      _imageService = imageService;
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
      var path = Path.Combine("img", imagePath, Request.Form.Files[0].FileName).Replace("\\", "/").ToLower();

      using (var newStream = ResizeImage(Request.Form.Files[0].OpenReadStream(), size))
      {
        // Write It
        newStream.Position = 0;
        var result = await _imageService.StoreImage("acc", path, newStream.ToArray());

        if (result.Success)
        {
          return Created(result.ImageUrl, new { succeeded = result.Success });
        }
        else
        {
          return BadRequest(result.Exception.Message);
        }
      }

    }

    private MemoryStream ResizeImage(Stream stream, Size size)
    {
      MemoryStream outStream = new MemoryStream();

      using (var image = Image.Load(stream))
      {
        var options = new ResizeOptions()
        {
          Mode = ResizeMode.Pad,
          Size = new Size(size.Width, size.Height)
        };

        // Load, resize, set the format, and quality and save an image.
        image.Mutate(x => x
          .Resize(options)
          .BackgroundColor(new Rgba32(255, 255, 255, 255)));

        image.Save(outStream, new JpegEncoder() { Quality = 70 });

        return outStream;
      }

    }
  }
}
