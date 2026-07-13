using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Api.Features.WarehouseUser.WarehouseUserList;
using ERP.Warehouse.Models.Models.Box;
using ERP.Warehouse.Models.Models.WarehouseUser.WarehouseUserList;
using Microsoft.AspNetCore.Mvc;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Box;

[Route("api/box")]
[ApiController]
public class BoxController : BaseController
{
    private readonly BoxService _boxService;

    public BoxController(BoxService boxService, ResponseService responseService) : base(responseService)
    {
        _boxService = boxService;
    }

    #region Get/Create/Edit/Update/Delete/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(BoxReqModel reqModel)
    {
        var result = await _boxService.Get(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Create(BoxReqModel reqModel)
    {
        var result = await _boxService.Create(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Edit")]
    public async Task<IActionResult> Edit(BoxEditModel reqModel)
    {
        var result = await _boxService.Edit(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(BoxReqModel reqModel)
    {
        var result = await _boxService.Update(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Delete")]
    public async Task<IActionResult> Delete(BoxEditModel reqModel)
    {
        var result = await _boxService.Delete(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Details")]
    public async Task<IActionResult> Details(BoxEditModel reqModel)
    {
        var result = await _boxService.Details(reqModel);
        return await Execute(result);
    }

    #endregion
}
