using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Api.Features.Product.ReqProduct;
using ERP.Warehouse.Models.Models.Package.ReqPackage;
using ERP.Warehouse.Models.Models.Product.ReqProduct;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Package.ReqPackage;

[Route("api/req-package")]
[ApiController]
public class ReqPackageController : BaseController
{
    private readonly ReqPackageService _reqPackageService;

    public ReqPackageController(ReqPackageService reqPackageService, ResponseService responseService) : base(responseService)
    {
        _reqPackageService = reqPackageService;
    }

    #region Get/Create/Edit/Update/Delete/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(ReqPackageReqModel reqModel)
    {
        var result = await _reqPackageService.Get(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Edit")]
    public async Task<IActionResult> Edit(ReqPackageEditModel reqModel)
    {
        var result = await _reqPackageService.Edit(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(ReqPackageReqModel reqModel)
    {
        var result = await _reqPackageService.Update(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Delete")]
    public async Task<IActionResult> Delete(ReqPackageEditModel reqModel)
    {
        var result = await _reqPackageService.Delete(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Details")]
    public async Task<IActionResult> Details(ReqPackageEditModel reqModel)
    {
        var result = await _reqPackageService.Details(reqModel);
        return await Execute(result);
    }

    #endregion
}
