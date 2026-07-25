namespace ERP.Warehouse.Models.Models.Package.ReqPackageChange;

public class ReqPackageChangeDetailModel
{
    public List<DynamicReportModel> PackageInfo { get; set; }
    public List<DynamicReportModel> MakerChecker { get; set; }
    public string? ItemImagePath { get; set; }
}
