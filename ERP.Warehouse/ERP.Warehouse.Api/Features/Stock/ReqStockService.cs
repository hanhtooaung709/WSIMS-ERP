using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models;
using WSIMS_ERP.Shared.Models.DynamicModel;
using ERP.Warehouse.Models.Models.Package.ReqPackageChange;
using ERP.Warehouse.Models.Models.Product.ProductList;
using ERP.Warehouse.Models.Models.Stock;
using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;
using System.Data;
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

    #region Get/Edit/Update/Delete/Details

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
                model = Result<StockModel>.Error(JsonResource.WHE094);
                return model;
            }

            bool stock = await _db.TblReqPackages
                .AsNoTracking()
                .AnyAsync(x => x.ReqPackageId == reqModel.ReqPackageId &&
                                          x.Status != EnumRequestedStatus.Pending.ToString());
            if (reqStock is null)
            {
                model = Result<StockModel>.Error(JsonResource.WHE095);
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

    public async Task<Result<StockModel>> Delete(StockEditModel reqModel)
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
                model = Result<StockModel>.Error(JsonResource.WHE094);
                return model;
            }

            bool stock = await _db.TblReqPackages
                .AsNoTracking()
                .AnyAsync(x => x.ReqPackageId == reqModel.ReqPackageId &&
                                          x.Status != EnumRequestedStatus.Pending.ToString());
            if (reqStock is null)
            {
                model = Result<StockModel>.Error(JsonResource.WHE095);
                return model;
            }

            #endregion

            #region Prepare Data

            _db.TblReqPackages.Remove(reqStock);
            var result = _db.SaveChanges();
            if (result <= 0)
            {
                model = Result<StockModel>.Error(JsonResource.WHE096);
                return model;
            }
            model = Result<StockModel>.Success(JsonResource.WHS097);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<StockModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<StockDetailModel>> Details(StockEditModel reqModel)
    {
        StockDetailModel model = new();
        try
        {
            var detail = await _dapperService.GetDetailAsync<StockDetailInfoModel>(
                SqlQueries.Sp_GetStockDetail, new
                {
                    ReqPackageId = reqModel.ReqPackageId
                }, CommandType.StoredProcedure);

            List<DynamicReportModel> packageInfo = new List<DynamicReportModel>();
            packageInfo.Add("Package Name", detail.PackageName!);
            packageInfo.Add("Product Name", detail.ProductName!);
            packageInfo.Add("PackageInfo Code", detail.PackageInfoCode!);
            packageInfo.Add("Price", detail.Price!);
            packageInfo.Add("Currency Code", detail.CurrencyCode!);
            packageInfo.Add("Weight", detail.Weight!);
            packageInfo.Add("Box", detail.Box!);
            model.Package = packageInfo;
            model.ItemImagePath = detail.ImagePath;

            List<DynamicReportModel> makerChecker = new List<DynamicReportModel>();
            makerChecker.Add("Requested User", detail.ReqUser!);
            makerChecker.Add("Requested DateTime", detail.ReqDateTime!);
            makerChecker.Add("Approved User", detail.ApprovedUser!.ToDashFromNull());
            makerChecker.Add("Approved DateTime ", detail.ApprovedDateTime!.ToDashFromNull());
            makerChecker.Add("Status", detail.Status!);
            makerChecker.Add("Reject Reason", detail.RejectReason!.ToDashFromNull());
            model.MakerChecker = makerChecker;

            return Result<StockDetailModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<StockDetailModel>.Error(ex);
        }
    }

    #endregion
}
