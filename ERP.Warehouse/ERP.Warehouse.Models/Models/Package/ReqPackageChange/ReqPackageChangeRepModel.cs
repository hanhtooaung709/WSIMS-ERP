namespace ERP.Warehouse.Models.Models.Package.ReqPackageChange;

public class ReqPackageChangeRepModel
{
    public List<ReqPackageChangeModel> list { get; set; }
}

public class ReqPackageChangeModel
{
    public string? ReqPackageChangeId { get; set; }
    public string? PackageId { get; set; }
    public string? PackageName { get; set; }
    public string? PackageInfoCode { get; set; }
    public string? ProductCode { get; set; }
    public string? Price { get; set; }
    public string? CurrencyCode { get; set; }
    public string? Weight { get; set; }
    public string? ChangesType { get; set; }
    public string? BoxCode { get; set; }
    public string? Status { get; set; }
}
