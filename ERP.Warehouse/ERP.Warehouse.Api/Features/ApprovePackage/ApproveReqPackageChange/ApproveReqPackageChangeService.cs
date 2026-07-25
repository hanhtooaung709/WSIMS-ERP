using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models;
using WSIMS_ERP.Shared.Models.DynamicModel;
using ERP.Warehouse.Models.Models.Package.ReqPackageChange;
using Module.CommonDbService.EfAppDbContextModels;
using System.Data;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Models.ConfigModel;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;
using WSIMS_ERP.Shared;
using ERP.Warehouse.Models.Models.Product.ReqProductChanges;
using Microsoft.EntityFrameworkCore;
using WSIMS_ERP.Shared.Enums;

namespace ERP.Warehouse.Api.Features.ApprovePackage.ApproveReqPackageChange;

public class ApproveReqPackageChangeService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;

    public ApproveReqPackageChangeService(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger,
        CustomSettingModel setting) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
    }

    #region Get/Approve/Reject/Details

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
                (SqlQueries.Sp_GetApproveReqPackageChangeList, parameters);
            model.list = result;
            return Result<ReqPackageChangeRepModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<ReqPackageChangeRepModel>.Error(ex);
        }
    }

    public async Task<Result<ReqPackageChangeModel>> Approve(ReqPackageChangeEditModel reqModel)
    {
        var model = new Result<ReqPackageChangeModel>();
        try
        {
            #region Check Package

            TblPackageInfo? package = await _db.TblPackageInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PackageInfoId == reqModel.PackageId);
            if (package is null)
            {
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE083);
                return model;
            }

            TblReqPackageInfoChange? packageChanges = await _db.TblReqPackageInfoChanges
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqPackageInfoChangesId == reqModel.ReqPackageChangeId);
            if (packageChanges is null)
            {
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE084);
                return model;
            }

            bool reqPackage = await _db.TblReqPackageInfoChanges
                .AsNoTracking()
                .AnyAsync(x => x.ReqPackageInfoChangesId == reqModel.ReqPackageChangeId &&
                               x.Status != EnumRequestedStatus.Pending.ToString());
            if (reqPackage)
            {
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE085);
                return model;
            }

            #endregion

            #region Prepare Data

            if (packageChanges!.ChangesType == EnumRequestedType.Update.ToString())
            {
                package.PackageName = packageChanges.PackageName!;
                package.PackageInfoCode = packageChanges.PackageInfoCode!;
                package.ProductCode = packageChanges.ProductCode!;
                package.Price = packageChanges.Price!;
                package.CurrencyCode = packageChanges.CurrencyCode!;
                package.Weight = packageChanges.Weight!;
                package.BoxCode = packageChanges.BoxCode!;
                package.ModifiedUserId = AuthorizedUserId;
                package.ModifiedDateTime = DevCode.GetServerDateTime();
                _db.Entry(package).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            else
            {
                package.DelFlag = 1;
                _db.Entry(package).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }

            packageChanges.Status = EnumRequestedStatus.Approved.ToString();
            packageChanges.ApprovedUserId = AuthorizedUserId;
            packageChanges.ApprovedDateTime = DevCode.GetServerDateTime();
            _db.Entry(packageChanges).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            model = Result<ReqPackageChangeModel>.Success(JsonResource.WHS091);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqPackageChangeModel>.Error(ex);
        }

        return model;
    }

    public async Task<Result<ReqPackageChangeModel>> Reject(ReqPackageChangeEditModel reqModel)
    {
        var model = new Result<ReqPackageChangeModel>();
        try
        {
            #region Check Package

            TblPackageInfo? package = await _db.TblPackageInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PackageInfoId == reqModel.PackageId);
            if (package is null)
            {
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE083);
                return model;
            }

            TblReqPackageInfoChange? packageChanges = await _db.TblReqPackageInfoChanges
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqPackageInfoChangesId == reqModel.ReqPackageChangeId);
            if (packageChanges is null)
            {
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE084);
                return model;
            }

            bool reqPackage = await _db.TblReqPackageInfoChanges
                .AsNoTracking()
                .AnyAsync(x => x.ReqPackageInfoChangesId == reqModel.ReqPackageChangeId &&
                               x.Status != EnumRequestedStatus.Pending.ToString());
            if (reqPackage)
            {
                model = Result<ReqPackageChangeModel>.Error(JsonResource.WHE085);
                return model;
            }

            #endregion

            #region Prepare Data

            packageChanges!.Status = EnumRequestedStatus.Rejected.ToString();
            packageChanges!.RejectReason = reqModel.RejectReason;
            packageChanges.ApprovedUserId = AuthorizedUserId;
            packageChanges.ApprovedDateTime = DevCode.GetServerDateTime();
            _db.Entry(packageChanges).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            model = Result<ReqPackageChangeModel>.Success(JsonResource.WHS092);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqPackageChangeModel>.Error(ex);
        }

        return model;
    }

    public async Task<Result<ReqPackageChangeDetailModel>> Details(ReqPackageChangeEditModel reqModel)
    {
        ReqPackageChangeDetailModel model = new();
        try
        {
            var detail = await _dapperService.GetDetailAsync<ReqPackageChangeDetailInfoModel>(
                SqlQueries.Sp_GetReqPackageChangeDetail, new
                {
                    ReqPackageChangeId = reqModel.ReqPackageChangeId
                }, CommandType.StoredProcedure);

            List<DynamicReportModel> packageInfo = new List<DynamicReportModel>();
            packageInfo.Add("Package Name", detail.PackageName!);
            packageInfo.Add("Product Name", detail.ProductName!);
            packageInfo.Add("PackageInfo Code", detail.PackageInfoCode!);
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

            return Result<ReqPackageChangeDetailModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<ReqPackageChangeDetailModel>.Error(ex);
        }
    }

    #endregion
}
