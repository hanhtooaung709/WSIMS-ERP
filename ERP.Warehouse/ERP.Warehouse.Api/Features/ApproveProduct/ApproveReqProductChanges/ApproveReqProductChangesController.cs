using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Api.Features.Product.ReqProductChanges;
using ERP.Warehouse.Models.Models.Product.ReqProductChanges;
using Microsoft.AspNetCore.Mvc;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.ApproveProduct.ApproveReqProductChanges;

[Route("api/approve-req-product-changes")]
[ApiController]
public class ApproveReqProductChangesController : BaseController
{
    private readonly ApproveReqProductChangesService _approveReqProductChangesService;

    public ApproveReqProductChangesController(ApproveReqProductChangesService approveReqProductChangesService, ResponseService responseService) : base(responseService)
    {
        _approveReqProductChangesService = approveReqProductChangesService;
    }

    #region Get/Approve/Reject/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(ReqProductChangesReqModel reqModel)
    {
        var result = await _approveReqProductChangesService.Get(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Approve")]
    public async Task<IActionResult> Approve(ReqProductChangesEditModel reqModel)
    {
        var result = await _approveReqProductChangesService.Approve(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Reject")]
    public async Task<IActionResult> Reject(ReqProductChangesEditModel reqModel)
    {
        var result = await _approveReqProductChangesService.Reject(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Details")]
    public async Task<IActionResult> Details(ReqProductChangesEditModel reqModel)
    {
        var result = await _approveReqProductChangesService.Details(reqModel);
        return await Execute(result);
    }

    #endregion
}
