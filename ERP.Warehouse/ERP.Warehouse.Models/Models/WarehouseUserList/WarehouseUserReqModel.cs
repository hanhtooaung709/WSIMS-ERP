using WSIMS_ERP.Shared.Models.PageSetting;

namespace ERP.Warehouse.Models.Models.WarehouseUserList;

public class WarehouseUserReqModel
{
    public string? FullName { get; set; }
    public string? StaffId { get; set; }
    public string? PhoneNo { get; set; }
    public string? Email { get; set; }
    public string? RoleName { get; set; }
    public string? BranchName { get; set; }
}
