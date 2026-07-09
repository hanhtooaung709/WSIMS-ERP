using Microsoft.AspNetCore.Mvc;

namespace ERP.Warehouse.Api.Features.Branch;

using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Api.Features.WarehouseUser.WarehouseUserList;
using ERP.Warehouse.Models.Models.WarehouseUser.WarehouseUserList;
using Microsoft.AspNetCore.Http;
[Route("api/branch")]
[ApiController]
public class BranchController : BaseController
{
    private readonly BranchService _branchService;

    public BranchController(BranchService branchService)
    {
        _branchService = branchService;
    }

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get()
    {
        var result = await _branchService.Get();
        return Execute(result);
    }
}
