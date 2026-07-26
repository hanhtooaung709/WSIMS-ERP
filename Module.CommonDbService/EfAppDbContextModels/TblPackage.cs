using System;
using System.Collections.Generic;

namespace Module.CommonDbService.EfAppDbContextModels;

public partial class TblPackage
{
    public string PackageId { get; set; } = null!;

    public string PackageInfoCode { get; set; } = null!;

    public int Quantity { get; set; }

    public string BranchCode { get; set; } = null!;

    public string CreatedUserId { get; set; } = null!;

    public DateTime CreatedDateTime { get; set; }

    public string? ModifiedUserId { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public int DelFlag { get; set; }
}
