using DocumentFormat.OpenXml.Spreadsheet;
using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Branch;
using ERP.Warehouse.Models.Models.Currency;
using ERP.Warehouse.Models.Models.Package.ReqPackage;
using ERP.Warehouse.Models.Models.Product.ProductList;
using ERP.Warehouse.Models.Models.Product.ReqProduct;
using ERP.Warehouse.Models.Models.WarehouseUser.ReqWarehouseUser;
using Microsoft.EntityFrameworkCore;
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
                ProductName = reqModel.ProductCode,
                Box = reqModel.BoxCode,
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

    public async Task<Result<ReqPackageModel>> Edit(ReqPackageEditModel reqModel)
    {
        var model = new Result<ReqPackageModel>();
        try
        {
            #region Check ReqPackage

            var reqPackage = await _db.TblReqPackageInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqPackageInfoId == reqModel.ReqPackageId);
            if (reqPackage is null)
            {
                model = Result<ReqPackageModel>.Error(JsonResource.WHE072);
                return model;
            }

            #endregion

            #region Check Branch

            var branch = await _db.TblBranches
            .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BranchCode == reqPackage.BranchCode && x.DelFlag == 0);
            if (branch is null)
            {
                model = Result<ReqPackageModel>.Error(JsonResource.WHE016);
                return model;
            }

            #endregion

            #region Check Product

            var product = await _db.TblProducts
            .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductCode == reqPackage.ProductCode && x.DelFlag == 0);
            if (product is null)
            {
                model = Result<ReqPackageModel>.Error(JsonResource.WHE067);
                return model;
            }

            #endregion

            #region Check Currency

            var currency = await _db.TblCurrencies
            .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CurrencyCode == reqPackage.CurrencyCode && x.DelFlag == 0);
            if (currency is null)
            {
                model = Result<ReqPackageModel>.Error(JsonResource.WHE067);
                return model;
            }

            #endregion

            #region Check Box

            var box = await _db.TblBoxes
            .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BoxCode == reqPackage.BoxCode && x.DelFlag == 0);
            if (product is null)
            {
                model = Result<ReqPackageModel>.Error(JsonResource.WHE059);
                return model;
            }

            #endregion

            #region Prepare Data

            var response = new ReqPackageModel
            {
                ReqPackageId = reqPackage.ReqPackageInfoId,
                PackageName = reqPackage.PackageName,
                PackageInfoCode = reqPackage.PackageInfoCode,
                BranchCode = reqPackage.BranchCode,
                Quanity = reqPackage.Quanity.ToString(),
                ProductCode = reqPackage.ProductCode,
                Price = reqPackage.Price.ToString(),
                CurrencyCode = reqPackage.CurrencyCode,
                Weight = reqPackage.Weight.ToString(),
                BoxCode = reqPackage.BoxCode,
                Status = reqPackage.Status
            };
            model = Result<ReqPackageModel>.Success(response);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqPackageModel>.Error(ex);
        }
        return model;
    }

    #endregion
}
