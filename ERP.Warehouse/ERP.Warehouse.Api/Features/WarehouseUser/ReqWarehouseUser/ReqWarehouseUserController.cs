using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Models.Models.WarehouseUser.ReqWarehouseUser;
using Microsoft.AspNetCore.Mvc;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.WarehouseUser.ReqWarehouseUser;

[Route("api/req-warehouse-user")]
[ApiController]
public class ReqWarehouseUserController : BaseController
{
    private readonly ReqWarehouseUserService _reqWarehouseUserService;

    public ReqWarehouseUserController(ReqWarehouseUserService reqWarehouseUserService, ResponseService responseService) : base(responseService)
    {
        _reqWarehouseUserService = reqWarehouseUserService;
    }

    #region Get/Edit/Update/Delete/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(ReqWarehouseUserReqModel reqModel)
    {
        var result = await _reqWarehouseUserService.Get(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Edit")]
    public async Task<IActionResult> Edit(ReqWarehouseUserEditModel reqModel)
    {
        var result = await _reqWarehouseUserService.Edit(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(ReqWarehouseUserReqModel reqModel)
    {
        var result = await _reqWarehouseUserService.Update(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Delete")]
    public async Task<IActionResult> Delete(ReqWarehouseUserEditModel reqModel)
    {
        var result = await _reqWarehouseUserService.Delete(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Details")]
    public async Task<IActionResult> Details(ReqWarehouseUserEditModel reqModel)
    {
        var result = await _reqWarehouseUserService.Details(reqModel);
        return await Execute(result);
    }

    #endregion
}
