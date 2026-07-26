using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Api.Features.Product.ReqProduct;
using ERP.Warehouse.Models.Models.Product.ReqProduct;
using ERP.Warehouse.Models.Models.Product.ReqProductChanges;
using Microsoft.AspNetCore.Mvc;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Product.ReqProductChanges;

[Route("api/req-product-changes")]
[ApiController]
public class ReqProductChangesController : BaseController
{
    private readonly ReqProductChangesService _reqProductChangesService;

    public ReqProductChangesController(ReqProductChangesService reqProductChangesService, ResponseService responseService) : base(responseService)
    {
        _reqProductChangesService = reqProductChangesService;
    }

    #region Get/Edit/Update/Delete/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(ReqProductChangesReqModel reqModel)
    {
        var result = await _reqProductChangesService.Get(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Edit")]
    public async Task<IActionResult> Edit(ReqProductChangesEditModel reqModel)
    {
        var result = await _reqProductChangesService.Edit(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(ReqProductChangesReqModel reqModel)
    {
        var result = await _reqProductChangesService.Update(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Delete")]
    public async Task<IActionResult> Delete(ReqProductChangesEditModel reqModel)
    {
        var result = await _reqProductChangesService.Delete(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Details")]
    public async Task<IActionResult> Details(ReqProductChangesEditModel reqModel)
    {
        var result = await _reqProductChangesService.Details(reqModel);
        return await Execute(result);
    }

    #endregion
}
