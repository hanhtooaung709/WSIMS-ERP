using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Branch;
using ERP.Warehouse.Models.Models.Package.ReqPackageChange;
using ERP.Warehouse.Models.Models.Product.ProductList;
using ERP.Warehouse.Models.Models.Stock;
using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;
using System.IO.Packaging;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;
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
                Branch = reqModel.BranchCode,
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

    public async Task<Result<StockModel>> Edit(StockEditModel reqModel)
    {
        var model = new Result<StockModel>();
        try
        {
            #region Check ReqPackage

            var reqStock = await _db.TblReqPackages
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqPackageId == reqModel.ReqPackageId);
            if (reqStock is null)
            {
                model = Result<StockModel>.Error(JsonResource.WHE094);
                return model;
            }

            var package = await _db.TblPackageInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PackageInfoCode == reqStock.PackageInfoCode);
            if (package is null)
            {
                model = Result<StockModel>.Error(JsonResource.WHE083);
                return model;
            }

            #endregion

            #region Prepare Data

            var response = new StockModel
            {
                ReqPackageId = reqStock.ReqPackageId,
                PackageId = reqStock.PackageId,
                PackageName = package.PackageName,
                PackageInfoCode = reqStock.PackageInfoCode,
                ProductCode = package.ProductCode,
                BoxCode = package.BoxCode,
                Quantity = reqStock.Quantity,
                BranchCode = reqStock.BranchCode
            };
            model = Result<StockModel>.Success(response);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<StockModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<StockModel>> Update(StockReqModel reqModel)
    {
        var model = new Result<StockModel>();
        try
        {
            #region Check ReqStock

            var reqStock = await _db.TblReqPackages
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqPackageId == reqModel.ReqPackageId);
            if (reqStock is null)
            {
                model = Result<StockModel>.Error(JsonResource.WHE095);
                return model;
            }

            bool stock = await _db.TblReqPackages
                .AsNoTracking()
                .AnyAsync(x => x.ReqPackageId == reqModel.ReqPackageId &&
                                          x.Status != EnumRequestedStatus.Pending.ToString());
            if (reqStock is null)
            {
                model = Result<StockModel>.Error(JsonResource.WHE096);
                return model;
            }

            #endregion

            #region Prepare Data

            reqStock.Quantity = reqModel.Quantity!;
            reqStock.BranchCode = reqModel.BranchCode!;
            reqStock.ReqDateTime = DevCode.GetServerDateTime();

            _db.Entry(reqStock).State = EntityState.Modified;
            _db.TblReqPackages.Update(reqStock);
            await _db.SaveChangesAsync();
            model = Result<StockModel>.Success(JsonResource.WHS086);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<StockModel>.Error(ex);
        }
        return model;
    }

    #endregion
}
