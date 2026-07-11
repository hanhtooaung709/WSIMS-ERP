using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Models.Models.Product.ReqProduct;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Warehouse.Api.Features.ApproveProduct.ApproveReqProduct;

[Route("api/approve-req-product")]
[ApiController]
public class ApproveReqProductController : BaseController
{
    private readonly ApproveReqProductService _approveReqProductService;

    public ApproveReqProductController(ApproveReqProductService approveReqProductService)
    {
        _approveReqProductService = approveReqProductService;
    }
    #region Get/Approve/Reject/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(ReqProductReqModel reqModel)
    {
        var result = await _approveReqProductService.Get(reqModel);
        return Execute(result);
    }

    [HttpPost]
    [Route("Approve")]
    public async Task<IActionResult> Approve(ReqProductEditModel reqModel)
    {
        var result = await _approveReqProductService.Approve(reqModel);
        return Execute(result);
    }

    [HttpPost]
    [Route("Reject")]
    public async Task<IActionResult> Reject(ReqProductEditModel reqModel)
    {
        var result = await _approveReqProductService.Reject(reqModel);
        return Execute(result);
    }

    [HttpPost]
    [Route("Details")]
    public async Task<IActionResult> Details(ReqProductEditModel reqModel)
    {
        var result = await _approveReqProductService.Details(reqModel);
        return Execute(result);
    }

    #endregion
}
