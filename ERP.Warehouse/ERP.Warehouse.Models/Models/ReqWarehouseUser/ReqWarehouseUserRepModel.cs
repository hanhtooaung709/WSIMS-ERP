namespace ERP.Warehouse.Models.Models.ReqWarehouseUser;

public class ReqWarehouseUserRepModel
{
    public List<ReqWarehouseUserModel> list {  get; set; }
}

public class ReqWarehouseUserModel
{
    public string? ReqWarehouseUserId { get; set; }
    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public string? StaffId { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? RoleCode { get; set; }
    public string? BranchCode { get; set; }
    public string? Status { get; set; }
}