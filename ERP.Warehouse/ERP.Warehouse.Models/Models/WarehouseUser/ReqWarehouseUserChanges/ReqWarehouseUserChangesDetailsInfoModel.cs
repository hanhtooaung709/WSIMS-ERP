namespace ERP.Warehouse.Models.Models.WarehouseUser.ReqWarehouseUserChanges;

public class ReqWarehouseUserChangesDetailsInfoModel
{
    // -- Change data --
    public string? ReqWarehouseUserChangesId { get; set; }
    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public string? StaffId { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? RoleName { get; set; }
    public string? BranchName { get; set; }

    // -- Old data --
    public string? OldName { get; set; }
    public string? OldFullName { get; set; }
    public string? OldStaffId { get; set; }
    public string? OldPhone { get; set; }
    public string? OldEmail { get; set; }
    public string? OldRole { get; set; }
    public string? OldBranch { get; set; }

    // -- Maker/Chacker --
    public string? Status { get; set; }
    public string? RejectReason { get; set; }
    public string? RequestedUser { get; set; }
    public string? RequestedDateTime { get; set; }
    public string? ApprovedUser { get; set; }
    public string? ApprovedDateTime { get; set; }
}
