namespace ERP.Warehouse.Models.Models.Package.ReqPackage;

public class ReqPackageDetailModel
{
    public List<DynamicReportModel> PackageInfo { get; set; }
    public List<DynamicReportModel> MakerChecker { get; set; }
    public string? ItemImagePath { get; set; }
}
