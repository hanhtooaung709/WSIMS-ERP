using Microsoft.Extensions.Options;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Models.ConfigModel;

namespace ERP.Warehouse.Api.Common;

public class CommonServices
{
    private readonly AppDbContext _db;
    private readonly ILogger<CommonServices> _logger;
    private readonly CustomSettingModel _customSettingModel;


    public CommonServices(
        AppDbContext db,
        IOptionsMonitor<CustomSettingModel> customSetting,
        ILogger<CommonServices> logger)
    {
        _db = db;
        _logger = logger;
        _customSettingModel = customSetting.CurrentValue;
    }
}
