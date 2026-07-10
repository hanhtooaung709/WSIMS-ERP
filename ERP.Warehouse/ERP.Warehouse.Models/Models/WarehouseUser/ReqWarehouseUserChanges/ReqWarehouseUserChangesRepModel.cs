namespace ERP.Warehouse.Models.Models.WarehouseUser.ReqWarehouseUserChanges;

public class ReqWarehouseUserChangesRepModel
{
    public List<ReqWarehouseUserChangesModel> list { get; set; }
}

public class ReqWarehouseUserChangesModel
{
    public string? ReqWarehouseUserChangesId { get; set; }
    public string? WarehouseUserId { get; set; }
    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public string? StaffId { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? RoleCode { get; set; }
    public string? BranchCode { get; set; }
    public string? ChangesType { get; set; }
    public string? Status { get; set; }
}