using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Models.Models.Package.ReqPackageChange;
using Microsoft.AspNetCore.Mvc;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Package.ReqPackageChange;

[Route("api/req-package-change")]
[ApiController]
public class ReqPackageChangeController : BaseController
{
    private readonly ReqPackageChangeService _reqPackageChangeService;

    public ReqPackageChangeController(ReqPackageChangeService reqPackageChangeService, ResponseService responseService) : base(responseService)
    {
        _reqPackageChangeService = reqPackageChangeService;
    }

    #region Get/Edit/Update/Delete/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(ReqPackageChangeReqModel reqModel)
    {
        var result = await _reqPackageChangeService.Get(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Edit")]
    public async Task<IActionResult> Edit(ReqPackageChangeEditModel reqModel)
    {
        var result = await _reqPackageChangeService.Edit(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(ReqPackageChangeReqModel reqModel)
    {
        var result = await _reqPackageChangeService.Update(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Delete")]
    public async Task<IActionResult> Delete(ReqPackageChangeEditModel reqModel)
    {
        var result = await _reqPackageChangeService.Delete(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Details")]
    public async Task<IActionResult> Details(ReqPackageChangeEditModel reqModel)
    {
        var result = await _reqPackageChangeService.Details(reqModel);
        return await Execute(result);
    }

    #endregion
}
