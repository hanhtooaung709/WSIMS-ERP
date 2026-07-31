using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Api.Features.Stock;
using ERP.Warehouse.Models.Models.Stock;
using Microsoft.AspNetCore.Mvc;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.ApproveStock;

[Route("api/approve-stock")]
[ApiController]
public class ApproveStockController : BaseController
{
    private readonly ApproveStockService _approveStockService;

    public ApproveStockController(ApproveStockService approveStockService, ResponseService responseService) : base(responseService)
    {
        _approveStockService = approveStockService;
    }

    #region Get/Approve/Reject/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(StockReqModel reqModel)
    {
        var result = await _approveStockService.Get(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Approve")]
    public async Task<IActionResult> Approve(StockEditModel reqModel)
    {
        var result = await _approveStockService.Approve(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Details")]
    public async Task<IActionResult> Details(StockEditModel reqModel)
    {
        var result = await _approveStockService.Details(reqModel);
        return await Execute(result);
    }

    #endregion
}
