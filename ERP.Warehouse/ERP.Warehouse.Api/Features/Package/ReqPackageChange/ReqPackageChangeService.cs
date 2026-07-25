using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Package.ReqPackage;
using ERP.Warehouse.Models.Models.Package.ReqPackageChange;
using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Models.ConfigModel;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Package.ReqPackageChange;

public class ReqPackageChangeService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;

    public ReqPackageChangeService(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger,
        CustomSettingModel setting) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
    }

    #region Get/Create/Edit/Update/Delete/Details

    public async Task<Result<ReqPackageChangeRepModel>> Get(ReqPackageChangeReqModel reqModel)
    {
        ReqPackageChangeRepModel model = new();
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
            var result = await _dapperService.QueryStoredProcedureAsync<ReqPackageChangeModel>
                (SqlQueries.Sp_GetReqPackageChangeList, parameters);
            model.list = result;
            return Result<ReqPackageChangeRepModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<ReqPackageChangeRepModel>.Error(ex);
        }
    }

    public async Task<Result<ReqPackageChangeModel>> Edit(ReqPackageChangeEditModel reqModel)
    {
        var model = new Result<ReqPackageChangeModel>();
        try
        {
            #region Check ReqPackageChange

            var reqPackage = await _db.TblReqPackageInfoChanges
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqPackageInfoChangesId == reqModel.ReqPackageChangeId);
            if (reqPackage is null)
            {
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE084);
                return model;
            }

            #endregion

            #region Check Product

            var product = await _db.TblProducts
            .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductCode == reqPackage.ProductCode && x.DelFlag == 0);
            if (product is null)
            {
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE067);
                return model;
            }

            #endregion

            #region Check Currency

            var currency = await _db.TblCurrencies
            .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CurrencyCode == reqPackage.CurrencyCode && x.DelFlag == 0);
            if (currency is null)
            {
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE052);
                return model;
            }

            #endregion

            #region Check Box

            var box = await _db.TblBoxes
            .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BoxCode == reqPackage.BoxCode && x.DelFlag == 0);
            if (product is null)
            {
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE059);
                return model;
            }

            #endregion

            #region Prepare Data

            var response = new ReqPackageChangeModel
            {
                ReqPackageChangeId = reqPackage.ReqPackageInfoChangesId,
                PackageId = reqPackage.PackageInfoId,
                PackageName = reqPackage.PackageName,
                PackageInfoCode = reqPackage.PackageInfoCode,
                ProductCode = reqPackage.ProductCode,
                Price = reqPackage.Price.ToString(),
                CurrencyCode = reqPackage.CurrencyCode,
                Weight = reqPackage.Weight.ToString(),
                BoxCode = reqPackage.BoxCode,
                Status = reqPackage.Status
            };
            model = Result<ReqPackageChangeModel>.Success(response);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqPackageChangeModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<ReqPackageChangeModel>> Update(ReqPackageChangeReqModel reqModel)
    {
        var model = new Result<ReqPackageChangeModel>();
        try
        {
            #region Check ReqPackageChange

            var reqPackage = await _db.TblReqPackageInfoChanges
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqPackageInfoChangesId == reqModel.ReqPackageChangeId);
            if (reqPackage is null)
            {
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE084);
                return model;
            }

            bool package = await _db.TblReqPackageInfoChanges
                .AsNoTracking()
                .AnyAsync(x => x.ReqPackageInfoChangesId == reqModel.ReqPackageChangeId &&
                               x.Status != EnumRequestedStatus.Pending.ToString());
            if (package)
            {
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE085);
                return model;
            }

            #endregion

            #region Check Duplicate PackageName

            bool name = await _db.TblPackageInfos
                .AsNoTracking()
                .AnyAsync(x => x.PackageName.Trim().ToLower() == reqModel.PackageName!.Trim().ToLower());
            if (name)
            {
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE074);
                return model;
            }

            name = await _db.TblReqPackageInfos
                .AsNoTracking()
                .AnyAsync(x => x.PackageName.Trim().ToLower() == reqModel.PackageName!.Trim().ToLower() &&
                               x.ReqPackageInfoId != reqModel.ReqPackageChangeId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (name)
            {
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE075);
                return model;
            }

            name = await _db.TblReqPackageInfoChanges
                .AsNoTracking()
                .AnyAsync(x => x.PackageName.Trim().ToLower() == reqModel.PackageName!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (name)
            {
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE076);
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
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE080);
                return model;
            }

            item = await _db.TblReqPackageInfos
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower() &&
                          x.BoxCode.Trim().ToLower() == reqModel.BoxCode!.Trim().ToLower() &&
                          x.ReqPackageInfoId != reqModel.ReqPackageChangeId &&
                          x.Status == EnumRequestedStatus.Pending.ToString());
            if (item)
            {
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE081);
                return model;
            }

            item = await _db.TblReqPackageInfoChanges
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower() &&
                          x.BoxCode.Trim().ToLower() == reqModel.BoxCode!.Trim().ToLower() &&
                          x.Status == EnumRequestedStatus.Pending.ToString());
            if (item)
            {
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE082);
                return model;
            }

            #endregion

            #region Prepare Data

            reqPackage.PackageName = reqModel.PackageName!;
            reqPackage.PackageInfoCode = reqModel.PackageInfoCode!;
            reqPackage.ProductCode = reqModel.ProductCode!;
            reqPackage.Price = reqModel.Price!.ToInt32();
            reqPackage.CurrencyCode = reqModel.CurrencyCode!;
            reqPackage.Weight = reqModel.Weight!.ToInt32();
            reqPackage.BoxCode = reqModel.BoxCode!;
            reqPackage.ReqDateTime = DevCode.GetServerDateTime();

            _db.Entry(reqPackage).State = EntityState.Modified;
            _db.TblReqPackageInfoChanges.Update(reqPackage);
            await _db.SaveChangesAsync();
            model = Result<ReqPackageChangeModel>.Success(JsonResource.WHS086);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqPackageChangeModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<ReqPackageChangeModel>> Delete(ReqPackageChangeEditModel reqModel)
    {
        var model = new Result<ReqPackageChangeModel>();
        try
        {
            #region Check ReqPackageChange

            var reqPackage = await _db.TblReqPackageInfoChanges
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqPackageInfoChangesId == reqModel.ReqPackageChangeId);
            if (reqPackage is null)
            {
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE084);
                return model;
            }

            bool package = await _db.TblReqPackageInfoChanges
                .AsNoTracking()
                .AnyAsync(x => x.ReqPackageInfoChangesId == reqModel.ReqPackageChangeId &&
                               x.Status != EnumRequestedStatus.Pending.ToString());
            if (package)
            {
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE085);
                return model;
            }

            #endregion

            #region Prepare Data

            _db.TblReqPackageInfoChanges.Remove(reqPackage);
            var result = _db.SaveChanges();
            if (result <= 0)
            {
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE087);
                return model;
            }
            model = Result<ReqPackageChangeModel>.Success(JsonResource.WHS088);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqPackageChangeModel>.Error(ex);
        }
        return model;
    }

    #endregion
}
