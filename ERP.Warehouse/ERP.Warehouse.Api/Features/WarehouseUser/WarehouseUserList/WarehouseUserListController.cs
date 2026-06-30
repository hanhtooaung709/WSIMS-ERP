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
    [Route("Edit")]
    public async Task<IActionResult> Edit(WarehouseUserEditModel reqModel)
    {
        var result = await _warehouseUserListService.Edit(reqModel);
        return Execute(result);
    }

    [HttpPost]
    [Route("Delete")]
    public async Task<IActionResult> Delete(WarehouseUserEditModel reqModel)
    {
        var result = await _warehouseUserListService.Delete(reqModel);
        return Execute(result);
    }
}
