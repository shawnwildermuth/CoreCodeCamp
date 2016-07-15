using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers.Api
{
  [Authorize]
  [Route("api/images")]
  public class ImageController : Controller
  {
    private IHostingEnvironment _env;

    public ImageController(IHostingEnvironment env)
    {
      _env = env;
    }

    [HttpPost("{imageType}")]
    public async Task<IActionResult> PostImage(string imageType, string imagePath)
    {
      if (imageType != "speaker" && imageType != "sponsor") return BadRequest("Invalid Image Type");

      if (!Request.Form.Files.Any())
      {
        return BadRequest("No Image supplied");
      }

      // Ensure it's a valid extension
      var extension = Path.GetExtension(Request.Form.Files[0].FileName).ToLower();
      if (!(new[] { ".jpg", ".png", ".jpeg" }.Any(s => extension == s)))
      {
        return BadRequest("File msut be .jpg or .png");
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

      // TODO Resize Image
      var size = imageType == "speaker" ? new Size(300, 300) : new Size(300, 88);
      using (var newStream = ResizeImage(Request.Form.Files[0].OpenReadStream(), size))
      using (var stream = System.IO.File.Create(filePath))
      {
        // Write It
        await newStream.CopyToAsync(stream);

        // Calculate the URL
        var imageUrl = $"/img/{imagePath}/{Path.GetFileName(filePath)}";

        return Ok(imageUrl);
      }
    }

    private Stream ResizeImage(Stream stream, Size size)
    {
      MemoryStream outStream = new MemoryStream();

      using (ImageFactory imageFactory = new ImageFactory())
      {
        // Load, resize, set the format, and quality and save an image.
        imageFactory.Load(stream)
                    .Resize(new ResizeLayer(size))
                    .Format(new JpegFormat())
                    .Quality(70)
                    .BackgroundColor(Color.White)
                    .Save(outStream);
      }

      return outStream;
    }


  }
}
