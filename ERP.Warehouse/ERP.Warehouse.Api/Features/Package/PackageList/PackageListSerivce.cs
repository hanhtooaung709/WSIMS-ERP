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
using WSIMS_ERP.Shared.Enums;
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

    public async Task<Result<PackageModel>> Edit(PackageEditModel reqModel)
    {
        var model = new Result<PackageModel>();
        try
        {
            #region Check Package

            var packageInfo = await _db.TblPackageInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PackageInfoId == reqModel.PackageId);
            if (packageInfo is null)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE083);
                return model;
            }

            var package = await _db.TblPackages
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PackageInfoCode == reqModel.PackageInfoCode);
            if (package is null)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE083);
                return model;
            }

            #endregion

            #region Check Branch

            var branch = await _db.TblBranches
            .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BranchCode == package.BranchCode && x.DelFlag == 0);
            if (branch is null)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE016);
                return model;
            }

            #endregion

            #region Check Product

            var product = await _db.TblProducts
            .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductCode == packageInfo.ProductCode && x.DelFlag == 0);
            if (product is null)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE067);
                return model;
            }

            #endregion

            #region Check Currency

            var currency = await _db.TblCurrencies
            .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CurrencyCode == packageInfo.CurrencyCode && x.DelFlag == 0);
            if (currency is null)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE067);
                return model;
            }

            #endregion

            #region Check Box

            var box = await _db.TblBoxes
            .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BoxCode == packageInfo.BoxCode && x.DelFlag == 0);
            if (product is null)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE059);
                return model;
            }

            #endregion

            #region Prepare Data

            var response = new PackageModel
            {
                PackageId = packageInfo.PackageInfoId,
                PackageName = packageInfo.PackageName,
                PackageInfoCode = packageInfo.PackageInfoCode,
                BranchCode = package.BranchCode,
                Quanity = package.Quanity.ToString(),
                ProductCode = packageInfo.ProductCode,
                Price = packageInfo.Price.ToString(),
                CurrencyCode = packageInfo.CurrencyCode,
                Weight = packageInfo.Weight.ToString(),
                BoxCode = packageInfo.BoxCode,
                ImagePath = product.ImagePath
            };
            model = Result<PackageModel>.Success(response);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<PackageModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<PackageModel>> Update(PackageReqModel reqModel)
    {
        var model = new Result<PackageModel>();
        try
        {
            #region Check Package

            var packageInfo = await _db.TblPackageInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PackageInfoId == reqModel.PackageId);
            if (packageInfo is null)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE083);
                return model;
            }

            #endregion

            #region Check Duplicate PackageName

            bool name = await _db.TblPackageInfos
                .AsNoTracking()
                .AnyAsync(x => x.PackageName.Trim().ToLower() == reqModel.PackageName!.Trim().ToLower() &&
                          x.PackageInfoId != reqModel.PackageId);
            if (name)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE074);
                return model;
            }

            name = await _db.TblReqPackageInfos
                .AsNoTracking()
                .AnyAsync(x => x.PackageName.Trim().ToLower() == reqModel.PackageName!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (name)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE075);
                return model;
            }

            name = await _db.TblReqPackageInfoChanges
                .AsNoTracking()
                .AnyAsync(x => x.PackageName.Trim().ToLower() == reqModel.PackageName!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (name)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE076);
                return model;
            }

            #endregion

            #region Check Duplicate Product and Box

            bool item = await _db.TblPackageInfos
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower() &&
                          x.BoxCode.Trim().ToLower() == reqModel.BoxCode!.Trim().ToLower() &&
                          x.PackageInfoId != reqModel.PackageId);
            if (item)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE080);
                return model;
            }

            item = await _db.TblReqPackageInfos
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower() &&
                          x.BoxCode.Trim().ToLower() == reqModel.BoxCode!.Trim().ToLower() &&
                          x.Status == EnumRequestedStatus.Pending.ToString());
            if (item)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE081);
                return model;
            }

            item = await _db.TblReqPackageInfoChanges
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower() &&
                          x.BoxCode.Trim().ToLower() == reqModel.BoxCode!.Trim().ToLower() &&
                          x.Status == EnumRequestedStatus.Pending.ToString());
            if (item)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE082);
                return model;
            }

            #endregion

            #region Prepare Data

            TblReqPackageInfoChange result = new TblReqPackageInfoChange
            {
                ReqPackageInfoChangesId = DevCode.GenerateUlid(),
                PackageName = reqModel.PackageName!,
                ProductCode = reqModel.ProductCode!,
                Price = reqModel.Price!.ToInt32(),
                CurrencyCode = reqModel.CurrencyCode!,
                Weight = reqModel.Weight!.ToInt32(),
                BoxCode = reqModel.BoxCode!,
                ChangesType = EnumRequestedType.Update.ToString(),
                Status = EnumRequestedStatus.Pending.ToString(),
                ReqUserId = AuthorizedUserId,
                ReqDateTime = DevCode.GetServerDateTime()
            };
            await _db.TblReqPackageInfoChanges.AddAsync(result);
            await _db.SaveChangesAsync();
            model = Result<PackageModel>.Success(JsonResource.WHS014);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<PackageModel>.Error(ex);
        }
        return model;
    }

    #endregion
}
