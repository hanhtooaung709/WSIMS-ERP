using WSIMS_ERP.Shared.Models.PageSetting;

namespace ERP.Warehouse.Models.Models.WarehouseUserList;

public class WarehouseUserReqModel
{
    public string? UserId { get; set; }
    public string? FullName { get; set; }
    public string? StaffId { get; set; }
    public string? PhoneNo { get; set; }
    public string? Email { get; set; }
    public string? RoleCode { get; set; }
    public string? BranchCode { get; set; }
}
