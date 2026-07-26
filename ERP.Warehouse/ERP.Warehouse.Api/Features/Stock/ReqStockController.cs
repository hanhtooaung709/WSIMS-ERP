using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Api.Features.Package.ReqPackageChange;
using ERP.Warehouse.Models.Models.Package.ReqPackageChange;
using ERP.Warehouse.Models.Models.Stock;
using Microsoft.AspNetCore.Mvc;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Stock;

[Route("api/req-stock")]
[ApiController]
public class ReqStockController : BaseController
{
    private readonly ReqStockService _reqStockService;

    public ReqStockController(ReqStockService reqStockService, ResponseService responseService) : base(responseService)
    {
        _reqStockService = reqStockService;
    }

    #region Get/Edit/Update/Delete/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(StockReqModel reqModel)
    {
        var result = await _reqStockService.Get(reqModel);
        return await Execute(result);
    }

    #endregion
}
