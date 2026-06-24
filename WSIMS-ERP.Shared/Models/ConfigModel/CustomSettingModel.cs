namespace WSIMS_ERP.Shared.Models.ConfigModel;

public class CustomSettingModel
{
    public string DbConnection { get; set; }
    public WarehouseAppSetting WarehouseApp { get; set; }
    public WarehouseApiSetting WarehouseApi { get; set; }
    public JwtModel Jwt { get; set; }
}
