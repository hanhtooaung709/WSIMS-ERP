using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models;
using ERP.Warehouse.Models.Models.Package.PackageList;
using WSIMS_ERP.Shared.Models.DynamicModel;
using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;
using System.Data;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Models.ConfigModel;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;
using ERP.Warehouse.Models.Models.WarehouseUser.WarehouseUserList;

namespace ERP.Warehouse.Api.Features.Package.PackageList;

public class PackageListService : AuthorizationService
{

    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;

    public PackageListService(IHttpContextAccessor httpContextAccessor,
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

    public async Task<Result<PackageModel>> Create(PackageReqModel reqModel)
    {
        var model = new Result<PackageModel>();
        try
        {
            #region Check User

            var user = await _db.TblWarehouseUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.WarehouseUserId == AuthorizedUserId);
            if (user is null)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE001);
                return model;
            }

            #endregion

            #region Check Duplicate PackageName

            bool name = await _db.TblPackageInfos
                .AsNoTracking()
                .AnyAsync(x => x.PackageName.Trim().ToLower() == reqModel.PackageName!.Trim().ToLower());
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
                               x.BoxCode.Trim().ToLower() == reqModel.BoxCode!.Trim().ToLower());
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

            TblReqPackageInfo result = new TblReqPackageInfo
            {
                ReqPackageInfoId = DevCode.GenerateUlid(),
                PackageName = reqModel.PackageName!,
                PackageInfoCode = reqModel.PackageInfoCode!,
                Quantity = reqModel.Quantity,
                BranchCode = user.BranchCode!,
                ProductCode = reqModel.ProductCode!,
                Price = reqModel.Price!.ToInt32(),
                CurrencyCode = reqModel.CurrencyCode!,
                Weight = reqModel.Weight!.ToInt32(),
                BoxCode = reqModel.BoxCode!,
                Status = EnumRequestedStatus.Pending.ToString(),
                ReqUserId = AuthorizedUserId,
                ReqDateTime = DevCode.GetServerDateTime()
            };
            await _db.TblReqPackageInfos.AddAsync(result);
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

    public async Task<Result<PackageModel>> Edit(PackageEditModel reqModel)
    {
        var model = new Result<PackageModel>();
        try
        {
            #region Check User

            var user = await _db.TblWarehouseUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.WarehouseUserId == AuthorizedUserId);
            if (user is null)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE001);
                return model;
            }

            #endregion

            #region Check Package

            var packageInfo = await _db.TblPackageInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PackageInfoId == reqModel.PackageInfoId &&
                                          x.DelFlag == 0);
            if (packageInfo is null)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE083);
                return model;
            }

            var package = await _db.TblPackages
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PackageId == reqModel.PackageId &&
                                          x.DelFlag == 0);
            if (packageInfo is null)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE083);
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

            int packageQut = await _db.TblPackages
                .AsNoTracking()
                .Where(x => x.PackageInfoCode == reqModel.PackageInfoCode &&
                            x.BranchCode == user.BranchCode &&
                            x.DelFlag == 0)
                .Select(x => x.Quantity)
                .FirstOrDefaultAsync();

            var response = new PackageModel
            {
                PackageInfoId = packageInfo.PackageInfoId,
                PackageId = package.PackageId!,
                PackageName = packageInfo.PackageName,
                PackageInfoCode = packageInfo.PackageInfoCode,
                BranchCode = user.BranchCode,
                Quantity = packageQut,
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
                .FirstOrDefaultAsync(x => x.PackageInfoId == reqModel.PackageInfoId &&
                                          x.DelFlag == 0);
            if (packageInfo is null)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE083);
                return model;
            }

            #endregion

            #region Check Duplicate Id

            bool reqPackage = await _db.TblReqPackageInfoChanges
                .AsNoTracking()
                .AnyAsync(x => x.PackageInfoId == reqModel.PackageInfoId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (reqPackage)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE093);
                return model;
            }

            #endregion

            #region Check Duplicate PackageName

            bool name = await _db.TblPackageInfos
                .AsNoTracking()
                .AnyAsync(x => x.PackageName.Trim().ToLower() == reqModel.PackageName!.Trim().ToLower() &&
                          x.PackageInfoId != reqModel.PackageInfoId);
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
                               x.PackageInfoId != reqModel.PackageInfoId);
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

            #region Check Change Data

            if (packageInfo.PackageInfoId == reqModel.PackageInfoId && packageInfo.PackageName == reqModel.PackageName &&
                packageInfo.PackageInfoCode == reqModel.PackageInfoCode && packageInfo.ProductCode == reqModel.ProductCode &&
                packageInfo.Price == reqModel.Price!.ToInt32() && packageInfo.CurrencyCode == reqModel.CurrencyCode &&
                packageInfo.Weight == reqModel.Weight!.ToInt32() && packageInfo.BoxCode == reqModel.BoxCode)
            {
                model = Result<PackageModel>.Warning(JsonResource.WHE101);
                return model;
            }

            #endregion

            #region Prepare Data

            TblReqPackageInfoChange result = new TblReqPackageInfoChange
            {
                ReqPackageInfoChangesId = DevCode.GenerateUlid(),
                PackageInfoId = reqModel.PackageInfoId!,
                PackageName = reqModel.PackageName!,
                PackageInfoCode = reqModel.PackageInfoCode!,
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

    public async Task<Result<PackageModel>> Delete(PackageEditModel reqModel)
    {
        var model = new Result<PackageModel>();
        try
        {
            #region Check Package

            var packageInfo = await _db.TblPackageInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PackageInfoId == reqModel.PackageInfoId &&
                                          x.DelFlag == 0);
            if (packageInfo is null)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE083);
                return model;
            }

            #endregion

            #region Check Duplicate Id

            bool reqPackage = await _db.TblReqPackageInfoChanges
                .AsNoTracking()
                .AnyAsync(x => x.PackageInfoId == reqModel.PackageInfoId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (reqPackage)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE093);
                return model;
            }

            #endregion

            #region Prepare Data

            TblReqPackageInfoChange item = new TblReqPackageInfoChange
            {
                ReqPackageInfoChangesId = DevCode.GenerateUlid(),
                PackageInfoId = packageInfo.PackageInfoId,
                PackageName = packageInfo.PackageName,
                PackageInfoCode = packageInfo.PackageInfoCode,
                ProductCode = packageInfo.ProductCode,
                Price = packageInfo.Price,
                CurrencyCode = packageInfo.CurrencyCode,
                Weight = packageInfo.Weight,
                BoxCode = packageInfo.BoxCode,
                ChangesType = EnumRequestedType.Delete.ToString(),
                Status = EnumRequestedStatus.Pending.ToString(),
                ReqUserId = AuthorizedUserId,
                ReqDateTime = DevCode.GetServerDateTime()
            };
            await _db.TblReqPackageInfoChanges.AddAsync(item);
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

    public async Task<Result<PackageDetailModel>> Details(PackageEditModel reqModel)
    {
        PackageDetailModel model = new PackageDetailModel();
        try
        {
            #region Check User

            var user = await _db.TblWarehouseUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.WarehouseUserId == AuthorizedUserId &&
                                          x.DelFlag == 0);
            if (user is null)
            {
                return Result<PackageDetailModel>.Error(JsonResource.WHE001);
            }

            #endregion

            var detail = await _dapperService.GetDetailAsync<PackageDetailInfoModel>(
               SqlQueries.Sp_GetPackageDetail, new
               {
                   BranchCode = user.BranchCode,
                   PackageId = reqModel.PackageInfoId
               }, CommandType.StoredProcedure);

            List<DynamicReportModel> packageInfo = new List<DynamicReportModel>();
            packageInfo.Add("Package Name", detail.PackageName!);
            packageInfo.Add("Product Name", detail.ProductName!);
            packageInfo.Add("PackageInfo Code", detail.PackageInfoCode!);
            packageInfo.Add("Quantity", detail.Quantity!);
            packageInfo.Add("Price", detail.Price!);
            packageInfo.Add("Currency Code", detail.CurrencyCode!);
            packageInfo.Add("Weight", detail.Weight!);
            packageInfo.Add("Box", detail.Box!);
            model.PackageInfo = packageInfo;
            model.ItemImagePath = detail.ImagePath;

            List<DynamicReportModel> makerChecker = new List<DynamicReportModel>();
            makerChecker.Add("Created User", detail.CreatedUser!);
            makerChecker.Add("Created DateTime", detail.CreatedDateTime!);
            makerChecker.Add("Modified User", detail.ModifiedUser!.ToDashFromNull());
            makerChecker.Add("Modified DateTime ", detail.ModifiedDateTime!.ToDashFromNull());
            model.MakerChecker = makerChecker;

            return Result<PackageDetailModel>.Success(model);
        }
        catch(Exception ex)
        {
            return Result<PackageDetailModel>.Error(ex);
        }
    }

    #endregion

    #region StockModifly/StockTransfer

    public async Task<Result<PackageModel>> StockModifly(PackageReqModel reqModel)
    {
        var model = new Result<PackageModel>();
        try
        {
            #region Check User

            var user = await _db.TblWarehouseUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.WarehouseUserId == AuthorizedUserId);
            if (user is null)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE001);
                return model;
            }

            #endregion

            #region Check Package

            var packageInfo = await _db.TblPackageInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PackageInfoId == reqModel.PackageInfoId &&
                                          x.DelFlag == 0);
            if (packageInfo is null)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE083);
                return model;
            }

            var package = await _db.TblPackages
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PackageId == reqModel.PackageId &&
                                          x.DelFlag == 0);
            if (package is null)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE083);
                return model;
            }

            #endregion

            #region Check Duplicate Id

            bool reqPackage = await _db.TblReqPackageInfoChanges
                .AsNoTracking()
                .AnyAsync(x => x.PackageInfoCode == reqModel.PackageInfoCode &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (reqPackage)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE093);
                return model;
            }

            reqPackage = await _db.TblReqPackages
                .AsNoTracking()
                .AnyAsync(x => x.PackageId == reqModel.PackageId &&
                               x.ChangesType == EnumRequestedType.Update.ToString() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (reqPackage)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE093);
                return model;
            }

            #endregion

            #region Check Stock Quantity

            if (reqModel.Quantity == 0)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE100);
                return model;
            }

            #endregion

            #region Prepare Data

            TblReqPackage result = new TblReqPackage
            {
                ReqPackageId = DevCode.GenerateUlid(),
                PackageId = reqModel.PackageId!,
                PackageInfoCode = reqModel.PackageInfoCode!,
                Quantity = reqModel.Quantity,
                BranchCode = user.BranchCode!,
                ChangesType = EnumRequestedType.Update.ToString(),
                Status = EnumRequestedStatus.Pending.ToString(),
                ReqUserId = AuthorizedUserId,
                ReqDateTime = DevCode.GetServerDateTime()
            };
            await _db.TblReqPackages.AddAsync(result);
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

    public async Task<Result<PackageModel>> StockTransfer(PackageReqModel reqModel)
    {
        var model = new Result<PackageModel>();
        try
        {
            #region Check Package

            var packageInfo = await _db.TblPackageInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PackageInfoId == reqModel.PackageInfoId &&
                                          x.DelFlag == 0);
            if (packageInfo is null)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE083);
                return model;
            }

            var package = await _db.TblPackages
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PackageId == reqModel.PackageId &&
                                          x.DelFlag == 0);
            if (package is null)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE083);
                return model;
            }

            #endregion

            #region Check Duplicate Id

            bool reqPackage = await _db.TblReqPackageInfoChanges
                .AsNoTracking()
                .AnyAsync(x => x.PackageInfoCode == reqModel.PackageInfoCode &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (reqPackage)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE093);
                return model;
            }

            #endregion

            #region Check Stock Quantity & Branch

            if (reqModel.Quantity == 0)
            {
                model = Result<PackageModel>.Error(JsonResource.WHE098);
                return model;
            }

            if (reqModel.BranchCode.IsNullOrEmpty())
            {
                model = Result<PackageModel>.Error(JsonResource.WHE099);
                return model;
            }

            #endregion

            #region Prepare Data

            TblReqPackage result = new TblReqPackage
            {
                ReqPackageId = DevCode.GenerateUlid(),
                PackageId = reqModel.PackageId!,
                PackageInfoCode = reqModel.PackageInfoCode!,
                Quantity = reqModel.Quantity,
                BranchCode = reqModel.BranchCode!,
                ChangesType = EnumRequestedType.Transfer.ToString(),
                Status = EnumRequestedStatus.Pending.ToString(),
                ReqUserId = AuthorizedUserId,
                ReqDateTime = DevCode.GetServerDateTime()
            };
            await _db.TblReqPackages.AddAsync(result);
            await _db.SaveChangesAsync();

            package.Quantity -= reqModel.Quantity;
            _db.Entry(package).State = EntityState.Modified;
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

    #region Get Other Branch

    public async Task<Result<List<OtherBranchResponseModel>>> GetOtherBranch()
    {
        try
        {
            var user = await _db.TblWarehouseUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.WarehouseUserId == AuthorizedUserId &&
                                          x.DelFlag == 0);

            var result = await _db.TblBranches
                .AsNoTracking()
                .Where(x => x.DelFlag == 0 && x.BranchCode != user.BranchCode)
                .Select(x => new OtherBranchResponseModel
                {
                    Address = x.Address,
                    BranchCode = x.BranchCode
                })
                .ToListAsync();

            return Result<List<OtherBranchResponseModel>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<OtherBranchResponseModel>>.Error(ex);
        }
    }

    #endregion
}
