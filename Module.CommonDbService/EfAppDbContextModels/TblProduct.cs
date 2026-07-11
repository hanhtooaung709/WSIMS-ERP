using System;
using System.Collections.Generic;

namespace Module.CommonDbService.EfAppDbContextModels;

public partial class TblProduct
{
    public string ProductId { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string ProductCode { get; set; } = null!;

    public string SupplierName { get; set; } = null!;

    public string CreatedUserId { get; set; } = null!;

    public DateTime CreatedDateTime { get; set; }

    public string? ModifiedUserId { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public int DelFlag { get; set; }
}
