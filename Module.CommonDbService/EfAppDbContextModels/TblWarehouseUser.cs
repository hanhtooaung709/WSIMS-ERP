using System;
using System.Collections.Generic;

namespace Module.CommonDbService.EfAppDbContextModels;

public partial class TblWarehouseUser
{
    public string WarehouseUserId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string RoleCode { get; set; } = null!;

    public string BranchCode { get; set; } = null!;

    public string LoginPassword { get; set; } = null!;

    public int FirstTimeLogin { get; set; }

    public int LoginFailCount { get; set; }

    public int LockFlag { get; set; }

    public string CreatedUserId { get; set; } = null!;

    public DateTime CreatedDateTime { get; set; }

    public string? ModifiedUserId { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public int DelFlag { get; set; }
}
