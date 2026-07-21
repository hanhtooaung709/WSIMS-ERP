using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.VariantTypes;
using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Currency;
using ERP.Warehouse.Models.Models.Package.PackageList;
using ERP.Warehouse.Models.Models.Package.ReqPackage;
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
            var user = await _db.TblWarehouseUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.WarehouseUserId == "01KVNHVBPTSARA9NFQ8NYSQS1F");
            if (user is null)
            {
                return Result<PackageRepModel>.Error(JsonResource.WHE001);
            }

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

            var combinedPackages = userPackages
                .Concat(distinctOtherPackages)
                .Select(item => new PackageModel
                {
                    PackageId = item.PackageId,
                    PackageInfoCode = item.PackageInfoCode,
                    Quanity = item.BranchCode != user.BranchCode ? "0" : item.Quanity.ToString(),
                })
                .ToList();

            model.list = combinedPackages;

            return Result<PackageRepModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<PackageRepModel>.Error(ex);
        }
    }

    #endregion
}
