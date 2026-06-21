using System;
using System.Collections.Generic;

namespace Module.CommonDbService.EfAppDbContextModels;

public partial class TblReqStockTran
{
    public string ReqStockTranId { get; set; } = null!;

    public string PackageId { get; set; } = null!;

    public string BranchCode { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string ReqUserId { get; set; } = null!;

    public DateTime ReqDateTime { get; set; }

    public string? RejectReason { get; set; }

    public string? ModifiedUserId { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public int DelFlag { get; set; }
}
