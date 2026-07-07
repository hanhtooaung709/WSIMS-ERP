using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Models.Models.WarehouseUser.ReqWarehouseUserChanges;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Warehouse.Api.Features.WarehouseUser.ReqWarehouseUserChanges;

[Route("api/req-warehouse-user-changes")]
[ApiController]
public class ReqWarehouseUserChangesController : BaseController
{
    private readonly ReqWarehouseUserChangesService _reqWarehouseUserChangesService;

    public ReqWarehouseUserChangesController(ReqWarehouseUserChangesService reqWarehouseUserChangesService)
    {
        _reqWarehouseUserChangesService = reqWarehouseUserChangesService;
    }

    #region Get/Create/Edit/Update/Delete/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(ReqWarehouseUserChangesReqModel reqModel)
    {
        var result = await _reqWarehouseUserChangesService.Get(reqModel);
        return Execute(result);
    }

    [HttpPost]
    [Route("Edit")]
    public async Task<IActionResult> Edit(ReqWarehouseUserChangesEditModel reqModel)
    {
        var result = await _reqWarehouseUserChangesService.Edit(reqModel);
        return Execute(result);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(ReqWarehouseUserChangesReqModel reqModel)
    {
        var result = await _reqWarehouseUserChangesService.Update(reqModel);
        return Execute(result);
    }

    [HttpPost]
    [Route("Delete")]
    public async Task<IActionResult> Delete(ReqWarehouseUserChangesEditModel reqModel)
    {
        var result = await _reqWarehouseUserChangesService.Delete(reqModel);
        return Execute(result);
    }

    [HttpPost]
    [Route("Details")]
    public async Task<IActionResult> Details(ReqWarehouseUserChangesEditModel reqModel)
    {
        var result = await _reqWarehouseUserChangesService.Details(reqModel);
        return Execute(result);
    }

    #endregion
}
