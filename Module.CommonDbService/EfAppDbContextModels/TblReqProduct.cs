using System;
using System.Collections.Generic;

namespace Module.CommonDbService.EfAppDbContextModels;

public partial class TblReqProduct
{
    public string ReqProductId { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string ProductCode { get; set; } = null!;

    public string SupplierName { get; set; } = null!;

    public string? ProductId { get; set; }

    public string Status { get; set; } = null!;

    public string ReqUserId { get; set; } = null!;

    public DateTime ReqDateTime { get; set; }

    public string? RejectReason { get; set; }

    public string? ApprovedUserId { get; set; }

    public DateTime? ApprovedDateTime { get; set; }
}
