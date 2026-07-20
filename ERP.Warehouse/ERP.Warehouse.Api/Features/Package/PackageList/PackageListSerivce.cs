using ERP.Warehouse.Api.Common;
using Module.CommonDbService.EfAppDbContextModels;
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



    #endregion
}
