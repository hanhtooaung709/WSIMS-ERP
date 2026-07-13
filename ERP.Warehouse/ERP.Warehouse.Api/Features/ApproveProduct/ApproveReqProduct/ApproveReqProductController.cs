using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Models.Models.Product.ReqProduct;
using Microsoft.AspNetCore.Mvc;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.ApproveProduct.ApproveReqProduct;

[Route("api/approve-req-product")]
[ApiController]
public class ApproveReqProductController : BaseController
{
    private readonly ApproveReqProductService _approveReqProductService;

    public ApproveReqProductController(ApproveReqProductService approveReqProductService, ResponseService responseService) : base(responseService)
    {
        _approveReqProductService = approveReqProductService;
    }
    #region Get/Approve/Reject/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(ReqProductReqModel reqModel)
    {
        var result = await _approveReqProductService.Get(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Approve")]
    public async Task<IActionResult> Approve(ReqProductEditModel reqModel)
    {
        var result = await _approveReqProductService.Approve(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Reject")]
    public async Task<IActionResult> Reject(ReqProductEditModel reqModel)
    {
        var result = await _approveReqProductService.Reject(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Details")]
    public async Task<IActionResult> Details(ReqProductEditModel reqModel)
    {
        var result = await _approveReqProductService.Details(reqModel);
        return await Execute(result);
    }

    #endregion
}
