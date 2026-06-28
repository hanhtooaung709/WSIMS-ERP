using Microsoft.AspNetCore.Mvc;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared.Models;

namespace ERP.Warehouse.Api.Controller;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    public IActionResult Execute<T>(Result<T> model)
    {
        if (model.RespType == EnumRespType.Success)
            return StatusCode(201, model);

        if (model.RespType == EnumRespType.Error)
            return Ok(model);

        if (model.RespType == EnumRespType.SystemError)
            return BadRequest(model);

        return Ok(model);
    }
}
