using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Api.Features.Box;
using ERP.Warehouse.Models.Models.Box;
using ERP.Warehouse.Models.Models.Currency;
using Microsoft.AspNetCore.Mvc;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Currency;

[Route("api/currency")]
[ApiController]
public class CurrencyController : BaseController
{
    private readonly CurrencyService _currencyService;

    public CurrencyController(CurrencyService currencyService, ResponseService responseService) : base(responseService)
    {
        _currencyService = currencyService;
    }

    #region Get/Create/Edit/Update/Delete/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(CurrencyReqModel reqModel)
    {
        var result = await _currencyService.Get(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Create(CurrencyReqModel reqModel)
    {
        var result = await _currencyService.Create(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Edit")]
    public async Task<IActionResult> Edit(CurrencyEditModel reqModel)
    {
        var result = await _currencyService.Edit(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(CurrencyReqModel reqModel)
    {
        var result = await _currencyService.Update(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Delete")]
    public async Task<IActionResult> Delete(CurrencyEditModel reqModel)
    {
        var result = await _currencyService.Delete(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Details")]
    public async Task<IActionResult> Details(CurrencyEditModel reqModel)
    {
        var result = await _currencyService.Details(reqModel);
        return await Execute(result);
    }

    #endregion
}
