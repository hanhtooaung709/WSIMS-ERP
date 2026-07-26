namespace ERP.Warehouse.Models.Models.Package.ReqPackage;

public class ReqPackageReqModel
{
    public string? ReqPackageId { get; set;}
    public string? PackageName { get; set; }
    public string? PackageInfoCode { get; set; }
    public string? BranchCode { get; set; }
    public int Quantity { get; set; }
    public string? ProductCode { get; set; }
    public string? Price { get; set; }
    public string? CurrencyCode { get; set; }
    public string? Weight { get; set; }
    public string? BoxCode { get; set; }
    public string? Status { get; set; }
}
