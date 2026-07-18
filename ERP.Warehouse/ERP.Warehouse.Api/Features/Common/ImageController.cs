using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WSIMS_ERP.Shared.Models.ConfigModel;

namespace ERP.Warehouse.Api.Features.Common;

[Route("api/image")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly CustomSettingModel _setting;

    public ImageController(IOptionsMonitor<CustomSettingModel> setting)
    {
        _setting = setting.CurrentValue;
    }

    [HttpGet]
    [Route("{folder}/{fileName}")]
    public IActionResult GetImage(string folder, string fileName)
    {
        var allowedFolders = new[] { "Product", "ReqProduct", "ReqProductChange" };
        if (!allowedFolders.Contains(folder))
            return NotFound();

        var basePath = folder switch
        {
            "Product" => _setting.Image.Product,
            "ReqProduct" => _setting.Image.ReqProduct,
            "ReqProductChange" => _setting.Image.ReqProductChange,
            _ => null
        };

        if (basePath is null)
            return NotFound();

        var filePath = Path.Combine(basePath, fileName);

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
