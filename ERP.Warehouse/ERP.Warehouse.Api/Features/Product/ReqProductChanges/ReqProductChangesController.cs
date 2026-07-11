using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Api.Features.Product.ReqProduct;
using ERP.Warehouse.Models.Models.Product.ReqProduct;
using ERP.Warehouse.Models.Models.Product.ReqProductChanges;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Warehouse.Api.Features.Product.ReqProductChanges;

[Route("api/req-product-changes")]
[ApiController]
public class ReqProductChangesController : BaseController
{
    private readonly ReqProductChangesService _reqProductChangesService;

    public ReqProductChangesController(ReqProductChangesService reqProductChangesService)
    {
        _reqProductChangesService = reqProductChangesService;
    }

    #region Get/Create/Edit/Update/Delete/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(ReqProductChangesReqModel reqModel)
    {
        var result = await _reqProductChangesService.Get(reqModel);
        return Execute(result);
    }

    [HttpPost]
    [Route("Edit")]
    public async Task<IActionResult> Edit(ReqProductChangesEditModel reqModel)
    {
        var result = await _reqProductChangesService.Edit(reqModel);
        return Execute(result);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(ReqProductChangesReqModel reqModel)
    {
        var result = await _reqProductChangesService.Update(reqModel);
        return Execute(result);
    }

    #endregion
}
