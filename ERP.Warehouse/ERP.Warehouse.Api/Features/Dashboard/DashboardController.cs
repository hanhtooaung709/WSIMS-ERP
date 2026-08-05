using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Api.Features.Currency;
using ERP.Warehouse.Models.Models.Currency;
using Microsoft.AspNetCore.Mvc;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Dashboard;

[Route("api/dashboard")]
[ApiController]
public class DashboardController : BaseController
{
    private readonly DashboardService _dashboardService;

    public DashboardController(DashboardService dashboardService, ResponseService responseService) : base(responseService)
    {
        _dashboardService = dashboardService;
    }

    [HttpPost]
    [Route("GetDashboardData")]
    public async Task<IActionResult> Get()
    {
        var result = await _dashboardService.GetDashboardData();
        return await Execute(result);
    }
}
