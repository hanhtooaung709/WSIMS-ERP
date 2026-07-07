namespace ERP.Warehouse.Models.Models.WarehouseUser.WarehouseUserList;

public class WarehouseUserRepModel
{
    public List<WarehouseUserModel> list { get; set; }
}

public class WarehouseUserModel
{
    public string? WarehouseUserId { get; set; }
    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public string? StaffId { get; set; }
    public string? PhoneNo { get; set; }
    public string? Email { get; set; }
    public string? RoleCode { get; set; }
    public string? BranchCode { get; set; }
}
