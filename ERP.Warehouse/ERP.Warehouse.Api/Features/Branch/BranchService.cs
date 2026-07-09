using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Branch;
using ERP.Warehouse.Models.Models.WarehouseUser.WarehouseUserList;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Branch;

public class BranchService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;

    public BranchService(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
    }

    public async Task<Result<BranchRepModel>> Get()
    {
        BranchRepModel model = new();
        try
        {
            var result = await _dapperService.QueryStoredProcedureAsync<BranchModel>
                (SqlQueries.Sp_GetBranch, null!);
            model.list = result;
            return Result<BranchRepModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<BranchRepModel>.Error(ex);
        }
    }
}
