using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Api.Features.Product.ReqProduct;
using ERP.Warehouse.Models.Models.Product.ReqProduct;
using Microsoft.AspNetCore.Http;
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

    #endregion
}
