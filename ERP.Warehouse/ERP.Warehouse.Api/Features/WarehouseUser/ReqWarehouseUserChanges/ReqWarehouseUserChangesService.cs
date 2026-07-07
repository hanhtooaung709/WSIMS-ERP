using ERP.Warehouse.Api.Common;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.WarehouseUser.ReqWarehouseUserChanges;

public class ReqWarehouseUserChangesService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;

    public ReqWarehouseUserChangesService(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
    }

    #region Get/Create/Edit/Update/Delete/Details



    #endregion
}
