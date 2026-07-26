namespace ERP.Warehouse.Models.Models.Package.ReqPackage;

public class ReqPackageRepModel
{
    public List<ReqPackageModel> list { get; set; }
}

public class ReqPackageModel
{
    public string? ReqPackageId { get; set;}
    public string? PackageName { get; set; }
    public string? PackageInfoCode { get; set; }
    public string? BranchCode { get; set; }
    public int Quanity { get; set; }
    public string? ProductCode { get; set; }
    public string? Price { get; set; }
    public string? CurrencyCode { get; set; }
    public string? Weight { get; set; }
    public string? BoxCode { get; set; }
    public string? Status { get; set; }
}
