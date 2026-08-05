using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Dashboard;
using ERP.Warehouse.Models.Models.Dashboard.Box;
using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Models.ConfigModel;
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
            #region GetBoxType

            var boxType = await GetBoxType();

            if (boxType.IsSuccess && boxType.Data != null)
            {
                model.BoxType = boxType.Data.Select(x => x.BoxType).ToList();
            }

            #endregion

            return Result<DashboardModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<DashboardModel>.Error(ex);
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
                    BoxType = x.Type
                })
                .ToListAsync();

            return Result<List<BoxResponseModel>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<BoxResponseModel>>.Error(ex);
        }
    }
}
