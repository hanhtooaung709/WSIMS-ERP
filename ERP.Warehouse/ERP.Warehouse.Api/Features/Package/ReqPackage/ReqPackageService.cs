using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models;
using ERP.Warehouse.Models.Models.Package.ReqPackage;
using Microsoft.EntityFrameworkCore;
using WSIMS_ERP.Shared.Models.DynamicModel;
using Module.CommonDbService.EfAppDbContextModels;
using System.Data;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Models.ConfigModel;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;
using ERP.Warehouse.Models.Models.WarehouseUser.WarehouseUserList;

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
                model = Result<ReqPackageModel>.Error(JsonResource.WHE052);
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
                Quanity = reqPackage.Quantity,
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

    public async Task<Result<ReqPackageModel>> Update(ReqPackageReqModel reqModel)
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

            bool package = await _db.TblReqPackageInfos
                .AsNoTracking()
                .AnyAsync(x => x.ReqPackageInfoId == reqModel.ReqPackageId &&
                               x.Status != EnumRequestedStatus.Pending.ToString());
            if (package)
            {
                model = Result<ReqPackageModel>.Error(JsonResource.WHE073);
                return model;
            }

            #endregion

            #region Check Duplicate PackageName

            bool name = await _db.TblPackageInfos
                .AsNoTracking()
                .AnyAsync(x => x.PackageName.Trim().ToLower() == reqModel.PackageName!.Trim().ToLower());
            if (name)
            {
                model = Result<ReqPackageModel>.Error(JsonResource.WHE074);
                return model;
            }

            name = await _db.TblReqPackageInfos
                .AsNoTracking()
                .AnyAsync(x => x.PackageName.Trim().ToLower() == reqModel.PackageName!.Trim().ToLower() &&
                               x.ReqPackageInfoId != reqModel.ReqPackageId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (name)
            {
                model = Result<ReqPackageModel>.Error(JsonResource.WHE075);
                return model;
            }

            name = await _db.TblReqPackageInfoChanges
                .AsNoTracking()
                .AnyAsync(x => x.PackageName.Trim().ToLower() == reqModel.PackageName!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (name)
            {
                model = Result<ReqPackageModel>.Error(JsonResource.WHE076);
                return model;
            }

            #endregion

            #region Check Duplicate Product and Box

            bool item = await _db.TblPackageInfos
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower() &&
                          x.BoxCode.Trim().ToLower() == reqModel.BoxCode!.Trim().ToLower());
            if (item)
            {
                model = Result<ReqPackageModel>.Error(JsonResource.WHE080);
                return model;
            }

            item = await _db.TblReqPackageInfos
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower() &&
                          x.BoxCode.Trim().ToLower() == reqModel.BoxCode!.Trim().ToLower() &&
                          x.ReqPackageInfoId != reqModel.ReqPackageId &&
                          x.Status == EnumRequestedStatus.Pending.ToString());
            if (item)
            {
                model = Result<ReqPackageModel>.Error(JsonResource.WHE081);
                return model;
            }

            item = await _db.TblReqPackageInfoChanges
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower() &&
                          x.BoxCode.Trim().ToLower() == reqModel.BoxCode!.Trim().ToLower() &&
                          x.Status == EnumRequestedStatus.Pending.ToString());
            if (item)
            {
                model = Result<ReqPackageModel>.Error(JsonResource.WHE082);
                return model;
            }

            #endregion

            #region Prepare Data

            reqPackage.PackageName = reqModel.PackageName!;
            reqPackage.PackageInfoCode = reqModel.PackageInfoCode!;
            reqPackage.Quantity = reqModel.Quanity;
            reqPackage.BranchCode = reqModel.BranchCode!;
            reqPackage.ProductCode = reqModel.ProductCode!;
            reqPackage.Price = reqModel.Price!.ToInt32();
            reqPackage.CurrencyCode = reqModel.CurrencyCode!;
            reqPackage.Weight = reqModel.Weight!.ToInt32();
            reqPackage.BoxCode = reqModel.BoxCode!;
            reqPackage.ReqDateTime = DevCode.GetServerDateTime();

            _db.Entry(reqPackage).State = EntityState.Modified;
            _db.TblReqPackageInfos.Update(reqPackage);
            await _db.SaveChangesAsync();
            model = Result<ReqPackageModel>.Success(JsonResource.WHS077);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqPackageModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<ReqPackageModel>> Delete(ReqPackageEditModel reqModel)
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

            bool package = await _db.TblReqPackageInfos
                .AsNoTracking()
                .AnyAsync(x => x.ReqPackageInfoId == reqModel.ReqPackageId &&
                               x.Status != EnumRequestedStatus.Pending.ToString());
            if (package)
            {
                model = Result<ReqPackageModel>.Error(JsonResource.WHE073);
                return model;
            }

            #endregion

            #region Prepare Data

            _db.TblReqPackageInfos.Remove(reqPackage);
            var result = _db.SaveChanges();
            if (result <= 0)
            {
                model = Result<ReqPackageModel>.Error(JsonResource.WHE078);
                return model;
            }
            model = Result<ReqPackageModel>.Success(JsonResource.WHS079);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqPackageModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<ReqPackageDetailModel>> Details(ReqPackageEditModel reqModel)
    {
        ReqPackageDetailModel model = new();
        try
        {
            var detail = await _dapperService.GetDetailAsync<ReqPackageDetailInfoModel>(
                SqlQueries.Sp_GetReqPackageDetail, new
                {
                    ReqPackageId = reqModel.ReqPackageId
                }, CommandType.StoredProcedure);

            List<DynamicReportModel> packageInfo = new List<DynamicReportModel>();
            packageInfo.Add("Package Name", detail.PackageName!);
            packageInfo.Add("Product Name", detail.ProductName!);
            packageInfo.Add("PackageInfo Code", detail.PackageInfoCode!);
            packageInfo.Add("Branch Name", detail.BranchName!);
            packageInfo.Add("Quanity", detail.Quantity!);
            packageInfo.Add("Price", detail.Price!);
            packageInfo.Add("Currency Code", detail.CurrencyCode!);
            packageInfo.Add("Weight", detail.Weight!);
            packageInfo.Add("Box", detail.Box!);
            model.PackageInfo = packageInfo;
            model.ItemImagePath = detail.ImagePath;

            List<DynamicReportModel> makerChecker = new List<DynamicReportModel>();
            makerChecker.Add("Requested User", detail.ReqUser!);
            makerChecker.Add("Requested DateTime", detail.ReqDateTime!);
            makerChecker.Add("Approved User", detail.ApprovedUser!.ToDashFromNull());
            makerChecker.Add("Approved DateTime ", detail.ApprovedDateTime!.ToDashFromNull());
            makerChecker.Add("Status", detail.Status!);
            makerChecker.Add("Reject Reason", detail.RejectReason!.ToDashFromNull());
            model.MakerChecker = makerChecker;

            return Result<ReqPackageDetailModel>.Success(model);
        }
        catch(Exception ex)
        {
            return Result<ReqPackageDetailModel>.Error(ex);
        }
    }

    #endregion

    #region DropDown

    public async Task<Result<List<ProductResponseModel>>> GetProduct()
    {
        try
        {
            var result = await _db.TblProducts
                .AsNoTracking()
                .Where(x => x.DelFlag == 0)
                .Select(x => new ProductResponseModel
                {
                    ProductCode = x.ProductCode,
                    ProductName = x.ProductName
                })
                .ToListAsync();

            return Result<List<ProductResponseModel>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<ProductResponseModel>>.Error(ex);
        }
    }

    public async Task<Result<List<CurrencyResponseModel>>> GetCurrency()
    {
        try
        {
            var result = await _db.TblCurrencies
                .AsNoTracking()
                .Where(x => x.DelFlag == 0)
                .Select(x => new CurrencyResponseModel
                {
                    CurrencyCode = x.CurrencyCode
                })
                .ToListAsync();

            return Result<List<CurrencyResponseModel>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<CurrencyResponseModel>>.Error(ex);
        }
    }

    public async Task<Result<List<BoxResponseModel>>> GetBox()
    {
        try
        {
            var result = await _db.TblBoxes
                .AsNoTracking()
                .Where(x => x.DelFlag == 0)
                .Select(x => new BoxResponseModel
                {
                    BoxCode = x.BoxCode,
                    Box = x.Size
                })
                .ToListAsync();

            return Result<List<BoxResponseModel>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<BoxResponseModel>>.Error(ex);
        }
    }

    #endregion
}
