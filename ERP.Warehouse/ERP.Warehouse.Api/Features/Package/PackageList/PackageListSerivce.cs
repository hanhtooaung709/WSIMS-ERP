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
            #region Check User

            var user = await _db.TblWarehouseUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.WarehouseUserId == "01KVNHVBPTSARA9NFQ8NYSQS1F");
            if (user is null)
            {
                return Result<PackageRepModel>.Error(JsonResource.WHE001);
            }

            #endregion

            #region Package

            var userPackages =  await _db.TblPackages
                .AsNoTracking()
                .Where(x => x.BranchCode == user.BranchCode)
                .ToListAsync();

            var existingCodes = userPackages.Select(x => x.PackageInfoCode).ToList();

            var otherPackages = await _db.TblPackages
                .AsNoTracking()
                .Where(x => x.BranchCode != user.BranchCode &&
                            !existingCodes.Contains(x.PackageInfoCode))
                .ToListAsync();

            var distinctOtherPackages = otherPackages
                .GroupBy(x => x.PackageInfoCode)
                .Select(g => g.First());

            #endregion

            #region PackageInfo

            var allPackageInfoCodes = userPackages
                .Concat(distinctOtherPackages)
                .Select(x => x.PackageInfoCode)
                .ToList();

            var packageInfo = await _db.TblPackageInfos
                .AsNoTracking()
                .Where(x => allPackageInfoCodes.Contains(x.PackageInfoCode))
                .ToListAsync();

            var infoDict = packageInfo.ToDictionary(x => x.PackageInfoCode);

            #endregion

            #region Product

            var allProductCodes = packageInfo.Select(x => x.ProductCode).ToList();

            var productCode = await _db.TblProducts
                .AsNoTracking()
                .Where(x => allProductCodes.Contains(x.ProductCode))
                .ToListAsync();

            var product = productCode.ToDictionary(x => x.ProductCode);

            #endregion

            #region Box

            var allBox = packageInfo.Select(x => x.BoxCode).ToList();

            var boxCode = await _db.TblBoxes
                .AsNoTracking()
                .Where(x => allBox.Contains(x.BoxCode))
                .ToListAsync();

            var box = boxCode.ToDictionary(x => x.BoxCode);

            #endregion

            #region Prepare Data

            var combinedPackages = userPackages
                .Concat(distinctOtherPackages)
                .Select(x =>
                {
                    infoDict.TryGetValue(x.PackageInfoCode, out var info);
                    product.TryGetValue(info.ProductCode, out var prod);
                    box.TryGetValue(info.BoxCode, out var boxSize);
                    {
                        return new PackageModel
                        {
                            PackageId = x.PackageId,
                            PackageName = info.PackageName,
                            ProductCode = prod.ProductName,
                            Price = info.Price.ToString(),
                            CurrencyCode = info.CurrencyCode,
                            Weight = info.Weight.ToString(),
                            BoxCode = boxSize.Size,
                            PackageInfoCode = x.PackageInfoCode,
                            Quanity = x.BranchCode != user.BranchCode ? "0" : x.Quanity.ToString(),
                        };
                    }
                    
                })
                .ToList();

            model.list = combinedPackages;

            return Result<PackageRepModel>.Success(model);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<PackageRepModel>.Error(ex);
        }
    }

    #endregion
}
