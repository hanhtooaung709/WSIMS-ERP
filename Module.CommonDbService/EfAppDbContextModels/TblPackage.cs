using System;
using System.Collections.Generic;

namespace Module.CommonDbService.EfAppDbContextModels;

public partial class TblPackage
{
    public string PackageId { get; set; } = null!;

    public string PackageName { get; set; } = null!;

    public string ProductCode { get; set; } = null!;

    public int ProductsQuanity { get; set; }

    public int Price { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public int Weight { get; set; }

    public string BoxCode { get; set; } = null!;

    public string BranchCode { get; set; } = null!;

    public string SupplierName { get; set; } = null!;

    public string CreatedUserId { get; set; } = null!;

    public DateTime CreatedDateTime { get; set; }

    public string? ModifiedUserId { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public int DispatchFlag { get; set; }

    public int DelFlag { get; set; }
}
