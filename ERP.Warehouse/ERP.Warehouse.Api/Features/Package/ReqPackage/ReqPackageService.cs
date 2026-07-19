using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Package.ReqPackage;
using ERP.Warehouse.Models.Models.Product.ProductList;
using ERP.Warehouse.Models.Models.WarehouseUser.ReqWarehouseUser;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Models.ConfigModel;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Package.ReqPackage;

public class ReqPackageService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;

    public ReqPackageService(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger,
        CustomSettingModel setting) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
    }

    #region Get/Create/Edit/Update/Delete/Details

    public async Task<Result<ReqPackageRepModel>> Get(ReqPackageReqModel reqModel)
    {
        ReqPackageRepModel model = new();
        try
        {
            var parameters = new
            {
                CurrentUserId = AuthorizedUserId,
                PackageName = reqModel.PackageName,
                ProductName = reqModel.ProductName,
                Box = reqModel.Box,
                Status = reqModel.Status
            };
            var result = await _dapperService.QueryStoredProcedureAsync<ReqPackageModel>
                (SqlQueries.Sp_GetReqPackageList, parameters);
            model.list = result;
            return Result<ReqPackageRepModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<ReqPackageRepModel>.Error(ex);
        }
    }

    #endregion
}
