namespace ERP.Warehouse.Models.Models.Stock;

public class StockRepModel
{
    public List<StockModel> list { get; set; }
}

public class StockModel
{
    public string? ReqPackageId { get; set; }
    public string? PackageId { get; set; }
    public string? PackageName { get; set; }
    public string? PackageInfoCode { get; set; }
    public string? ProductCode { get; set; }
    public string? ChangesType { get; set; }
    public string? BoxCode { get; set; }
    public string? Price { get; set; }
    public int Quantity { get; set; }
    public string? BranchCode { get; set; }
    public string? Status { get; set; }
}
