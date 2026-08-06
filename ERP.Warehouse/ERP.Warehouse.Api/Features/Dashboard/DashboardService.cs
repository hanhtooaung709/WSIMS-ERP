using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Dashboard;
using ERP.Warehouse.Models.Models.Dashboard.Box;
using ERP.Warehouse.Models.Models.Dashboard.Product;
using ERP.Warehouse.Models.Models.Package.PackageList;
using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Models.ConfigModel;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Dashboard;

public class DashboardService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;

    public DashboardService(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger,
        CustomSettingModel setting) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
    }

    public async Task<Result<DashboardModel>> GetDashboardData()
    {
        DashboardModel model = new();
        try
        {
            #region GetProductName

            var productName = await GetProductName();

            if (productName.IsSuccess && productName.Data != null)
            {
                model.ProductName = productName.Data.Select(x => x.ProductName).ToList();
            }

            #endregion

            #region GetBoxType

            var boxType = await GetBoxType();
            if (boxType.IsSuccess && boxType.Data != null)
            {
                model.Boxes = boxType.Data;
            }

            #endregion

            #region GetStock

            var stockQuantity = await GetStock();

            if (stockQuantity.IsSuccess && stockQuantity.Data?.list != null)
            {
                model.Packages = stockQuantity.Data.list;
                model.StockQty = stockQuantity.Data.list.Select(x => x.Quantity).ToList();
            }

            #endregion

            #region GetProductCount

            var productCount = await GetProductCount();

            if (productCount.IsSuccess)
            {
                model.ProductCount = productCount.Data;
            }

            #endregion

            #region GetPackageCount

            var packageCount = await GetPackageCount();

            if (packageCount.IsSuccess)
            {
                model.PackageCount = packageCount.Data;
            }

            #endregion

            return Result<DashboardModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<DashboardModel>.Error(ex);
        }
    }

    private async Task<Result<List<ProductResponceModel>>> GetProductName()
    {
        try
        {
            var result = await _db.TblProducts
                .AsNoTracking()
                .Where(x => x.DelFlag == 0)
                .OrderByDescending(x => x.CreatedDateTime)
                .Select(x => new ProductResponceModel
                {
                    ProductName = x.ProductName
                })
                .ToListAsync();

            return Result<List<ProductResponceModel>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<ProductResponceModel>>.Error(ex);
        }
    }

    private async Task<Result<List<BoxResponseModel>>> GetBoxType()
    {
        try
        {
            var result = await _db.TblBoxes
                .AsNoTracking()
                .Where(x => x.DelFlag == 0)
                .OrderByDescending(x => x.Size)
                .Select(x => new BoxResponseModel
                {
                    BoxType = x.Type,
                    BoxCode = x.BoxCode
                })
                .ToListAsync();

            return Result<List<BoxResponseModel>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<BoxResponseModel>>.Error(ex);
        }
    }

    public async Task<Result<PackageRepModel>> GetStock()
    {
        PackageRepModel model = new();
        try
        {
            var parameters = new
            {
                CurrentUserId = AuthorizedUserId
            };
            var result = await _dapperService.QueryStoredProcedureAsync<PackageModel>
                (SqlQueries.Sp_GetStockQuantity, parameters);
            model.list = result;
            return Result<PackageRepModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<PackageRepModel>.Error(ex);
        }
    }

    private async Task<Result<int>> GetProductCount()
    {
        try
        {
            var count = await _db.TblProducts
                .AsNoTracking()
                .Where(x => x.DelFlag == 0)
                .CountAsync();

            return Result<int>.Success(count);
        }
        catch (Exception ex)
        {
            return Result<int>.Error(ex);
        }
    }

    private async Task<Result<int>> GetPackageCount()
    {
        try
        {
            var count = await _db.TblPackageInfos
                .AsNoTracking()
                .Where(x => x.DelFlag == 0)
                .CountAsync();

            return Result<int>.Success(count);
        }
        catch (Exception ex)
        {
            return Result<int>.Error(ex);
        }
    }
}
