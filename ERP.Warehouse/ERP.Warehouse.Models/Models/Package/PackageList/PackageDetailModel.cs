namespace ERP.Warehouse.Models.Models.Package.PackageList;

public class PackageDetailModel
{
    public List<DynamicReportModel> PackageInfo { get; set; }
    public List<DynamicReportModel> MakerChecker { get; set; }
    public string? ItemImagePath { get; set; }
}
