using System;
using System.Collections.Generic;

namespace Module.CommonDbService.EfAppDbContextModels;

public partial class TblReqPackage
{
    public string ReqPackageId { get; set; } = null!;

    public string PackageName { get; set; } = null!;

    public string ProductCode { get; set; } = null!;

    public int ProductsQuanity { get; set; }

    public int Price { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public int Weight { get; set; }

    public string BoxCode { get; set; } = null!;

    public string BranchCode { get; set; } = null!;

    public string SupplierName { get; set; } = null!;

    public byte[] Status { get; set; } = null!;

    public string? PackageId { get; set; }

    public string ReqUserId { get; set; } = null!;

    public DateTime ReqDateTime { get; set; }

    public string? RejectReason { get; set; }

    public string? ApprovedUserId { get; set; }

    public DateTime? ApprovedDateTime { get; set; }
}
