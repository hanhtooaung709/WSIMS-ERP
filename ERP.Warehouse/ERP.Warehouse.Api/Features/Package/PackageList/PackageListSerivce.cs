using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.VariantTypes;
using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Currency;
using ERP.Warehouse.Models.Models.Package.PackageList;
using ERP.Warehouse.Models.Models.Package.ReqPackage;
using ERP.Warehouse.Models.Models.Product.ProductList;
using ERP.Warehouse.Models.Models.WarehouseUser.WarehouseUserList;
using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;
using System.IO.Packaging;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Models.ConfigModel;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Package.PackageList;

public class PackageListSerivce : AuthorizationService
{

    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;

    public PackageListSerivce(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger,
        CustomSettingModel setting) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
    }

    #region Get/Create/Edit/Update/Delete/Details

    public async Task<Result<PackageRepModel>> Get(PackageReqModel reqModel)
    {
        PackageRepModel model = new();
        try
        {
            var parameters = new
            {
                CurrentUserId = AuthorizedUserId,
                PackageName = reqModel.PackageName,
                ProductName = reqModel.ProductCode,
                Box = reqModel.BoxCode,
            };
            var result = await _dapperService.QueryStoredProcedureAsync<PackageModel>
                (SqlQueries.Sp_GetPackageList, parameters);
            model.list = result;
            return Result<PackageRepModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<PackageRepModel>.Error(ex);
        }
    }

    #endregion
}
