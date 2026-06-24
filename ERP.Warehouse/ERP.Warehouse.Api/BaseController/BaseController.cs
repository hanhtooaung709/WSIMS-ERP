using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared.Models;

namespace ERP.Warehouse.Api.BaseController;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    // API Controller တိုင်းကနေ လှမ်းသုံးလို့ရအောင် Shortcut ဆောက်ထားခြင်း
    protected string CurrentUserId => User.FindFirst("UserId")?.Value
                                      ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                      ?? string.Empty;

    public IActionResult Execute<T>(Result<T> model)
    {
        if (model.RespType == EnumRespType.Success)
            StatusCode(201, model);

        if (model.RespType == EnumRespType.Error)
            return Ok(model);

        if (model.RespType == EnumRespType.SystemError)
            return BadRequest(model);

        return Ok(model);
    }
}
