using System;
using System.Collections.Generic;

namespace Module.CommonDbService.EfAppDbContextModels;

public partial class TblReqPackageInfo
{
    public string ReqPackageInfoId { get; set; } = null!;

    public string PackageName { get; set; } = null!;

    public string PackageInfoCode { get; set; } = null!;

    public int Quantity { get; set; }

    public string BranchCode { get; set; } = null!;

    public string ProductCode { get; set; } = null!;

    public int Price { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public int Weight { get; set; }

    public string BoxCode { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? PackageInfoId { get; set; }

    public string ReqUserId { get; set; } = null!;

    public DateTime ReqDateTime { get; set; }

    public string? RejectReason { get; set; }

    public string? ApprovedUserId { get; set; }

    public DateTime? ApprovedDateTime { get; set; }
}
