namespace ERP.Warehouse.Models.Models.WarehouseUserList;

public class WarehouseUserRepModel
{
    public List<WarehouseUserModel> list { get; set; }
    public int TotalRowCount { get; set; }
}

public class WarehouseUserModel
{
    public string WarehouseUserId { get; set; }
    public string FullName { get; set; }
    public string StaffId { get; set; }
    public string PhoneNo { get; set; }
    public string Email { get; set; }
    public string RoleName { get; set; }
    public string BranchName { get; set; }
}
