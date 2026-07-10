using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Api.Features.Box;
using ERP.Warehouse.Models.Models.Box;
using ERP.Warehouse.Models.Models.Currency;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Warehouse.Api.Features.Currency;

[Route("api/currency")]
[ApiController]
public class CurrencyController : BaseController
{
    private readonly CurrencyService _currencyService;

    public CurrencyController(CurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    #region Get/Create/Edit/Update/Delete/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(CurrencyReqModel reqModel)
    {
        var result = await _currencyService.Get(reqModel);
        return Execute(result);
    }

    #endregion
}
