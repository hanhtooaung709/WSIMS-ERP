using System;
using System.Collections.Generic;

namespace Module.CommonDbService.EfAppDbContextModels;

public partial class TblReqWarehouseUserChange
{
    public string ReqWarehouseUserChangesId { get; set; } = null!;

    public string WarehouseUserId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string RoleCode { get; set; } = null!;

    public string BranchCode { get; set; } = null!;

    public string ChangesType { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? RejectReason { get; set; }

    public string ReqUserId { get; set; } = null!;

    public DateTime ReqDateTime { get; set; }

    public string? ApprovedUserId { get; set; }

    public DateTime? ApprovedDateTime { get; set; }
}
