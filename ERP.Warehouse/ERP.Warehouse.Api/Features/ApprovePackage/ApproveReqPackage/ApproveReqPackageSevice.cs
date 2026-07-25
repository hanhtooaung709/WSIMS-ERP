using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models;
using ERP.Warehouse.Models.Models.Branch;
using ERP.Warehouse.Models.Models.Currency;
using ERP.Warehouse.Models.Models.Package.ReqPackage;
using ERP.Warehouse.Models.Models.Product.ReqProduct;
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

namespace ERP.Warehouse.Api.Features.ApprovePackage.ApproveReqPackage;

public class ApproveReqPackageSevice : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;
    private readonly CustomSettingModel _setting;

    public ApproveReqPackageSevice(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger,
        CustomSettingModel setting) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
        _setting = setting;
    }

    #region Get/Approve/Reject/Details

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

    public async Task<Result<ReqPackageModel>> Approve(ReqPackageEditModel reqModel)
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

            #region Add TblPackageInfo

            var packageInfoId = DevCode.GenerateUlid();
            TblPackageInfo item = new TblPackageInfo
            {
                PackageInfoId = packageInfoId,
                PackageName = reqPackage.PackageName,
                PackageInfoCode = reqPackage.PackageInfoCode,
                ProductCode = reqPackage.ProductCode,
                Price = reqPackage.Price,
                CurrencyCode = reqPackage.CurrencyCode,
                Weight = reqPackage.Weight,
                BoxCode = reqPackage.BoxCode,
                CreatedUserId = AuthorizedUserId,
                CreatedDateTime = DevCode.GetServerDateTime(),
            };

            await _db.TblPackageInfos.AddAsync(item);
            await _db.SaveChangesAsync();

            reqPackage.Status = EnumRequestedStatus.Approved.ToString();
            reqPackage.PackageInfoId = packageInfoId;
            reqPackage.ApprovedUserId = AuthorizedUserId;
            reqPackage.ApprovedDateTime = DevCode.GetServerDateTime();
            _db.Entry(reqPackage).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            #endregion

            #region Add TblPackage

            TblPackage item2 = new TblPackage
            {
                PackageId = DevCode.GenerateUlid(),
                PackageInfoCode = reqPackage.PackageInfoCode,
                Quanity = reqPackage.Quanity,
                BranchCode = reqPackage.BranchCode,
                CreatedUserId = AuthorizedUserId,
                CreatedDateTime = DevCode.GetServerDateTime(),
            };

            await _db.TblPackages.AddAsync(item2);
            await _db.SaveChangesAsync();

            model = Result<ReqPackageModel>.Success(JsonResource.WHS089);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqPackageModel>.Error(ex);
        }

        return model;
    }

    public async Task<Result<ReqPackageModel>> Reject(ReqPackageEditModel reqModel)
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

            reqPackage!.Status = EnumRequestedStatus.Rejected.ToString();
            reqPackage.ApprovedUserId = AuthorizedUserId;
            reqPackage.ApprovedDateTime = DevCode.GetServerDateTime();
            reqPackage.RejectReason = reqModel.RejectReason;
            _db.Entry(reqPackage).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            model = Result<ReqPackageModel>.Success(JsonResource.WHS090);

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
            packageInfo.Add("Quanity", detail.Quanity!);
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
        catch (Exception ex)
        {
            return Result<ReqPackageDetailModel>.Error(ex);
        }
    }

    #endregion
}
