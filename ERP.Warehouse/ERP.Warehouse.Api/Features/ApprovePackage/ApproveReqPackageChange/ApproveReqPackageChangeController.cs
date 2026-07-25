using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Api.Features.Package.ReqPackageChange;
using ERP.Warehouse.Models.Models.Package.ReqPackageChange;
using Microsoft.AspNetCore.Mvc;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.ApprovePackage.ApproveReqPackageChange;

[Route("api/approve-req-package-change")]
[ApiController]
public class ApproveReqPackageChangeController : BaseController
{
    private readonly ApproveReqPackageChangeService _approveReqPackageChangeService;

    public ApproveReqPackageChangeController(ApproveReqPackageChangeService approveReqPackageChangeService, ResponseService responseService) : base(responseService)
    {
        _approveReqPackageChangeService = approveReqPackageChangeService;
    }

    #region Get/Approve/Reject/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(ReqPackageChangeReqModel reqModel)
    {
        var result = await _approveReqPackageChangeService.Get(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Approve")]
    public async Task<IActionResult> Approve(ReqPackageChangeEditModel reqModel)
    {
        var result = await _approveReqPackageChangeService.Approve(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Reject")]
    public async Task<IActionResult> Reject(ReqPackageChangeEditModel reqModel)
    {
        var result = await _approveReqPackageChangeService.Reject(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Details")]
    public async Task<IActionResult> Details(ReqPackageChangeEditModel reqModel)
    {
        var result = await _approveReqPackageChangeService.Details(reqModel);
        return await Execute(result);
    }

    #endregion
}
