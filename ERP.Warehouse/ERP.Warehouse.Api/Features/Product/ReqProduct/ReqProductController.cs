using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Api.Features.WarehouseUser.ReqWarehouseUser;
using ERP.Warehouse.Models.Models.Product.ReqProduct;
using ERP.Warehouse.Models.Models.WarehouseUser.ReqWarehouseUser;
using Microsoft.AspNetCore.Mvc;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Product.ReqProduct;

[Route("api/req-product")]
[ApiController]
public class ReqProductController : BaseController
{
    private readonly ReqProductService _reqProductService;

    public ReqProductController(ReqProductService reqProductService, ResponseService responseService) : base(responseService)
    {
        _reqProductService = reqProductService;
    }

    #region Get/Create/Edit/Update/Delete/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(ReqProductReqModel reqModel)
    {
        var result = await _reqProductService.Get(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Edit")]
    public async Task<IActionResult> Edit(ReqProductEditModel reqModel)
    {
        var result = await _reqProductService.Edit(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(ReqProductReqModel reqModel)
    {
        var result = await _reqProductService.Update(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Delete")]
    public async Task<IActionResult> Delete(ReqProductEditModel reqModel)
    {
        var result = await _reqProductService.Delete(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Details")]
    public async Task<IActionResult> Details(ReqProductEditModel reqModel)
    {
        var result = await _reqProductService.Details(reqModel);
        return await Execute(result);
    }

    #endregion
}
