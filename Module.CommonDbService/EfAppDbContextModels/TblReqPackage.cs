using System;
using System.Collections.Generic;

namespace Module.CommonDbService.EfAppDbContextModels;

public partial class TblReqPackage
{
    public string ReqPackageId { get; set; } = null!;

    public string PackageId { get; set; } = null!;

    public string PackageInfoCode { get; set; } = null!;

    public int Quanity { get; set; }

    public string BranchCode { get; set; } = null!;

    public string ChangesType { get; set; } = null!;

    public byte[] Status { get; set; } = null!;

    public string ReqUserId { get; set; } = null!;

    public DateTime ReqDateTime { get; set; }

    public string? RejectReason { get; set; }

    public string? ApprovedUserId { get; set; }

    public DateTime? ApprovedDateTime { get; set; }
}
