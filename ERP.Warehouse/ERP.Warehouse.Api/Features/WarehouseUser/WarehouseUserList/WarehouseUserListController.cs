using ERP.Warehouse.Models.Models.WarehouseUserList;
using ERP.Warehouse.Api.Controller;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Warehouse.Api.Features.WarehouseUser.WarehouseUserList;

[Route("api/warehouse-user-list")]
[ApiController]
public class WarehouseUserListController : BaseController
{
    private readonly WarehouseUserListService _warehouseUserListService;

    public WarehouseUserListController(WarehouseUserListService warehouseUserListService)
    {
        _warehouseUserListService = warehouseUserListService;
    }

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(WarehouseUserReqModel reqModel)
    {
        var result = await _warehouseUserListService.Get(reqModel);
        return Execute(result);
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Create(WarehouseUserReqModel reqModel)
    {
        var result = await _warehouseUserListService.Create(reqModel);
        return Execute(result);
    }

    [HttpPost]
    [Route("Edit")]
    public async Task<IActionResult> Edit(WarehouseUserEditModel reqModel)
    {
        var result = await _warehouseUserListService.Edit(reqModel);
        return Execute(result);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(WarehouseUserReqModel reqModel)
    {
        var result = await _warehouseUserListService.Update(reqModel);
        return Execute(result);
    }

    [HttpPost]
    [Route("Delete")]
    public async Task<IActionResult> Delete(WarehouseUserEditModel reqModel)
    {
        var result = await _warehouseUserListService.Delete(reqModel);
        return Execute(result);
    }

    #region DropDown

    [HttpPost]
    [Route("GetRole")]
    public async Task<IActionResult> GetRole()
    {
        var result = await _warehouseUserListService.GetRole();
        return Execute(result);
    }

    [HttpPost]
    [Route("GetBranch")]
    public async Task<IActionResult> GetBranch()
    {
        var result = await _warehouseUserListService.GetBranch();
        return Execute(result);
    }

    #endregion
}
