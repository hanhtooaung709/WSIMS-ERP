using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Api.Features.WarehouseUser.WarehouseUserList;
using ERP.Warehouse.Models.Models.Box;
using ERP.Warehouse.Models.Models.WarehouseUser.WarehouseUserList;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Warehouse.Api.Features.Box;

[Route("api/box")]
[ApiController]
public class BoxController : BaseController
{
    private readonly BoxService _boxService;

    public BoxController(BoxService boxService)
    {
        _boxService = boxService;
    }

    #region Get/Create/Edit/Update/Delete/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(BoxReqModel reqModel)
    {
        var result = await _boxService.Get(reqModel);
        return Execute(result);
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Create(BoxReqModel reqModel)
    {
        var result = await _boxService.Create(reqModel);
        return Execute(result);
    }

    [HttpPost]
    [Route("Edit")]
    public async Task<IActionResult> Edit(BoxEditModel reqModel)
    {
        var result = await _boxService.Edit(reqModel);
        return Execute(result);
    }

    #endregion
}
