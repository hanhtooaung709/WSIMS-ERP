using Microsoft.AspNetCore.Mvc;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Controller;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    private readonly IResponseService _responseService;

    public BaseController(IResponseService responseService)
    {
        _responseService = responseService;
    }

    public async Task<IActionResult> Execute<T>(Result<T> model)
    {
        string translation = await _responseService.GetResponseData(model.RespCode!, model.RespDesp);
        model.RespDesp = translation;
        return Ok(model);
    }
}
