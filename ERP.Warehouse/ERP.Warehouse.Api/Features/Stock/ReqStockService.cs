using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Package.ReqPackageChange;
using ERP.Warehouse.Models.Models.Stock;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Models.ConfigModel;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Stock;

public class ReqStockService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;

    public ReqStockService(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger,
        CustomSettingModel setting) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
    }

    #region Get/Create/Edit/Update/Delete/Details

    public async Task<Result<StockRepModel>> Get(StockReqModel reqModel)
    {
        StockRepModel model = new();
        try
        {
            var parameters = new
            {
                CurrentUserId = AuthorizedUserId,
                PackageName = reqModel.PackageName,
                ProductName = reqModel.ProductCode,
                Box = reqModel.BoxCode,
                Branch = reqModel.Branch,
                Status = reqModel.Status
            };
            var result = await _dapperService.QueryStoredProcedureAsync<StockModel>
                (SqlQueries.Sp_GetStockList, parameters);
            model.list = result;
            return Result<StockRepModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<StockRepModel>.Error(ex);
        }
    }

    #endregion
}
