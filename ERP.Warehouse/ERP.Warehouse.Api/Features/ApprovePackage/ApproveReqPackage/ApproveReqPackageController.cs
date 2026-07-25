using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Api.Features.Package.ReqPackage;
using ERP.Warehouse.Models.Models.Package.ReqPackage;
using Microsoft.AspNetCore.Mvc;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.ApprovePackage.ApproveReqPackage;

[Route("api/approve-req-package")]
[ApiController]
public class ApproveReqPackageController : BaseController
{
    private readonly ApproveReqPackageSevice _aproveReqPackageSevice;

    public ApproveReqPackageController(ApproveReqPackageSevice aproveReqPackageSevice, ResponseService responseService) : base(responseService)
    {
        _aproveReqPackageSevice = aproveReqPackageSevice;
    }

    #region Get/Approve/Reject/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(ReqPackageReqModel reqModel)
    {
        var result = await _aproveReqPackageSevice.Get(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Approve")]
    public async Task<IActionResult> Approve(ReqPackageEditModel reqModel)
    {
        var result = await _aproveReqPackageSevice.Approve(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Details")]
    public async Task<IActionResult> Details(ReqPackageEditModel reqModel)
    {
        var result = await _aproveReqPackageSevice.Details(reqModel);
        return await Execute(result);
    }

    #endregion
}
