using Microsoft.AspNetCore.Mvc;

namespace ERP.Warehouse.Api.Features.Common;

[Route("api/image")]
[ApiController]
public class ImageController : ControllerBase
{
    [HttpGet]
    [Route("product/{fileName}")]
    public IActionResult GetProductImage(string fileName)
    {
        var folder = @"D:\Website Portfolio\Wholesale & Inventory Management System\Image\Product";
        var filePath = Path.Combine(folder, fileName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var ext = Path.GetExtension(fileName).ToLower();
        var contentType = ext switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            ".bmp" => "image/bmp",
            _ => "image/jpeg"
        };

        var bytes = System.IO.File.ReadAllBytes(filePath);
        return File(bytes, contentType);
    }
}
