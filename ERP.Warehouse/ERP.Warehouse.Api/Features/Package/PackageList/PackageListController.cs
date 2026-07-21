using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Api.Features.Package.ReqPackage;
using ERP.Warehouse.Models.Models.Package.PackageList;
using ERP.Warehouse.Models.Models.Package.ReqPackage;
using Microsoft.AspNetCore.Mvc;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Package.PackageList;

[Route("api/package")]
[ApiController]
public class PackageListController : BaseController
{
    private readonly PackageListSerivce _packageListSerivce;

    public PackageListController(PackageListSerivce packageListSerivce, ResponseService responseService) : base(responseService)
    {
        _packageListSerivce = packageListSerivce;
    }

    #region Get/Create/Edit/Update/Delete/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(PackageReqModel reqModel)
    {
        var result = await _packageListSerivce.Get(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Create(PackageReqModel reqModel)
    {
        var result = await _packageListSerivce.Create(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Edit")]
    public async Task<IActionResult> Edit(PackageEditModel reqModel)
    {
        var result = await _packageListSerivce.Edit(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(PackageReqModel reqModel)
    {
        var result = await _packageListSerivce.Update(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Delete")]
    public async Task<IActionResult> Delete(PackageEditModel reqModel)
    {
        var result = await _packageListSerivce.Delete(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Details")]
    public async Task<IActionResult> Details(PackageEditModel reqModel)
    {
        var result = await _packageListSerivce.Details(reqModel);
        return await Execute(result);
    }

    #endregion
}
