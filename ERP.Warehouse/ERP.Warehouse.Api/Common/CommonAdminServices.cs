using Microsoft.Extensions.Options;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Models.ConfigModel;

namespace ERP.Warehouse.Api.Common;

public class CommonAdminServices
{
    private readonly AppDbContext _db;
    private readonly ILogger<CommonAdminServices> _logger;
    private readonly CustomSettingModel _customSettingModel;


    public CommonAdminServices(
        AppDbContext db,
        IOptionsMonitor<CustomSettingModel> customSetting,
        ILogger<CommonAdminServices> logger)
    {
        _db = db;
        _logger = logger;
        _customSettingModel = customSetting.CurrentValue;
    }
}
