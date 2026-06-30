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
    public async Task<IActionResult> Edit(string Id)
    {
        var result = await _warehouseUserListService.Edit(Id);
        return Execute(result);
    }
}
